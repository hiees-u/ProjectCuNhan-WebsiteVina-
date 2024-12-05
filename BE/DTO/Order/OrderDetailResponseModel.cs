namespace DTO.Order
{
    public class OrderDetailResponseModel
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public decimal Gia { get; set; }
        public int SoLuongTon { get; set; } = 0;
        public int SoLuongMua { get; set; }

    }
}
