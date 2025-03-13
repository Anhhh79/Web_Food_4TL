using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web_Food_4TL.Models;

namespace Web_Food_4TL.Models
{
    public class HoaDonChiTiet
    {
        [Key]
        public int Id { get; set; } // Mã chi tiết hóa đơn

        [Required]
        public string TenMonAn { get; set; } // Tên món ăn

        [Required]
        public int SoLuong { get; set; } // Số lượng món

        [Required]
        public decimal Gia { get; set; } // Giá của từng món

        // Khóa ngoại tới bảng HoaDon
        public int HoaDonId { get; set; }
        [ForeignKey("HoaDonId")]
        public HoaDon HoaDon { get; set; }

        // Khóa ngoại tới bảng MonAn
        public int MonAnId { get; set; }
        [ForeignKey("MonAnId")]
        public MonAn MonAn { get; set; }

        // Truy xuất NguoiDungId từ bảng HoaDons
        [NotMapped] // Không lưu vào database
        public int NguoiDungId => HoaDon?.NguoiDungId ?? 0; // Lấy từ bảng HoaDons
    }
}
