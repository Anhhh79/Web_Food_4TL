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
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null || userId == 0)
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng nếu chưa đăng nhập
            }

            var cartItems = await _context.GioHangs
                .Include(g => g.MonAn)
                .Where(g => g.NguoiDungId == userId)
                .ToListAsync();

            return View(cartItems);
        }

        // Thêm vào giỏ hàng
        [HttpPost]
        public async Task<IActionResult> AddToCart(int monAnId, int quantity)
        {
            if (monAnId <= 0 || quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId == 0)
            {
                return Json(new { success = false, needLogin = true, message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng!" });
            }

            var monAn = await _context.MonAns.FindAsync(monAnId);
            if (monAn == null)
            {
                return NotFound(new { success = false, message = "Món ăn không tồn tại!" });
            }

            if (monAn.Gia <= 0)
            {
                return BadRequest(new { success = false, message = "Giá món ăn không hợp lệ!" });
            }

            // Kiểm tra món ăn đã có trong giỏ hàng chưa
            var cartItem = await _context.GioHangs
                .FirstOrDefaultAsync(g => g.MonAnId == monAnId && g.NguoiDungId == userId);

            if (cartItem != null)
            {
                cartItem.SoLuong += quantity;
                cartItem.Gia = monAn.Gia; // Cập nhật giá mới nếu có thay đổi
            }
            else
            {
                cartItem = new GioHang
                {
                    NguoiDungId = userId.Value,
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

        // Xóa món ăn khỏi giỏ hàng
        [HttpPost]
        public async Task<IActionResult> XoaMonAn(int id)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null || userId == 0)
                {
                    return Json(new { success = false, message = "Bạn chưa đăng nhập!" });
                }

                var item = await _context.GioHangs
                    .FirstOrDefaultAsync(x => x.Id == id && x.NguoiDungId == userId);

                if (item == null)
                {
                    return Json(new { success = false, message = "Món ăn không tồn tại trong giỏ hàng!" });
                }

                _context.GioHangs.Remove(item);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Xóa thành công!" });
            }
            catch (Exception)
            {
                return BadRequest(new { success = false, message = "Lỗi server!" });
            }
        }
    }
}
