using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class ThongTinDatBan
    {
        [Key]
        public int Id { get; set; }

        public string TenNguoiDung {  get; set; }

        public DateTime OrderDate { get; set; }

        public double TongTien { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }

        public int NguoiDungId { get; set; }
        [ForeignKey("NguoiDungId")]
        public NguoiDung NguoiDung { get; set; }

    }
}
