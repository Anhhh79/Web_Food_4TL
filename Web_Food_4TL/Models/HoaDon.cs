using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Food_4TL.Models
{
    public class HoaDon
    {
        [Key]
        public int Id { get; set; } // Mã hóa đơn

        [Required]
        public decimal TongTien { get; set; } // Tổng tiền hóa đơn

        [Required]
        public string TrangThai { get; set; } // Trạng thái: Chờ xác nhận, Đã thanh toán, Đã hủy

        public DateTime NgayTao { get; set; } = DateTime.Now; // Thời gian tạo hóa đơn

        public string SoDienThoai { get; set; }

        public string DiaChiGiaoHang { get; set; }

        public DateTime? NgayNhan { get; set; }

        public string? TrangThaiDonHang { get; set; }

        public string? Lydo { get; set; }

        public string? LyDoTuChoi { get; set; }

        public string? TrangThaiGiaoHang { get; set; }

        public int NguoiDungId { get; set; }
        [ForeignKey("NguoiDungId")]
        public NguoiDung NguoiDung { get; set; }

        public ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } // Danh sách món ăn trong hóa đơn
    }
}
