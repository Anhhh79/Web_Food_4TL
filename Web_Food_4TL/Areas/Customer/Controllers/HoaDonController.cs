using Microsoft.AspNetCore.Mvc;
using Web_Food_4TL.Data;
using Web_Food_4TL.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Food_4TL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("Customer/[controller]/[action]")]
    public class HoaDonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoaDonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Customer/HoaDon/List
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new List<object>());

            var ds = await _context.HoaDons
                .Where(h => h.NguoiDungId == userId)
                .Select(h => new {
                    h.Id,
                    h.NgayTao,
                    h.TongTien,
                    h.TrangThaiDonHang
                })
                .ToListAsync();

            return Json(ds);
        }

        // GET: /Customer/HoaDon/ChiTiet/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ChiTiet(int id)
        {
            var hd = await _context.HoaDons
                .Include(h => h.HoaDonChiTiets)
                    .ThenInclude(ct => ct.MonAn)
                        .ThenInclude(ma => ma.AnhMonAnh)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (hd == null) return NotFound();

            return Json(new
            {
                hd.Id,
                hd.TrangThaiDonHang,
                hd.TrangThaiGiaoHang,
                hd.Lydo,
                hd.LyDoTuChoi,
                hd.SoDienThoai,
                hd.DiaChiGiaoHang,
                HoaDonChiTiets = hd.HoaDonChiTiets.Select(d => new {
                    d.MonAnId,
                    d.TenMonAn,
                    d.SoLuong,
                    d.Gia,
                    AnhMonAn = d.MonAn.AnhMonAnh != null && d.MonAn.AnhMonAnh.Count > 0 ? d.MonAn.AnhMonAnh.First().Url : null
                })
            });
        }


        // POST: /Customer/HoaDon/CapNhatTrangThai/5
        [HttpPost("CapNhatTrangThai/{id}")]
        public async Task<IActionResult> CapNhatTrangThai(int id, [FromBody] System.Text.Json.JsonElement body)
        {
            var hd = await _context.HoaDons.FindAsync(id);
            if (hd == null) return NotFound();

            string trangThai = body.GetProperty("trangThai").GetString();
            hd.TrangThaiDonHang = trangThai;
            if (trangThai == "Hoàn thành")
            {
                hd.NgayNhan = DateTime.Now;
            }

            _context.Update(hd);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // POST: /Customer/HoaDon/GuiYeuCauHoanTien/5
        [HttpPost("GuiYeuCauHoanTien/{id}")]
        public async Task<IActionResult> GuiYeuCauHoanTien(int id, [FromBody] System.Text.Json.JsonElement body)
        {
            try
            {
                var hd = await _context.HoaDons.FindAsync(id);
                if (hd == null) return NotFound();

                string lyDo = body.GetProperty("Lydo").GetString();
                hd.Lydo = lyDo;
                hd.TrangThaiDonHang = "Chờ đổi trả";
                _context.Update(hd);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Yêu cầu hoàn tiền đã được gửi thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });
            }
        }
    }
}
