namespace DTO.Supplier
{
    public class SupplierRequestModule
    {
        public string SupplierName { get; set; } = null!;
        public int? AddressId { get; set; }

        public bool isValidate()
        {
            if (!string.IsNullOrEmpty(SupplierName))
            {
                return true;
            }

            if(AddressId.HasValue)
            {
                return true;
            }
            return false;
        }
    }
}
