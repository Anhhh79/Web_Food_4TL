using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class GioHang
    {
        [Key]
        public int Id { get; set; }

        public decimal Gia { get; set; }
        public int SoLuong { get; set; }

        public int NguoiDungId { get; set; }
        [ForeignKey("NguoiDungId")]
        public NguoiDung NguoiDung { get; set; }

        public int MonAnId { get; set; }
        [ForeignKey("MonAnId")]
        public MonAn MonAn { get; set; } 

    }
}
