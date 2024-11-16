using DTO.Employee;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IEmployee
    {
        public BaseResponseModel Get(int? EmployeeTypeID, int? DepartmentID, int pageNumber = 1, int pageSize = 8);
        public BaseResponseModel Post(EmployeeRequestModule req);
        public BaseResponseModel Put(EmployeeRequestPutModule req);
        public BaseResponseModel Delete(string accountName);
    }
}
