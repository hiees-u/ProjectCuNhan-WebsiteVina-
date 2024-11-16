namespace DTO.Employee
{
    public class EmployeeRequestModule
    {
        public string AccountName { get; set; } = null!;
        public int? EmployeeTypeId { get; set; }
        public int? DepartmentId { get; set; }

        public bool isValidate()
        {
            if(string.IsNullOrWhiteSpace(AccountName) || !EmployeeTypeId.HasValue || !DepartmentId.HasValue)
                return false;
            return true;
        }
    }
}
