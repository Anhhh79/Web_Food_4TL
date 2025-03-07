using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class VaiTroNguoiDung
    {
        [Key]
        public int Id { get; set; }

        public int NguoiDungId { get; set; }
        [ForeignKey("NguoiDungId")]
        public NguoiDung NguoiDung { get; set; }

        public int VaiTroId { get; set; }
        [ForeignKey("VaiTroId")]
        public VaiTro VaiTro { get; set; }  
    }
}
