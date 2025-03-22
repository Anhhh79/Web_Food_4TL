using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Food_4TL.Controllers
{
    [Area("Admin")]
    [Route("api/taikhoan")]
    [ApiController]
    public class TaiKhoanController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        private const int PageSize = 10;

        public TaiKhoanController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách tài khoản
        [HttpGet("DanhSach")]
        public async Task<IActionResult> DanhSach(int page = 1)
        {
            var totalUsers = await _context.NguoiDungs.CountAsync();
            var danhSachNguoiDung = await _context.NguoiDungs
                .OrderBy(nd => nd.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Lấy vai trò của từng người dùng
            var vaiTroDict = await _context.VaiTroNguoiDungs
                .Include(vtnd => vtnd.VaiTro)
                .ToDictionaryAsync(vtnd => vtnd.NguoiDungId, vtnd => vtnd.VaiTro.TenVaiTro);

            return Json(new
            {
                totalPages = (int)Math.Ceiling(totalUsers / (double)PageSize),
                currentPage = page,
                danhSachNguoiDung,
                vaiTroDict
            });
        }

        // View danh sách tài khoản
        [HttpGet("Index")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var danhSachNguoiDung = await _context.NguoiDungs
                .OrderBy(nd => nd.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling(await _context.NguoiDungs.CountAsync() / (double)PageSize);
            ViewBag.CurrentPage = page;
            ViewBag.VaiTroDict = await _context.VaiTroNguoiDungs
                .Include(vtnd => vtnd.VaiTro)
                .ToDictionaryAsync(vtnd => vtnd.NguoiDungId, vtnd => vtnd.VaiTro.TenVaiTro);
            ViewBag.VaiTroList = await _context.VaiTros.ToListAsync();

            return View("IndexTaiKhoan", danhSachNguoiDung);
        }

        // Thay đổi vai trò người dùng
        [HttpPost("ThayDoiVaiTro/{id}")]
        public async Task<IActionResult> ThayDoiVaiTro(int id, [FromBody] int vaiTroId)
        {
            var vaiTroNguoiDung = await _context.VaiTroNguoiDungs.FirstOrDefaultAsync(vtnd => vtnd.NguoiDungId == id);

            if (vaiTroNguoiDung == null)
            {
                vaiTroNguoiDung = new VaiTroNguoiDung
                {
                    NguoiDungId = id,
                    VaiTroId = vaiTroId
                };
                _context.VaiTroNguoiDungs.Add(vaiTroNguoiDung);
            }
            else
            {
                vaiTroNguoiDung.VaiTroId = vaiTroId;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // Đổi trạng thái tài khoản
        [HttpPost("DoiTrangThai/{id}")]
        public async Task<IActionResult> DoiTrangThai(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return Json(new { success = false, message = "Tài khoản không tồn tại!" });
            }

            nguoiDung.TrangThai = (nguoiDung.TrangThai == "Hoạt động") ? "Bị khóa" : "Hoạt động";
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // Xóa tài khoản
        [HttpDelete("XoaNguoiDung/{id}")]
        public async Task<IActionResult> XoaNguoiDung(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return Json(new { success = false, message = "Tài khoản không tồn tại!" });
            }

            var vaiTroNguoiDungs = _context.VaiTroNguoiDungs.Where(vtnd => vtnd.NguoiDungId == id);
            _context.VaiTroNguoiDungs.RemoveRange(vaiTroNguoiDungs);

            _context.NguoiDungs.Remove(nguoiDung);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
