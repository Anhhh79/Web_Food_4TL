using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class AnhMonAn
    {
        [Key]
        public int Id {  get; set; }
        public string Url {  get; set; }

        public int MonAnId { get; set; }
        [ForeignKey("MonAnId")]
        public MonAn MonAnh { get; set; }
    }
}
