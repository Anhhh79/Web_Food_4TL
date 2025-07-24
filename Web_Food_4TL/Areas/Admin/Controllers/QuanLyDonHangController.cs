using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Data;
using Web_Food_4TL.Services;

namespace Web_Food_4TL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyDonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyDonHangController(ApplicationDbContext context)
        {
            this._context = context;
        }


        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var name = HttpContext.Session.GetString("UserName");

            if (!id.HasValue || name != "Admin")
            {
                return RedirectToAction("Index", "Error", new { area = "Customer" });
            }

            return View();
        }


        public IActionResult ViewDonDangGiao()
        {
            return View("ViewDangGiao");
        }

        public IActionResult ViewDonYeuCauDoiTra()
        {
            return View("ViewYeuCauDoiTra");
        }

        public IActionResult ViewDonDoiTra()
        {
            return View("ViewDoiTra");
        }

        public IActionResult ViewDonHoanThanh()
        {
            return View("ViewHoanThanh");
        }

        //API hiển thị danh sách đơn hàng chờ xác nhận
        [HttpGet]
        public async Task<IActionResult> GetDonHangChoXacNhan()
        {
            try
            {
                var donHangChoXacNhan = await _context.HoaDons
               .Where(h => h.TrangThaiDonHang == "Đã đặt hàng")
               .Select(h => new
               {
                   h.Id,
                   NgayDatHang = h.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                   h.TongTien,
                   h.TrangThaiDonHang,
                   h.DiaChiGiaoHang,
                   h.SoDienThoai,
                   TenKhachHang = _context.NguoiDungs
                                       .Where(nd => nd.Id == h.NguoiDungId)
                                       .Select(nd => nd.TenNguoiDung)
                                       .FirstOrDefault()
               })
               .ToListAsync();

                if (donHangChoXacNhan == null || !donHangChoXacNhan.Any())
                {
                    return Json(new { success = false, message = "Không có đơn hàng chờ xác nhận." });
                }

                return Json(new { success = true, data = donHangChoXacNhan });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Lỗi server!" });
            }
           
        }


        //API hiển thị danh sách đơn hàng đang giao
        public async Task<IActionResult> GetDonHangDangGiao()
        {

            try
            {
                var donHangDangGiao = await _context.HoaDons
                .Where(h => h.TrangThaiGiaoHang == "Đang giao")
                .Select(h => new
                {
                    h.Id,
                    NgayDatHang = h.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                    h.TongTien,
                    h.TrangThaiDonHang,
                    h.DiaChiGiaoHang,
                    h.SoDienThoai,
                    TenKhachHang = _context.NguoiDungs
                                        .Where(nd => nd.Id == h.NguoiDungId)
                                        .Select(nd => nd.TenNguoiDung)
                                        .FirstOrDefault()
                })
                .ToListAsync();
                if (donHangDangGiao == null || !donHangDangGiao.Any())
                {
                    return Json(new { success = false, message = "Không có đơn hàng đang giao." });
                }
                return Json(new { success = true, data = donHangDangGiao });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Lỗi server!" });
            }
            
        }

        //API hiển thị danh sách đơn hàng yêu cầu đổi trả
        public async Task<IActionResult> GetDonHangYeuCauDoiTra()
        {
            try
            {
                var donHangYeuCauDoiTra = await _context.HoaDons
               .Where(h => h.TrangThaiDonHang == "Chờ đổi trả")
               .Select(h => new
               {
                   h.Id,
                   NgayDatHang = h.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                   h.TongTien,
                   h.TrangThaiDonHang,
                   h.DiaChiGiaoHang,
                   h.SoDienThoai,
                   h.Lydo,
                   TenKhachHang = _context.NguoiDungs
                                       .Where(nd => nd.Id == h.NguoiDungId)
                                       .Select(nd => nd.TenNguoiDung)
                                       .FirstOrDefault()
               })
               .ToListAsync();
                if (donHangYeuCauDoiTra == null || !donHangYeuCauDoiTra.Any())
                {
                    return Json(new { success = false, message = "Không có đơn hàng yêu cầu đổi trả." });
                }
                return Json(new { success = true, data = donHangYeuCauDoiTra });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Lỗi server!" });
            }
           
        }

        //API hiển thị danh sách đơn hàng đã đổi trả
        public async Task<IActionResult> GetDonHangDoiTra()
        {
            try
            {
                var donHangDoiTra = await _context.HoaDons
                .Where(h => h.TrangThaiDonHang == "Đơn đổi trả")
                .Select(h => new
                {
                    h.Id,
                    NgayDatHang = h.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                    h.TongTien,
                    h.TrangThaiDonHang,
                    h.DiaChiGiaoHang,
                    h.SoDienThoai,
                    TenKhachHang = _context.NguoiDungs
                                        .Where(nd => nd.Id == h.NguoiDungId)
                                        .Select(nd => nd.TenNguoiDung)
                                        .FirstOrDefault()
                })
                .ToListAsync();
                if (donHangDoiTra == null || !donHangDoiTra.Any())
                {
                    return Json(new { success = false, message = "Không có đơn hàng đã đổi trả." });
                }
                return Json(new { success = true, data = donHangDoiTra });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Lỗi server!" });
            }
            
        }

        //API hiển thị danh sách đơn hàng đã hoàn thành
        public async Task<IActionResult> GetDonHangHoanThanh()
        {
            try
            {
                var donHangHoanThanh = await _context.HoaDons
                .Where(h => h.TrangThaiDonHang == "Hoàn thành")
                .Select(h => new
                {
                    h.Id,
                    NgayDatHang = h.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                    h.TongTien,
                    h.TrangThaiDonHang,
                    h.DiaChiGiaoHang,
                    h.SoDienThoai,
                    TenKhachHang = _context.NguoiDungs
                                        .Where(nd => nd.Id == h.NguoiDungId)
                                        .Select(nd => nd.TenNguoiDung)
                                        .FirstOrDefault()
                })
                .ToListAsync();
                if (donHangHoanThanh == null || !donHangHoanThanh.Any())
                {
                    return Json(new { success = false, message = "Không có đơn hàng đã hoàn thành." });
                }
                return Json(new { success = true, data = donHangHoanThanh });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Lỗi server!" });
            }
            
        }

        //API xử lý đơn hàng chờ xác nhận
        [HttpPost]
        public async Task<IActionResult> XuLyDonHangChoXacNhan(int id)
        {
            try
            {
                var donHang = await _context.HoaDons.FindAsync(id);
                if (donHang == null)
                {
                    return Json(new { success = false, message = "Đơn hàng không tồn tại." });
                }

                donHang.TrangThaiDonHang = "Đã duyệt"; // Cập nhật trạng thái đơn hàng
                donHang.TrangThaiGiaoHang = "Đang giao";

                _context.HoaDons.Update(donHang);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cập nhật đơn hàng thành công." });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Lỗi server!" });
            }
            
        }

        //API xử lý đơn hàng đang giao
        [HttpPost]
        public async Task<IActionResult> XuLyDonHangDangGiao(int id)
        {
            try
            {
                var donHang = await _context.HoaDons.FindAsync(id);
                if (donHang == null)
                {
                    return Json(new { success = false, message = "Đơn hàng không tồn tại." });
                }
                donHang.TrangThaiGiaoHang = "Đã giao";
                donHang.NgayNhan = DateTime.Now;
                _context.HoaDons.Update(donHang);
                await _context.SaveChangesAsync();

                // Lên lịch job chạy sau 5 phút
                BackgroundJob.Schedule<OrderService>(
             svc => svc.AutoCompleteIfNoReturn(id),
             TimeSpan.FromMinutes(5));

                return Json(new { success = true, message = "Cập nhật đơn hàng thành công." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lỗi server!" });
            }

        }

        //API xử lý đơn hàng yêu cầu đổi trả
        [HttpPost]
        public async Task<IActionResult> AcceptYeuCauDoiTra(int id)
        {
            try
            {
                var donHang = await _context.HoaDons.FindAsync(id);
                if (donHang == null)
                {
                    return Json(new { success = false, message = "Đơn hàng không tồn tại." });
                }
                donHang.TrangThaiDonHang = "Đơn đổi trả"; // Cập nhật trạng thái đơn hàng
                _context.HoaDons.Update(donHang);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật đơn hàng thành công." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lỗi server!" });
            }

        }

        //API xử lý đơn hàng yêu cầu đổi trả
        [HttpPost]
        public async Task<IActionResult> RejectYeuCauDoiTra(int id, string lyDo)
        {
            try
            {
                var donHang = await _context.HoaDons.FindAsync(id);
                if (donHang == null)
                {
                    return Json(new { success = false, message = "Đơn hàng không tồn tại." });
                }
                donHang.TrangThaiDonHang = "Hoàn thành"; // Cập nhật trạng thái đơn hàng
                donHang.LyDoTuChoi = lyDo; // Lưu lý do từ chối
                _context.HoaDons.Update(donHang);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật đơn hàng thành công." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lỗi server!" });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetDanhSachChiTiet(int id)
        {
            try
            {
                var donHangs = await _context.HoaDons
                    .AsNoTracking()
                    .Where(hd => hd.Id == id)
                    .OrderByDescending(hd => hd.NgayTao)
                    .Select(hd => new
                    {
                        hd.Id,
                        hd.TongTien,
                        hd.TrangThai,
                        hd.NgayTao,
                        hd.DiaChiGiaoHang,
                        hd.SoDienThoai,
                        ChiTiets = hd.HoaDonChiTiets.Select(hct => new
                        {
                            hct.Id,
                            hct.TenMonAn,
                            hct.SoLuong,
                            hct.Gia,
                            MonAn = new
                            {
                                hct.MonAn.Id,
                                hct.MonAn.TenMonAn,
                                hct.MonAn.MoTa,
                                hct.MonAn.Gia,
                                DanhMuc = hct.MonAn.DanhMuc.TenDanhMuc,
                                AnhMonAn = hct.MonAn.AnhMonAnh
                                    .OrderBy(a => a.Id)
                                    .Select(a => a.Url)
                                    .FirstOrDefault()
                            }
                        })
                    })
                    .ToListAsync();

                return Json(new { success = true, data = donHangs });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã có lỗi xảy ra khi truy xuất dữ liệu.",
                    error = ex.Message  // hoặc không expose chi tiết exception ra client tùy policy
                });
            }
        }
    }
}