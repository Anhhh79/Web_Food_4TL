using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Data;

namespace Web_Food_4TL.Areas.Customer.Controllers
{
    [Route("api/donhang")]
    [ApiController]
    public class DonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("danhsach")]
        public IActionResult GetDanhSach()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            Console.WriteLine($"UserId từ Session: {userId}");

            if (userId == null)
            {
                return Json(new { success = false, message = "Không tìm thấy UserId trong Session" });
            }

            var donHangs = _context.HoaDons
                .Where(hd => hd.NguoiDungId == userId)
                .OrderByDescending(hd => hd.NgayTao)
                .Select(hd => new
                {
                    hd.Id,
                    hd.TongTien,
                    hd.TrangThai,
                    hd.NgayTao,
                    hd.DiaChiGiaoHang,
                    hd.SoDienThoai,
                    hd.TrangThaiDonHang,
                    hd.TrangThaiGiaoHang,
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
