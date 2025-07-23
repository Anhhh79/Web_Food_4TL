using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Data;

namespace Web_Food_4TL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DanhGiaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhGiaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetAverageRatingAndDanhGias(int monAnId)
        {
            var danhGias = await _context.DanhGias
                .Where(d => d.MonAnId == monAnId)
                .Include(d => d.NguoiDung)
                .ToListAsync();

            // Tính toán thống kê từ TẤT CẢ đánh giá (kể cả "Chưa phản hồi")
            var totalReviews = danhGias.Count;
            var averageRating = danhGias.Any() ? Math.Round(danhGias.Average(d => d.SoSao), 1) : 0;

            var fiveStar = danhGias.Count(d => d.SoSao == 5);
            var fourStar = danhGias.Count(d => d.SoSao == 4);
            var threeStar = danhGias.Count(d => d.SoSao == 3);
            var twoStar = danhGias.Count(d => d.SoSao == 2);
            var oneStar = danhGias.Count(d => d.SoSao == 1);

            // Lọc danh sách đánh giá: chỉ lấy những cái KHÁC "Chưa phản hồi"
            var danhGiasHienThi = danhGias
                .Where(d => d.NoiDungDanhGia != "Chưa phản hồi")
                .Select(d => new
                {
                    TenNguoiDung = d.NguoiDung.TenNguoiDung,
                    SoSao = d.SoSao,
                    NoiDung = d.NoiDungDanhGia,
                    NoiDungPhanHoi = d.NoiDungPhanHoi,
                    ThoiGian = d.ThoiGian.ToString("dd/MM/yyyy HH:mm") // Hoặc CreatedAt nếu bạn có
                })
                .ToList();

            return Json(new
            {
                averageRating,
                totalReviews,
                fiveStar,
                fourStar,
                threeStar,
                twoStar,
                oneStar,
                danhGias = danhGiasHienThi
            });
        }



    }
}
