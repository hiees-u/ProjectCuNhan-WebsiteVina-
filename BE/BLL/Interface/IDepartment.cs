using DTO.Department;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IDepartment
    {
        public BaseResponseModel Get();
        public BaseResponseModel Post(string departmentName);
        public BaseResponseModel Put(DepartmentRequestModule req);
        public BaseResponseModel Delete(int? deparId);
    }
}
