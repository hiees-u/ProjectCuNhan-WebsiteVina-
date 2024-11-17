namespace DTO.Customer
{
    public class CustomerResponseModule
    {
        public string AccountName { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? Gender { get; set; }
        public int? TypeCustomerId { get; set; }
        public string TypeCustomerName { get; set; } = null!;
        public int? AddressId { get; set; }
        public string? AddressString { get; set; }

    }
}
