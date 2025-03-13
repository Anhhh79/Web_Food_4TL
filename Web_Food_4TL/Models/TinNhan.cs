using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web_Food_4TL.Models
{
    public class TinNhan
    {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "Tin nhắn không được để trống.")]
            public string MessageText { get; set; }

            public bool IsFromAI { get; set; }

            public DateTime Timestamp { get; set; } = DateTime.UtcNow;

            [Required(ErrorMessage = "SenderId là bắt buộc.")]
            public string SenderId { get; set; }

            [ForeignKey("SenderId")]
            [JsonIgnore] // Ngăn vòng lặp JSON khi lấy dữ liệu người gửi
            public virtual NguoiDung Sender { get; set; }

            public string? ReceiverId { get; set; } // Cho phép null khi chat với AI

            [ForeignKey("ReceiverId")]
            [JsonIgnore] // Ngăn vòng lặp JSON khi lấy dữ liệu người nhận
            public virtual NguoiDung? Receiver { get; set; }
    }
}
