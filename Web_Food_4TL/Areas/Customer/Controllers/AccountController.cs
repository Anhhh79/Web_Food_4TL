﻿using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using Web_Food_4TL.Data;
using Web_Food_4TL.Models;
using BCrypt.Net;
using Web_Food_4TL.Models.ViewModels;
namespace Web_Food_4TL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Đăng ký 
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpVM model)
        {
            try
            {
                if (model == null)
                    return Json( new { success = false, message = "Dữ liệu không hợp lệ!" });

                if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Email) ||
                    string.IsNullOrEmpty(model.PassWord) || string.IsNullOrEmpty(model.ConfirmPassWord))
                    return Json(new {success = false, message = "Vui lòng điền đầy đủ thông tin!" });

                if (model.PassWord != model.ConfirmPassWord)
                    return Json(new { success = false, message = "Mật khẩu xác nhận không khớp!" });

                // Kiểm tra email hoặc số điện thoại đã tồn tại chưa
                if (_context.NguoiDungs.Any(u => u.Email == model.Email || u.SoDienThoai == model.Phone))
                    return Json(new { success = false, message = "Email hoặc số điện thoại đã tồn tại!" });

                // Mã hóa mật khẩu trước khi lưu vào database
                string passwordHash = HashPassword(model.PassWord);

                var newUser = new NguoiDung
                {
                    TenNguoiDung = model.FullName,
                    Email = model.Email,
                    MatKhau = passwordHash,
                    SoDienThoai = model.Phone

                };

                await _context.NguoiDungs.AddAsync(newUser);
                await _context.SaveChangesAsync(); // Lưu vào database

                // Lưu vai trò người dùng (IDVaiTro = 1)
                var userRole = new VaiTroNguoiDung
                {
                    NguoiDungId = newUser.Id, // ID của người dùng vừa tạo
                    VaiTroId = 1 // 1 là ID vai trò mặc định (khách hàng)
                };

                await _context.VaiTroNguoiDungs.AddAsync(userRole);
                await _context.SaveChangesAsync(); // Lưu vai trò vào database

                _logger.LogInformation($"User {model.Email} đăng ký thành công với vai trò là khách hàng!");

                return Json(new { success = true, message = "Đăng ký thành công!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi đăng ký: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi server! Vui lòng thử lại sau." });
            }   
        }

        //Mã hóa mật khẩu 
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        //đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            try
            {
                // Validate input
                if (model == null || string.IsNullOrWhiteSpace(model.UserNameOrEmail) || string.IsNullOrWhiteSpace(model.Password))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin đăng nhập" });
                }

                // Tìm người dùng
                var user = await _context.NguoiDungs
                    .AsNoTracking() // Thêm để cải thiện hiệu suất khi chỉ đọc
                    .FirstOrDefaultAsync(u =>
                        EF.Functions.Collate(u.Email, "SQL_Latin1_General_CP1_CI_AS") == model.UserNameOrEmail.ToLower() ||
                        u.SoDienThoai == model.UserNameOrEmail);

                // Kiểm tra người dùng tồn tại
                if (user == null)
                {
                    _logger.LogWarning("Đăng nhập thất bại: Không tìm thấy tài khoản với username/email: {0}", model.UserNameOrEmail);
                    return Json(new { success = false, message = $"Tài khoản chưa được đăng ký!" });
                }

                // Kiểm tra mật khẩu
                if (!VerifyPassword(model.Password, user.MatKhau))
                {
                    _logger.LogWarning("Đăng nhập thất bại: Sai mật khẩu cho user: {0}", user.Id);
                    return Json(new { success = false, message = $"Mật khẩu không đúng!" });
                }

                //lấy trạng thái tài khoản
                if (user.TrangThai == "Bị khóa"){
                    _logger.LogWarning("Đăng nhập thất bại: tài khoản {0} đã bị khóa", user.Id);
                    return Json(new { success = false, message = $"Tài khoản đã bị khóa!" });
                }

                // Lưu thông tin vào session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.TenNguoiDung);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserPhone", user.SoDienThoai);

                HttpContext.Session.CommitAsync().Wait(); // Đảm bảo session được lưu

                // Log thành công
                _logger.LogInformation("Đăng nhập thành công cho người dùng: {UserId}", user.Id);

                // Lấy danh sách vai trò của user
                var roles = await _context.VaiTroNguoiDungs
                    .Where(vtnd => vtnd.NguoiDungId == user.Id)
                    .Select(vtnd => vtnd.VaiTro.TenVaiTro)
                    .ToListAsync();
                
                if (roles.Contains("Quản Lý"))
                {
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home", new { area = "Admin" }) });

                }
                else if(roles.Contains("Khách Hàng"))
                {
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home", new { area = "Customer" }) });

                }
                else
                {
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home", new { area = "Customer" }) });

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi trong quá trình đăng nhập cho username/email: {0}", model.UserNameOrEmail);
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi server. Vui lòng thử lại sau" });
            }
        }

        //Giải mã mật khẩu
        private bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        //Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session của người dùng
            return RedirectToAction("Index", "Home", new { area = "Customer" }); // Chuyển hướng về Customer/Home/Index
        }



    }
}

