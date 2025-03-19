namespace Web_Food_4TL.Models.ViewModels
{
    public class MonAnVM
    {
        public int Id { get; set; }
        public string TenMonAn { get; set; }
        public string MoTa { get; set; }
        public decimal Gia { get; set; }
        public int DanhMucId { get; set; } // Liên kết với danh mục
        public string TenDanhMuc { get; set; } // Tên danh mục
        public List<string> AnhMonAn { get; set; } // Danh sách đường dẫn ảnh
    }
}
