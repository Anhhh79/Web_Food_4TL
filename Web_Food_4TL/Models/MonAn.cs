using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class MonAn
    {
        [Key]
        public int MaMonAn {  get; set; }
        public string TenMonAn { get; set; }
        public string MoTa {  get; set; }
        public double Gia { get; set; }

        [ForeignKey("MaDanhMuc")]
        public int MaDanhMuc { get; set; }

        [ValidateNever]

        public DanhMuc DanhMuc { get; set; }

        public List<AnhMonAn> AnhMonAnh { get; set; }

    }
}
