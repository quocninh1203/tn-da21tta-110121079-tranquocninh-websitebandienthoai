namespace Shop.Application.DTOs.Reviews
{
    public class ReviewDTO
    {
        public string FullName { get; set; }
        public string UserImageUrl { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public string ReviewImageUrl { get; set; }
        public string Color { get; set; }  // Thêm thông tin màu sắc
        public string Ram { get; set; }    // Thêm thông tin RAM
        public string Storage { get; set; } // Thêm thông tin dung lượng lưu trữ
        public DateTime ReviewDate { get; set; }
    }
}
