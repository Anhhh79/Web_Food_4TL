using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class HoaDon
    {
        [Key]
        public int MaHoaDon { get; set; }

        public int SoLuong { get; set; }
        public double Gia { get; set; }


        public int MaThongTinDatBan { get; set; }
        [ForeignKey("MaNguoiDung")]
        [ValidateNever]
        public ThongTinDatBan ThongTinDatBan { get; set; }


        public int MaMonAn { get; set; }
        [ForeignKey("MaMonAn")]
        public MonAn MonAn { get; set; }
    }
}
