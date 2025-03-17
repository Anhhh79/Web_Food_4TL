using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
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
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0; // Giả sử ID user (cần thay thế bằng ID từ session hoặc Identity)
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

        //Them vao gio hang
        [HttpPost]
        public async Task<IActionResult> AddToCart(int monAnId, int quantity)
        {
            if (monAnId <= 0 || quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0; // Dữ liệu mẫu, sau này lấy từ Session hoặc Identity
            if (userId == 0) {
                return Json(new { success = false, needLogin = true, message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng!" });
            }

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

        //xoa mon an khoi gio hang
        [HttpPost]
        public async Task<IActionResult> XoaMonAn(int id)
        {
            try
            {
                // Giả sử bạn lấy ID người dùng từ session hoặc claims
                int? nguoiDungId = HttpContext.Session.GetInt32("UserId");
                if (nguoiDungId == null)
                {
                    return Json(new { success = false, message = "Bạn chưa đăng nhập!" });
                }

                // Tìm món ăn trong giỏ hàng của người dùng
                var item = await _context.GioHangs
                    .FirstOrDefaultAsync(x => x.Id == id && x.NguoiDungId == nguoiDungId);
                Console.WriteLine("id mon an: " + item);
                if (item != null)
                {
                    _context.GioHangs.Remove(item);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, message = "Xóa thành công!" });
            }
            catch (Exception)
            {
                return BadRequest(new { success = false, message = "Lỗi server!" });
            }
        }



    }
}
