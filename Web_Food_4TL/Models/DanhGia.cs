using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class DanhGia
    {
        [Key]
        public int Id { get; set; }

        // Nội dung đánh giá
        public string NoiDungDanhGia { get; set; }
        public int SoSao { get; set; }

        // Nội dung phản hồi từ chủ quán hoặc admin
        public string NoiDungPhanHoi { get; set; }


        public int MonAnId { get; set; }
        [ForeignKey("MonAnId")]
        public MonAn MonAnh { get; set; }

        public int NguoiDungId { get; set; }
        [ForeignKey("NguoiDungId")]
        public NguoiDung NguoiDung { get; set; }


    }
}
