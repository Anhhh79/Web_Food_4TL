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
        public DbSet<ThongTinDatBan> ThongTinDatBans { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<VaiTroNguoiDung> VaiTroNguoiDungs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}

