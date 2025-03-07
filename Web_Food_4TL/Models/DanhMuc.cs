using System.ComponentModel.DataAnnotations;

namespace Web_Food_4TL.Models
{
    public class DanhMuc
    {
        [Key]
        public int Id { get; set; }
        public string TenDanhMuc { get; set; }

    }
}
