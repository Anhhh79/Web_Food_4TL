using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Web_Food_4TL.Data;
using Web_Food_4TL.Models;

namespace Web_Food_4TL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị giỏ hàng
        public async Task<IActionResult> Index()
        {
            int userId = 1; // Giả sử ID user (cần thay thế bằng ID từ session hoặc Identity)
            var cartItems = await _context.GioHangs
                .Include(g => g.MonAn)
                .Where(g => g.NguoiDungId == userId)
                .ToListAsync();

            foreach (var item in cartItems)
            {
                Console.WriteLine($"Món: {item.MonAn.TenMonAn}, Giá: {item.Gia}, Số lượng: {item.SoLuong}");
            }

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int monAnId, int quantity)
        {
            if (monAnId <= 0 || quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            int userId = 1; // Dữ liệu mẫu, sau này lấy từ Session hoặc Identity

            var cartItem = await _context.GioHangs
                .FirstOrDefaultAsync(g => g.MonAnId == monAnId && g.NguoiDungId == userId);

            if (cartItem != null)
            {
                cartItem.SoLuong += quantity;
            }
            else
            {
                var monAn = await _context.MonAns.FindAsync(monAnId);
                if (monAn == null)
                {
                    return NotFound(new { success = false, message = "Món ăn không tồn tại!" });
                }

                if (monAn.Gia <= 0) // Kiểm tra giá hợp lệ
                {
                    return BadRequest(new { success = false, message = "Giá món ăn không hợp lệ!" });
                }

                cartItem = new GioHang
                {
                    NguoiDungId = userId,
                    MonAnId = monAnId,
                    SoLuong = quantity,
                    Gia = monAn.Gia
                };
                _context.GioHangs.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            // Tính tổng số lượng giỏ hàng
            var cartCount = await _context.GioHangs
                .Where(g => g.NguoiDungId == userId)
                .SumAsync(g => g.SoLuong);

            return Json(new { success = true, message = "Thêm vào giỏ hàng thành công!", cartCount });
        }

        

       
    }
}
