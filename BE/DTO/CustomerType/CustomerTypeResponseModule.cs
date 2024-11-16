namespace DTO.CustomerType
{
    public class CustomerTypeResponseModule
    {
        public int? TypeCustomerId { get; set; }
        public string TypeCustomerName { get; set; } = null!;

        public bool isValidate()
        {
            if (string.IsNullOrEmpty(TypeCustomerName) || TypeCustomerName.Length > 50 || !TypeCustomerId.HasValue)
                return false;
            return true;
        }
    }
}
