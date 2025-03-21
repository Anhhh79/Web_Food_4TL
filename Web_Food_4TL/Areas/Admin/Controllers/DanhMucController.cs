using Microsoft.AspNetCore.Mvc;
using Web_Food_4TL.Models;
using Web_Food_4TL.Data;
using System.Linq;

namespace Web_Food_4TL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/DanhMuc")]
    public class DanhMucController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhMucController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            var danhMucs = _context.DanhMucs.ToList();
            return View("IndexDanhMuc",danhMucs);
        }

        [HttpPost("ThemDanhMuc")]
        public IActionResult ThemDanhMuc([FromBody] DanhMuc model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.TenDanhMuc))
            {
                return BadRequest(new { success = false, message = "Tên danh mục không hợp lệ" });
            }

            _context.DanhMucs.Add(model);
            _context.SaveChanges();

            return PartialView("_DanhMucTable", _context.DanhMucs.ToList());
        }

        [HttpPost("SuaDanhMuc")]
        public IActionResult SuaDanhMuc([FromBody] DanhMuc model)
        {
            var danhMuc = _context.DanhMucs.Find(model.Id);
            if (danhMuc == null)
            {
                return NotFound(new { success = false, message = "Danh mục không tồn tại" });
            }

            danhMuc.TenDanhMuc = model.TenDanhMuc;
            _context.SaveChanges();

            return PartialView("_DanhMucTable", _context.DanhMucs.ToList());
        }

        [HttpPost("XoaDanhMuc")]
        public IActionResult XoaDanhMuc([FromBody] DanhMuc model)
        {
            var danhMuc = _context.DanhMucs.Find(model.Id);
            if (danhMuc == null)
            {
                return NotFound(new { success = false, message = "Danh mục không tồn tại" });
            }

            _context.DanhMucs.Remove(danhMuc);
            _context.SaveChanges();

            return PartialView("_DanhMucTable", _context.DanhMucs.ToList());
        }
    }
}
