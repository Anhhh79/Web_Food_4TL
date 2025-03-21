using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Data;
using Web_Food_4TL.Models;

namespace Web_Food_4TL.Areas.Customer.Controllers
{
    [Route("customer/api/chat")]
    [ApiController]
    [Area("Customer")]
    public class CustomerChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class SendMessageDto
        {
            public string MessageText { get; set; }
        }

        private int? GetCurrentUserId()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            Console.WriteLine($"UserID từ session: {userId}"); // Debug
            return userId;
        }

        [HttpGet("check-session")]
        public IActionResult CheckSession()
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new { error = "Session không tồn tại hoặc đã hết hạn." });
            }

            return Ok(new { userId });
        }


        public class SendMessageCustomerDto
        {
            public int MessageToAdminId { get; set; }  // AdminId nhận tin (nếu cần)
            public string MessageText { get; set; }
        }

        [HttpPost("send/admins")]
        public async Task<IActionResult> SendMessageToAdmins([FromBody] SendMessageCustomerDto request)
        {
            if (string.IsNullOrWhiteSpace(request.MessageText))
            {
                return BadRequest(new { error = "Nội dung tin nhắn không hợp lệ." });
            }

            var customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                return Unauthorized(new { error = "Bạn chưa đăng nhập khách hàng!" });
            }

            try
            {
                // Lấy danh sách các admin có vai trò "Quản lý"
                var adminUsers = await (from nd in _context.NguoiDungs
                                        join vtn in _context.VaiTroNguoiDungs on nd.Id equals vtn.NguoiDungId
                                        join vt in _context.VaiTros on vtn.VaiTroId equals vt.Id
                                        where vt.TenVaiTro == "Quản lý"
                                        select nd)
                                       .ToListAsync();

                if (!adminUsers.Any())
                {
                    return NotFound(new { error = "Không tìm thấy quản lý nào." });
                }

                var messages = new List<TinNhan>();

                foreach (var admin in adminUsers)
                {
                    var message = new TinNhan
                    {
                        NoiDung = request.MessageText,
                        LaTinNhanTuKhach = true,
                        NguoiGuiId = customerId,
                        NguoiNhanId = admin.Id,
                        NguoiDungId = customerId,
                        ThoiGianGui = DateTime.Now
                    };

                    messages.Add(message);
                }

                _context.TinNhans.AddRange(messages);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, count = messages.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi máy chủ nội bộ.", details = ex.Message });
            }
        }


        [HttpGet("messages")]
        public async Task<IActionResult> GetUserMessages()
        {
            try
            {
                var nguoiDungId = GetCurrentUserId();

                if (nguoiDungId == null)
                {
                    return Unauthorized(new { error = "Bạn cần đăng nhập." });
                }

                var messages = await _context.TinNhans
                    .Where(m => m.NguoiGuiId == nguoiDungId.Value || m.NguoiNhanId == nguoiDungId.Value)
                    .OrderBy(m => m.ThoiGianGui)
                    .Select(m => new
                    {
                        id = m.Id,
                        noiDung = m.NoiDung,
                        thoiGianGui = m.ThoiGianGui,
                        laTinNhanTuKhach = m.LaTinNhanTuKhach
                    })
                    .ToListAsync();

                return Ok(new { nguoiDungId, messages });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi máy chủ nội bộ.", details = ex.Message });
            }
        }

    }
}
