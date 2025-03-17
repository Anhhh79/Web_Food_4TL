using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Data;

namespace Web_Food_4TL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/thongke")]
    [ApiController]
    public class ThongKe : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThongKe(ApplicationDbContext context) 
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("tongtien")]
        public async Task<IActionResult> GetTongTien()
        {
            var tongTien = await _context.HoaDons.SumAsync(h => h.TongTien);
            return Ok(new { success = true, tongTien });
        }

        [HttpGet("monan")]
        public IActionResult GetSoLuongMonAn()
        {
            var soLuong = _context.MonAns.Count();
            return Ok(new { success = true, soLuong });
        }

        [HttpGet("nguoidung")]
        public IActionResult GetSoLuongNguoiDung()
        {
            var soLuong = _context.NguoiDungs.Count();
            return Ok(new { success = true, soLuong });
        }

        [HttpGet("thongke-theo-thang")]
        public IActionResult GetDoanhThuTheoThang()
        {
            var thongKe = _context.HoaDons
                .GroupBy(h => new { h.NgayTao.Year, h.NgayTao.Month })
                .Select(g => new
                {
                    Nam = g.Key.Year,
                    Thang = g.Key.Month,
                    TongDoanhThu = g.Sum(h => h.TongTien)
                })
                .OrderBy(g => g.Nam)
                .ThenBy(g => g.Thang)
                .ToList();

            return Ok(new { success = true, data = thongKe });
        }

    }
}
