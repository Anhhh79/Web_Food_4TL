using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class GioHang
    {
        [Key]
        public int MaGioHang { get; set; }

        [NotMapped]
        public double Gia { get; set; }
        public int SoLuong { get; set; }

        [ForeignKey("MaNguoiDung")]
        public int MaNguoiDung { get; set; }
        [ValidateNever]

        public NguoiDung NguoiDung { get; set; }

        [ForeignKey("MaMonAn")]
        public int MaMonAn { get; set; }
        [ValidateNever]

        public MonAn MonAn { get; set; } 

    }
}
