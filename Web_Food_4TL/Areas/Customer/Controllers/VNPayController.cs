using Microsoft.AspNetCore.Mvc;
using Web_Food_4TL.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Data;
using System.Net;

namespace Web_Food_4TL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class VnPayController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public VnPayController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Lấy ID người dùng từ session
        private int? GetCurrentUserId()
        {
            var userId = HttpContext.Session.GetInt32("UserId"); // Đổi từ "NguoiDungId" thành "UserId"
            Console.WriteLine($"UserID từ session: {userId}"); // Debug kiểm tra
            return userId;
        }



        [HttpPost]
        public IActionResult Payment(string DiaChiGiaoHang, string SoDienThoai)
        {
            // Lưu địa chỉ và số điện thoại vào session
            HttpContext.Session.SetString("DiaChiGiaoHang", DiaChiGiaoHang);
            HttpContext.Session.SetString("SoDienThoai", SoDienThoai);

            // Lấy user ID
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Bạn chưa đăng nhập!");

            // Lấy giỏ hàng của user
            var gioHang = _context.GioHangs
                .Include(g => g.MonAn)
                .Where(g => g.NguoiDungId == userId.Value)
                .ToList();

            if (!gioHang.Any()) return NotFound("Giỏ hàng trống!");

            decimal tongTien = gioHang.Sum(g => g.Gia * g.SoLuong);
            return RedirectToAction("ProcessVnPay", new { tongTien });
        }

        public IActionResult ProcessVnPay(double tongTien)
        {
            string vnp_Returnurl = $"{Request.Scheme}://{Request.Host}/Customer/VnPay/PaymentCallback";
            string vnp_Url = _configuration["VNPay:Url"];
            string vnp_TmnCode = _configuration["VNPay:TmnCode"];
            string vnp_HashSecret = _configuration["VNPay:HashSecret"]?.Trim();

            string amount = ((int)(tongTien * 100)).ToString();
            string orderId = DateTime.Now.Ticks.ToString();

            // Lấy địa chỉ IP thực tế nếu có proxy
            string ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            var pay = new SortedDictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", amount },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", orderId },
                { "vnp_OrderInfo", "Thanh toán đơn hàng" },
                { "vnp_OrderType", "billpayment" },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", vnp_Returnurl },
                { "vnp_IpAddr", ipAddress },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
            };

            string signData = string.Join("&", pay.Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));
            string hash = ComputeHmacSHA512(vnp_HashSecret, signData);
            pay.Add("vnp_SecureHash", hash);

            string paymentUrl = $"{vnp_Url}?{string.Join("&", pay.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"))}";
            return Redirect(paymentUrl);
        }

        private static string ComputeHmacSHA512(string key, string data)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        [HttpGet]
        public IActionResult PaymentCallback()
        {
            string vnp_ResponseCode = Request.Query["vnp_ResponseCode"];
            string vnp_TxnRef = Request.Query["vnp_TxnRef"];

            if (vnp_ResponseCode == "00")
            {
                return RedirectToAction("PaymentSuccess", new { orderId = vnp_TxnRef });
            }
            else
            {
                return RedirectToAction("PaymentFail");
            }
        }

        [HttpGet]
        public IActionResult PaymentSuccess(string orderId)
        {
            string diaChi = HttpContext.Session.GetString("DiaChiGiaoHang") ?? "Không có";
            string soDienThoai = HttpContext.Session.GetString("SoDienThoai") ?? "Không có";

            var userId = GetCurrentUserId();
            if (userId == null || !_context.NguoiDungs.Any(u => u.Id == userId.Value))
            {
                return BadRequest("Người dùng không hợp lệ!");
            }

            var gioHang = _context.GioHangs
                .Include(g => g.MonAn)
                .Where(g => g.NguoiDungId == userId.Value)
                .ToList();

            if (!gioHang.Any()) return NotFound("Giỏ hàng trống!");

            decimal tongTien = gioHang.Sum(g => g.MonAn.Gia * g.SoLuong);

            var hoaDon = new HoaDon
            {
                NgayTao = DateTime.Now,
                TrangThai = "Đã thanh toán",
                NguoiDungId = userId.Value,
                DiaChiGiaoHang = diaChi,
                SoDienThoai = soDienThoai,
                TongTien = tongTien
            };

            _context.HoaDons.Add(hoaDon);
            _context.SaveChanges();

            foreach (var item in gioHang)
            {
                var chiTiet = new HoaDonChiTiet
                {
                    HoaDonId = hoaDon.Id,
                    MonAnId = item.MonAnId,
                    TenMonAn = item.MonAn?.TenMonAn ?? "Không xác định",
                    SoLuong = item.SoLuong,
                    Gia = item.MonAn?.Gia ?? 0
                };

                _context.HoaDonChiTiets.Add(chiTiet);
            }

            _context.GioHangs.RemoveRange(gioHang);
            _context.SaveChanges();

            return View("Success");
        }

        [HttpGet]
        public IActionResult PaymentFail()
        {
            return View("Fail");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
