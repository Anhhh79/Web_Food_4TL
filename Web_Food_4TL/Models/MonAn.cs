using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class MonAn
    {
        [Key]
        public int Id {  get; set; }
        public string TenMonAn { get; set; }
        public string MoTa {  get; set; }
        public decimal Gia { get; set; }

        public int DanhMucId { get; set; }
        [ForeignKey("DanhMucId")]
        public DanhMuc DanhMuc { get; set; }

        public List<AnhMonAn> AnhMonAnh { get; set; }

    }
}
