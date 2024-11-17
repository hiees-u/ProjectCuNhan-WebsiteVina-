using DTO.Employee;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IEmployee
    {
        public BaseResponseModel Get(int? departmentID = null, int? employeeTypeID = null);
        public BaseResponseModel Post(EmployeeRequestPostModule req);
        public BaseResponseModel Delete(string accountName);
        public BaseResponseModel Put(EmployeeRequestPutModule req);
    }
}
