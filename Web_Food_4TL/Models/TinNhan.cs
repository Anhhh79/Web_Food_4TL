using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web_Food_4TL.Models
{
    public class TinNhan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NoiDung { get; set; } // Nội dung tin nhắn

        public DateTime ThoiGianGui { get; set; } = DateTime.Now; // Thời gian gửi

        public bool LaTinNhanTuKhach { get; set; } // True nếu là khách hàng gửi, False nếu là admin gửi

        public bool DaDoc { get; set; } = false;

        public int? NguoiDungId { get; set; } // ID người gửi (nếu có)

        [ForeignKey("NguoiDungId")]
        [JsonIgnore]
        public NguoiDung NguoiDung { get; set; } // Navigation property tới NguoiDung

        public int? NguoiGuiId { get; set; } // Người gửi là ai
        [ForeignKey("NguoiGuiId")]
        public NguoiDung NguoiGui { get; set; }

        public int? NguoiNhanId { get; set; } // Người nhận là ai
        [ForeignKey("NguoiNhanId")]
        public NguoiDung NguoiNhan { get; set; }

    }
}
