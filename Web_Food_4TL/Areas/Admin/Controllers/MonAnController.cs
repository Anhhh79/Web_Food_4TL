using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web_Food_4TL.Data;
using Web_Food_4TL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web_Food_4TL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MonAnController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MonAnController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;
            int totalItems = await _context.MonAns.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var monAnList = await _context.MonAns
                .Include(m => m.DanhMuc)
                .Include(m => m.AnhMonAnh)
                .OrderBy(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.DanhMucList = await _context.DanhMucs.ToListAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            return View("IndexMonAn", monAnList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [Bind("TenMonAn,MoTa,Gia,DanhMucId")] MonAn monAn,
            List<IFormFile> files)
        {
            ModelState.Remove("DanhMuc");
            ModelState.Remove("AnhMonAnh");

            if (!_context.DanhMucs.Any(d => d.Id == monAn.DanhMucId))
            {
                return Json(new { success = false, message = "Danh mục không hợp lệ." });
            }

            // ——————————————
            // VALIDATION CHO GIÁ
            if (monAn.Gia <= 0)
            {
                ModelState.AddModelError(nameof(monAn.Gia), "Giá phải là số nguyên dương.");
            }
            // ——————————————

            if (ModelState.IsValid)
            {
                _context.MonAns.Add(monAn);
                await _context.SaveChangesAsync();

                if (files?.Any() == true)
                {
                    await SaveImages(monAn.Id, files);
                }
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,TenMonAn,MoTa,Gia,DanhMucId")] MonAn monAn,
            List<IFormFile> files)
        {
            // Bỏ qua validation cho DanhMuc và AnhMonAnh
            ModelState.Remove("DanhMuc");
            ModelState.Remove("AnhMonAnh");

            var existingMonAn = await _context.MonAns
                .Include(m => m.AnhMonAnh)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (existingMonAn == null)
            {
                return Json(new { success = false, message = "Món ăn không tồn tại." });
            }

            // ——————————————
            // VALIDATION CHO GIÁ
            if (monAn.Gia <= 0)
            {
                ModelState.AddModelError(nameof(monAn.Gia), "Giá phải là số nguyên dương.");
            }
            // ——————————————

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Lỗi dữ liệu", errors });
            }

            // Xử lý xóa ảnh theo checkbox "imagesToDelete" từ form
            var imagesToDelete = Request.Form["imagesToDelete"].ToList();
            if (imagesToDelete.Any())
            {
                foreach (var imgIdStr in imagesToDelete)
                {
                    if (int.TryParse(imgIdStr, out int imgId))
                    {
                        var image = existingMonAn.AnhMonAnh.FirstOrDefault(a => a.Id == imgId);
                        if (image != null)
                        {
                            string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "monan", image.Url);
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);

                            _context.AnhMonAns.Remove(image);
                        }
                    }
                }
            }

            // Cập nhật các thuộc tính của món ăn
            existingMonAn.TenMonAn = monAn.TenMonAn;
            existingMonAn.MoTa = monAn.MoTa;
            existingMonAn.Gia = monAn.Gia;
            existingMonAn.DanhMucId = monAn.DanhMucId;

            // Nếu có file mới, lưu ảnh mới
            if (files?.Any() == true)
            {
                await SaveImages(existingMonAn.Id, files);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var monAn = await _context.MonAns
                .Include(m => m.AnhMonAnh)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monAn == null)
            {
                return Json(new { success = false, message = "Món ăn không tồn tại" });
            }

            foreach (var anh in monAn.AnhMonAnh)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "monan", anh.Url);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _context.AnhMonAns.RemoveRange(monAn.AnhMonAnh);
            _context.MonAns.Remove(monAn);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        private async Task SaveImages(int monAnId, List<IFormFile> files)
        {
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "monan");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var anhMonAn = new AnhMonAn
                    {
                        MonAnId = monAnId,
                        Url = fileName
                    };
                    _context.AnhMonAns.Add(anhMonAn);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
