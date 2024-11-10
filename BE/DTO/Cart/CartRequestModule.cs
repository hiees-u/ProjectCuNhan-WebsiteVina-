namespace DTO.Cart
{
    public class CartRequestModule
    {
        public int ProductId { get; set; }
        public int? Quantity { get; set; } = 0;

        public bool validateQuantity()
        {
            if(this.Quantity <= 0)
                return false;
            return true;
        }
    }
}
