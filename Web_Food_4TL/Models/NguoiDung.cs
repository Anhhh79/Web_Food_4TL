using System.ComponentModel.DataAnnotations;

namespace Web_Food_4TL.Models
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }
        public string TenNguoiDung { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
    }
}
