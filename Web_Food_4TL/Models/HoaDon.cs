using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class HoaDon
    {
        [Key]
        public int Id { get; set; }

        public int SoLuong { get; set; }
        public double Gia { get; set; }


        public int ThongTinDatBanId { get; set; }
        [ForeignKey("NguoiDungId")]
        public ThongTinDatBan ThongTinDatBan { get; set; }


        public int MonAnId { get; set; }
        [ForeignKey("MonAnId")]
        public MonAn MonAn { get; set; }
    }
}
