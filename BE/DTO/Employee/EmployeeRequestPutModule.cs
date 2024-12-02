using System.Reflection;

namespace DTO.Employee
{
    public class EmployeeRequestPutModule
    {
        public string? accountName { get; set; }
        public int? employeeTypeId { get; set; }
        public int? departmentId { get; set; }
        public string? fullName { get; set; }
        public int? gender { get; set; }

        public bool isValid()
        {
            if (!string.IsNullOrEmpty(accountName) &&
                employeeTypeId.HasValue &&
                departmentId.HasValue &&
                (gender == null || (gender >= 0 && gender <= 1))
            )
                return true;
            return false;
        }
    }
}
