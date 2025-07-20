using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Data;
using Web_Food_4TL.Models;

namespace Web_Food_4TL.Areas.Admin.Controllers
{
    [Route("admin/api/chat")]
    [ApiController]
    [Area("Admin")]
    public class AdminChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang quản lý chat
        public IActionResult Index()
        {
            return View();
        }

        // Lấy danh sách tất cả người dùng đã từng nhắn tin
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsersChat()
        {
            var users = await _context.TinNhans
                .Include(m => m.NguoiDung)
                .Where(m => m.NguoiDungId != null)
                .GroupBy(m => m.NguoiDungId)
                .Select(g => new
                {
                    nguoiDungId = g.Key,
                    tenNguoiDung = g.FirstOrDefault().NguoiDung.TenNguoiDung,
                    soLuongTinMoi = g.Count(m => m.LaTinNhanTuKhach && !m.DaDoc),
                    thoiGianMoiNhat = g.Max(m => m.ThoiGianGui)
                })
                .OrderByDescending(g => g.thoiGianMoiNhat)
                .ToListAsync();

            return Ok(users);
        }




        // Lấy toàn bộ tin nhắn giữa Admin và một người dùng cụ thể
        [HttpGet("messages/{nguoiDungId}")]
        public async Task<IActionResult> GetMessagesWithUser(int nguoiDungId)
        {
            var messages = await _context.TinNhans
                .Where(m => m.NguoiDungId == nguoiDungId)
                .OrderBy(m => m.ThoiGianGui)
                .Select(m => new
                {
                    id = m.Id,
                    noiDung = m.NoiDung,
                    thoiGianGui = m.ThoiGianGui,
                    laTinNhanTuKhach = m.LaTinNhanTuKhach
                })
                .ToListAsync();

            if (messages == null || messages.Count == 0)
                return NotFound(new { error = "Không tìm thấy tin nhắn với người dùng này." });

            return Ok(new { nguoiDungId, messages });
        }


        // DTO để nhận thông tin gửi từ Admin
        public class SendMessageAdminDto
        {
            public int NguoiDungId { get; set; }    // ID người nhận
            public string MessageText { get; set; } // Nội dung tin nhắn
        }

        // Admin gửi tin nhắn cho khách hàng
        [HttpPost("send/customer")]
        public async Task<IActionResult> SendMessageToCustomer([FromBody] SendMessageAdminDto request)
        {
            if (string.IsNullOrWhiteSpace(request.MessageText))
            {
                return BadRequest(new { error = "Nội dung tin nhắn không hợp lệ." });
            }

            var userExists = await _context.NguoiDungs.AnyAsync(u => u.Id == request.NguoiDungId);
            if (!userExists)
            {
                return NotFound(new { error = "Người dùng không tồn tại." });
            }

            // Lấy AdminId từ session (quan trọng)
            var adminId = HttpContext.Session.GetInt32("UserId");
            if (adminId == null)
            {
                return Unauthorized(new { error = "Bạn chưa đăng nhập admin!" });
            }

            try
            {
                var message = new TinNhan
                {
                    NoiDung = request.MessageText,
                    LaTinNhanTuKhach = false,             // Admin gửi
                    NguoiGuiId = adminId,                 // Admin là người gửi
                    NguoiNhanId = request.NguoiDungId,    // Khách hàng là người nhận
                    NguoiDungId = request.NguoiDungId,    // Khách hàng là chủ cuộc trò chuyện
                    ThoiGianGui = DateTime.Now
                };

                _context.TinNhans.Add(message);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, messageId = message.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi máy chủ nội bộ.", details = ex.Message });
            }
        }

        [HttpPost("mark-read/{nguoiDungId}")]
        public async Task<IActionResult> MarkMessagesAsRead(int nguoiDungId)
        {
            var messages = await _context.TinNhans
                .Where(m => m.NguoiDungId == nguoiDungId && m.LaTinNhanTuKhach && !m.DaDoc)
                .ToListAsync();

            foreach (var msg in messages)
            {
                msg.DaDoc = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
