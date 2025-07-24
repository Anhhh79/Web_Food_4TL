// /Services/OrderService.cs
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Web_Food_4TL.Data;        // namespace của YourDbContext
using Web_Food_4TL.Models; // namespace của entity HoaDon

namespace Web_Food_4TL.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _db;
        public OrderService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Phương thức Hangfire sẽ gọi
        public async Task AutoCompleteIfNoReturn(int orderId)
        {
            var dh = await _db.HoaDons.FindAsync(orderId);
            if (dh == null) return;

            // Nếu đơn vẫn ở trạng thái Đang giao
            if (dh.TrangThaiDonHang == "Đã duyệt")
            {
                dh.TrangThaiDonHang = "Hoàn thành";
                await _db.SaveChangesAsync();
            }
        }
    }
}
