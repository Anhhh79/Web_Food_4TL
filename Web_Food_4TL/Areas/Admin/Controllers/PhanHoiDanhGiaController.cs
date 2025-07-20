using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Data;

namespace Web_Food_4TL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhanHoiDanhGiaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhanHoiDanhGiaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LayDanhSachDanhGia()
        {
            var danhSach = _context.DanhGias
                .Include(d => d.NguoiDung)
                .Include(d => d.MonAnh)
                .OrderByDescending(d => d.ThoiGian)
                .Select(d => new
                {
                    d.Id,
                    TenNguoiDung = d.NguoiDung.TenNguoiDung,
                    SoDienThoai = d.NguoiDung.SoDienThoai,
                    NoiDungDanhGia = d.NoiDungDanhGia,
                    SoSao = d.SoSao,
                    TenMonAn = d.MonAnh.TenMonAn,
                    ThoiGian = d.ThoiGian,
                    NoiDungPhanHoi = d.NoiDungPhanHoi
                })
                .ToList();

            return Ok(danhSach);
        }

        [HttpPost]
        public IActionResult GuiPhanHoi(int id, string noiDungPhanHoi)
        {
            var danhGia = _context.DanhGias.FirstOrDefault(d => d.Id == id);
            if (danhGia == null)
            {
                return NotFound();
            }

            danhGia.NoiDungPhanHoi = noiDungPhanHoi;
            _context.SaveChanges();

            return Ok(new { message = "Phản hồi đã lưu" });
        }

    }
}
