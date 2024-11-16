namespace DTO.Employee
{
    public class EmployeeRequestPutModule
    {
        public string AccountName { get; set; } = null!;
        public int? EmployeeTypeId { get; set; }
        public int? DepartmentId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int? AddressId { get; set; }
        public string? Phone { get; set; }
        public int? Gender { get; set; }

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(AccountName) &&
                EmployeeTypeId.HasValue &&
                DepartmentId.HasValue &&
                !string.IsNullOrWhiteSpace(FullName) &&
                FullName.Length <= 50 &&
                !string.IsNullOrWhiteSpace(Email) &&
                Email.Length <= 50 &&
                AddressId > 0 &&
                !string.IsNullOrWhiteSpace(Phone) && 
                Phone.Length <= 11 &&
                Gender >= 0 && Gender <= 1;
        }
    }
}
