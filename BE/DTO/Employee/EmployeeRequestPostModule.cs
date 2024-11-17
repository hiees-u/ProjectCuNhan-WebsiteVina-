namespace DTO.Employee
{
    public class EmployeeRequestPostModule
    {
        public int? employeeTypeId { get; set; }
        public int? departmentId { get; set; }
        public string accountName { get; set; } = null!;

        public bool isValid()
        {
            if(employeeTypeId.HasValue && departmentId.HasValue && !string.IsNullOrEmpty(accountName) )
            {
                return true;
            }
            return false;
        }

    }
}
