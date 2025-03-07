using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class VaiTroNguoiDung
    {
        [Key]
        public int MaVaiTroNguoiDung { get; set; }

        [ForeignKey("NguoiDung")]
        public int MaNguoiDung { get; set; }
        [ValidateNever]
        public NguoiDung NguoiDung { get; set; }  

        [ForeignKey("VaiTro")]
        public int MaVaiTro { get; set; }
        [ValidateNever]
        public VaiTro VaiTro { get; set; }  
    }
}
