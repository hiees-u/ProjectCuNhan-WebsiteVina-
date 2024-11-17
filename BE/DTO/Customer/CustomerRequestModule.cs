namespace DTO.Customer
{
    public class CustomerRequestModule
    {
        public string? AccountName { get; set; } = string.Empty;
        public int? TypeCustomerId { get; set; }

        public bool isValid()
        {
            if(!string.IsNullOrEmpty(AccountName) && TypeCustomerId.HasValue)
                return true;
            return false;
        }
    }
}
