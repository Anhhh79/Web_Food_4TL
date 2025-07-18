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
            // Kiểm tra UserId trong Session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "User"); // Chuyển hướng nếu chưa đăng nhập

            // Lưu địa chỉ và số điện thoại vào Session
            HttpContext.Session.SetString("DiaChiGiaoHang", DiaChiGiaoHang);
            HttpContext.Session.SetString("SoDienThoai", SoDienThoai);

            // Lấy giỏ hàng của người dùng hiện tại
            var gioHang = _context.GioHangs
                .Include(g => g.MonAn)
                .Where(g => g.NguoiDungId == userId) // Lọc theo UserId
                .ToList();

            if (!gioHang.Any())
            {
                ViewBag.ErrorMessage = "Giỏ hàng trống!";
                return View("~/Areas/Customer/Views/Cart/Index.cshtml", gioHang); // Trả về lại trang giỏ hàng với thông báo lỗi
            }

            // Tính tổng tiền
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
            string ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            // Lấy múi giờ Việt Nam (UTC+7)
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Windows
                                                                                                    // Nếu dùng Linux, dùng: TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");

            // Thời gian hiện tại theo múi giờ Việt Nam
            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            DateTime expireTime = startTime.AddMinutes(1440); // 24 giờ, thay vì 1,000,000 phút

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
        { "vnp_CreateDate", startTime.ToString("yyyyMMddHHmmss") },
        { "vnp_ExpireDate", expireTime.ToString("yyyyMMddHHmmss") }
    };

            // Tạo chuỗi dữ liệu để ký
            string signData = string.Join("&", pay.Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));
            string hash = ComputeHmacSHA512(vnp_HashSecret, signData);
            pay.Add("vnp_SecureHash", hash);

            // Tạo URL thanh toán
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
        public async Task<IActionResult> PaymentSuccess(string orderId)
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

            // GỬI EMAIL XÁC NHẬN
            var user = _context.NguoiDungs.FirstOrDefault(u => u.Id == userId.Value);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                try
                {
                    var mailHelper = new MailHelper(_configuration);
                    string subject = "Xác nhận đơn hàng";
                    string body = $@"
<div style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
  <div style='max-width: 600px; margin: auto; background: #ffffff; border: 1px solid #ddd; border-radius: 8px; overflow: hidden;'>
    <div style='background-color: #4CAF50; color: white; padding: 16px; text-align: center;'>
      <h2>Đặt hàng thành công </h2>
    </div>

    <div style='padding: 20px;'>
      <p>Chào <strong>{user.TenNguoiDung}</strong>,</p>
      <p>Chúng tôi xin xác nhận rằng đơn hàng <strong>#{hoaDon.Id}</strong> của bạn đã được thanh toán thành công.</p>

      <hr style='margin: 20px 0;' />

      <p><strong> Thông tin đơn hàng:</strong></p>
      <ul style='list-style: none; padding: 0;'>
        <li><strong> Địa chỉ giao hàng:</strong> {diaChi}</li>
        <li><strong> Số điện thoại:</strong> {soDienThoai}</li>
        <li><strong> Tổng tiền:</strong> {tongTien:N0} VND</li>
      </ul>

      <p style='margin-top: 20px;'>Nếu bạn có bất kỳ câu hỏi nào, xin vui lòng liên hệ với chúng tôi qua email hoặc hotline hỗ trợ.</p>

      <p style='color: #4CAF50;'><strong>Cảm ơn bạn đã mua hàng tại Web Food 4TL!</strong></p>
    </div>

    <div style='background-color: #f0f0f0; color: #888; text-align: center; padding: 12px; font-size: 12px;'>
      © 2025 Web Food 4TL. Mọi quyền được bảo lưu.
    </div>
  </div>
</div>";


                    mailHelper.SendOrderConfirmation(user.Email, subject, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Lỗi khi gửi email: " + ex.Message);
                }
            }

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
