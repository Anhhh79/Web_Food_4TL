using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Data;
using Web_Food_4TL.Models;

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


        [HttpPost("DanhGia")]
        public IActionResult CreateDanhGia([FromForm] string PhoneNumber, [FromForm] string Content, [FromForm] int Rating, [FromForm] int MonAnId)
        {
            try
            {
                var nguoiDungId = HttpContext.Session.GetInt32("UserId");
                if (nguoiDungId == null)
                {
                    return Json(new { success = false, message = "Bạn chưa đăng nhập." });
                }

                // 👉 Log giá trị để chắc chắn!
                Console.WriteLine($"☎️ PhoneNumber: {PhoneNumber}");
                Console.WriteLine($"📝 Content: {Content}");
                Console.WriteLine($"⭐ Rating: {Rating}");
                Console.WriteLine($"🍜 MonAnId: {MonAnId}");
                Console.WriteLine($"👤 NguoiDungId (Session): {nguoiDungId}");

                var hopLe = _context.HoaDons
                    .Where(hd => hd.SoDienThoai == PhoneNumber && hd.NguoiDungId == nguoiDungId)
                    .Join(
                        _context.HoaDonChiTiets,
                        hd => hd.Id,
                        ct => ct.HoaDonId,
                        (hd, ct) => new { hd, ct }
                    )
                    .Any(joined => joined.ct.MonAnId == MonAnId);

                Console.WriteLine($"✅ HopLe: {hopLe}");

                if (!hopLe)
                {
                    return Json(new { success = false, message = "Số điện thoại không đúng hoặc món ăn chưa được đặt." });
                }

                var danhGia = new DanhGia
                {
                    NoiDungDanhGia = Content,
                    SoSao = Rating,
                    MonAnId = MonAnId,
                    NguoiDungId = nguoiDungId.Value,
                    NoiDungPhanHoi = "Chưa phản hồi",
                    ThoiGian = DateTime.Now
                };

                _context.DanhGias.Add(danhGia);
                _context.SaveChanges();

                return Json(new { success = true, message = "Đánh giá thành công!" });
            }
            catch (DbUpdateException dbEx)
            {
                var inner = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine("💥 DB ERROR: " + inner);
                return Json(new { success = false, message = "DbUpdateException: " + inner });
            }
            catch (Exception ex)
            {
                Console.WriteLine("💥 EXCEPTION: " + ex.Message);
                return Json(new { success = false, message = "Exception: " + ex.Message });
            }
        }



    }
}
