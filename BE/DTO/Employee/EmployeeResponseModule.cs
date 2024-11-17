namespace DTO.Employee
{
    public class EmployeeResponseModule
    {
        public int? employId { get; set; }
        public string accountName { get; set; } = null!;
        public string? fullName { get; set; }
        public int? employeeTypeId { get; set; }
        public string? employeeTypeName { get; set; } = null!;
        public int? departmentId { get; set; }
        public string? departmentName { get; set; } = null!;
        public int? gender { get; set; }
        public int? addressId { get; set; }
        public string? addressName { get; set; }
    }
}
