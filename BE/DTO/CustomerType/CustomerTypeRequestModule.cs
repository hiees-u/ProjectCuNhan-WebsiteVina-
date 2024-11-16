namespace DTO.CustomerType
{
    public class CustomerTypeRequestModule
    {
        public string TypeCustomerName { get; set; } = null!;

        public bool isValidate()
        {
            if (string.IsNullOrEmpty(TypeCustomerName) || TypeCustomerName.Length > 50)
                return false;
            return true;
        }
    }
}
