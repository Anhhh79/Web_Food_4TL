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

        // =============================
        //  Danh sách món ăn (phân trang)
        // =============================
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

        // =============================
        //  Thêm món ăn (POST)
        // =============================
        [HttpPost]
        public async Task<IActionResult> Create(
            [Bind("TenMonAn,MoTa,Gia,DanhMucId")] MonAn monAn,
            List<IFormFile> files)
        {
            // Bỏ validate các trường không bind
            ModelState.Remove("DanhMuc");
            ModelState.Remove("AnhMonAnh");

            // Kiểm tra DanhMucId
            if (!_context.DanhMucs.Any(d => d.Id == monAn.DanhMucId))
            {
                return Json(new { success = false, message = "Danh mục không hợp lệ." });
            }

            if (ModelState.IsValid)
            {
                _context.MonAns.Add(monAn);
                await _context.SaveChangesAsync();

                // Lưu ảnh nếu có
                if (files != null && files.Count > 0)
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

        // =============================
        //  Hiển thị form sửa (GET) - SỬA ĐỂ TRẢ VỀ JSON
        // =============================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Tìm món ăn trong DB
            var monAn = await _context.MonAns
                .Include(m => m.AnhMonAnh)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (monAn == null)
                return NotFound();

            // *** SỬA: Trả về JSON thay vì PartialView ***
            var data = new
            {
                id = monAn.Id,
                tenMonAn = monAn.TenMonAn,
                moTa = monAn.MoTa,
                gia = monAn.Gia,
                danhMucId = monAn.DanhMucId,
                anhMonAnh = monAn.AnhMonAnh?.Select(a => new
                {
                    id = a.Id,
                    url = a.Url
                }).ToList()
            };

            return Json(data);
        }

        // =============================
        //  Xử lý sửa món ăn (POST)
        // =============================
        [HttpPost]
        public async Task<IActionResult> Edit(
            [Bind("Id,TenMonAn,MoTa,Gia,DanhMucId")] MonAn monAn,
            List<IFormFile> files,
            List<int> imagesToDelete) // Danh sách ảnh cũ cần xóa
        {
            ModelState.Remove("DanhMuc");
            ModelState.Remove("AnhMonAnh");

            // Kiểm tra DanhMucId
            if (!_context.DanhMucs.Any(d => d.Id == monAn.DanhMucId))
            {
                return Json(new { success = false, message = "Danh mục không hợp lệ." });
            }

            if (ModelState.IsValid)
            {
                // Lấy dữ liệu cũ
                var monAnDb = await _context.MonAns
                    .Include(m => m.AnhMonAnh)
                    .FirstOrDefaultAsync(m => m.Id == monAn.Id);

                if (monAnDb == null)
                {
                    return Json(new { success = false, message = "Món ăn không tồn tại." });
                }

                // Cập nhật thông tin
                monAnDb.TenMonAn = monAn.TenMonAn;
                monAnDb.MoTa = monAn.MoTa;
                monAnDb.Gia = monAn.Gia;
                monAnDb.DanhMucId = monAn.DanhMucId;

                // Xóa ảnh cũ (nếu chọn)
                if (imagesToDelete != null && imagesToDelete.Any())
                {
                    var images = monAnDb.AnhMonAnh
                        .Where(a => imagesToDelete.Contains(a.Id))
                        .ToList();

                    foreach (var img in images)
                    {
                        // Xóa file vật lý
                        string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "monan", img.Url);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    _context.AnhMonAns.RemoveRange(images);
                }

                // Thêm ảnh mới nếu có
                if (files != null && files.Count > 0)
                {
                    await SaveImages(monAnDb.Id, files);
                }

                await _context.SaveChangesAsync();
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

        // =============================
        //  Xóa món ăn
        // =============================
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

            // Xóa file ảnh vật lý
            foreach (var anh in monAn.AnhMonAnh)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "monan", anh.Url);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            // Xóa record ảnh trong DB
            _context.AnhMonAns.RemoveRange(monAn.AnhMonAnh);

            // Xóa món ăn
            _context.MonAns.Remove(monAn);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // =============================
        //  Lưu ảnh vào thư mục + DB
        // =============================
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
