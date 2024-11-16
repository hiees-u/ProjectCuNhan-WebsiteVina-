namespace DTO.Department
{
    public class DepartmentRequestModule
    {
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;

        public bool isValidate()
        {
            if(!DepartmentId.HasValue || string.IsNullOrWhiteSpace(DepartmentName) || DepartmentName.Length > 30)
                return false;
            return true;
        }
    }
}
