using Microsoft.EntityFrameworkCore;
using Web_Food_4TL.Areas.Customer.Controllers;
using Web_Food_4TL.Models;

namespace Web_Food_4TL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<MonAn> MonAns { get; set; }
        public DbSet<AnhMonAn> AnhMonAns { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<VaiTroNguoiDung> VaiTroNguoiDungs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DanhMuc>().HasData(
                new DanhMuc { Id = 1, TenDanhMuc = "Breakfast" });
            modelBuilder.Entity<MonAn>().HasData(
                new MonAn { Id = 1, TenMonAn = "Banh Mi", Gia = 15000, MoTa = "Ngon", DanhMucId = 1});

            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung { Id = 1, TenNguoiDung = "H", Email = "@123", MatKhau = "012", SoDienThoai= "123" });

            modelBuilder.Entity<VaiTro>().HasData(
                   new VaiTro { Id = 1, TenVaiTro = "Khách Hàng"},
                   new VaiTro { Id = 2, TenVaiTro = "Quản lý" });
        }

    }
}

