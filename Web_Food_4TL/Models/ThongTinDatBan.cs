using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class ThongTinDatBan
    {
        [Key]
        public int MaThongTinDatBan { get; set; }

        public string TenNguoiDung {  get; set; }

        public DateTime OrderDate { get; set; }

        public double TongTien { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }

        [ForeignKey("MaNguoiDung")]
        public int MaNguoiDung { get; set; }
        [ValidateNever]

        public NguoiDung NguoiDung { get; set; }

    }
}
