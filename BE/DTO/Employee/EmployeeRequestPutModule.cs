namespace DTO.Employee
{
    public class EmployeeRequestPutModule
    {
        public string? accountName { get; set; }
        public int? employeeTypeId { get; set; }
        public int? departmentId { get; set; }
        public string? fullName { get; set; }
        public string? email { get; set; }
        public int? addressId { get; set; }
        public string? phone {  get; set; }
        public int? gender { get; set; }

        public bool isValid()
        {
            if (!string.IsNullOrEmpty(accountName) &&
                employeeTypeId.HasValue &&
                departmentId.HasValue &&
                !string.IsNullOrEmpty(fullName) &&
                !string.IsNullOrEmpty(email) &&
                addressId.HasValue &&
                !string.IsNullOrEmpty(phone) &&
                gender.HasValue &&
                gender >= 0 && gender <= 1
            )
                return true;
            return false;
        }
    }
}
