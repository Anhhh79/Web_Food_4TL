using System.ComponentModel.DataAnnotations;

namespace Web_Food_4TL.Models
{
    public class VaiTro
    {
        [Key]
       public int Id { get; set; }
       public string TenVaiTro { get; set; }
    }
}
