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
                .FirstOrDefaultAsync(h => h.Id == id);
            if (hd == null) return NotFound();

            return Json(new
            {
                hd.Id,
                hd.TrangThaiDonHang,
                hd.TrangThaiGiaoHang,
                hd.Lydo,
                hd.LyDoTuChoi,
                HoaDonChiTiets = hd.HoaDonChiTiets.Select(d => new {
                    d.TenMonAn,
                    d.SoLuong,
                    d.Gia
                })
            });
        }

        // POST: /Customer/HoaDon/CapNhatTrangThai/5
        [HttpPost("{id}")]
        public async Task<IActionResult> CapNhatTrangThai(int id, [FromBody] dynamic body)
        {
            var hd = await _context.HoaDons.FindAsync(id);
            if (hd == null) return NotFound();

            hd.TrangThaiDonHang = (string)body.TrangThaiDonHang;
            _context.Update(hd);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: /Customer/HoaDon/GuiYeuCauHoanTien/5
        [HttpPost("{id}")]
        public async Task<IActionResult> GuiYeuCauHoanTien(int id, [FromBody] dynamic body)
        {
            var hd = await _context.HoaDons.FindAsync(id);
            if (hd == null) return NotFound();

            hd.Lydo = (string)body.Lydo;
            _context.Update(hd);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
