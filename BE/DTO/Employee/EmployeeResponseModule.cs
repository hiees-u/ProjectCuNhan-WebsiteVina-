namespace DTO.Employee
{
    public class EmployeeResponseModule
    {
        public int EmployeeId { get; set; }
        public string AccountName { get; set; } = null!;
        public string? FullName { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; } = null!;
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public int? Gender { get; set; }
        public int? AddressId { get; set; }
        public string AddressName { get; set; }
    }
}
