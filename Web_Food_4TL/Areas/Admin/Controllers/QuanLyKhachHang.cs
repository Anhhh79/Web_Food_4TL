using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Web_Food_4TL.Data;

namespace Web_Food_4TL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/hoadon")]
    [ApiController]
    public class QuanLyKhachHang : Controller
    {


        private readonly ApplicationDbContext _context;

        public QuanLyKhachHang(ApplicationDbContext context) 
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("danhsach")]
        public IActionResult GetHoaDons()
        {
            var danhSachKhachHang = _context.NguoiDungs
        .GroupJoin(
            _context.HoaDons, // Join với bảng hóa đơn
            n => n.Id,        // Khóa chính của NguoiDungs
            h => h.NguoiDungId, // Khóa ngoại trong HoaDons
            (n, hoaDonGroup) => new
            {
                Id = n.Id,
                TenNguoiDung = n.TenNguoiDung,
                Email = n.Email,
                SoDienThoai = n.SoDienThoai,
                NgayTao = hoaDonGroup.Any()
                    ? hoaDonGroup.Min(h => h.NgayTao).ToString("dd/MM/yyyy")
                    : "Chưa mua hàng",  // Nếu không có hóa đơn
                TongTien = hoaDonGroup.Sum(h => (decimal?)h.TongTien) ?? 0
            }
        )
        .ToList();

            return Ok(danhSachKhachHang);
        }

        [HttpGet("ds/{nguoiDungId}")]
        public IActionResult GetDanhSach(string nguoiDungId) 
        {
            Console.WriteLine($"Lấy dữ liệu cho NguoiDungId: {nguoiDungId}");

            if (!int.TryParse(nguoiDungId, out int id))
            {
                return BadRequest(new { success = false, message = "ID không hợp lệ!" });
            }

            var donHangs = _context.HoaDons
                .Where(hd => hd.NguoiDungId == id)
                .OrderByDescending(hd => hd.NgayTao)
                .Select(hd => new
                {
                    hd.Id,
                    hd.TongTien,
                    hd.TrangThai,
                    hd.NgayTao,
                    hd.DiaChiGiaoHang,
                    hd.SoDienThoai,
                    ChiTiets = hd.HoaDonChiTiets.Select(hct => new
                    {
                        hct.Id,
                        hct.TenMonAn,
                        hct.SoLuong,
                        hct.Gia,
                        MonAn = new
                        {
                            hct.MonAn.Id,
                            hct.MonAn.TenMonAn,
                            hct.MonAn.MoTa,
                            hct.MonAn.Gia,
                            DanhMuc = hct.MonAn.DanhMuc.TenDanhMuc,
                            AnhMonAn = hct.MonAn.AnhMonAnh.OrderBy(a => a.Id).Select(a => a.Url).FirstOrDefault()
                        }
                    })
                })
                .ToList();

            return Json(new { success = true, data = donHangs });
        }
    }
}
