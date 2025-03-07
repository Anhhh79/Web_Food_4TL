using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class AnhMonAn
    {
        [Key]
        public int MaAnhMonAnh {  get; set; }
        public string Url {  get; set; }

        public int MaMonAnh { get; set; }
        [ForeignKey("MaMonAnh")]
        public MonAn MonAnh { get; set; }
    }
}
