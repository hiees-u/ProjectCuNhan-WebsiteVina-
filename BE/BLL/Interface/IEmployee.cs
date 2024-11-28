using DTO.Employee;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IEmployee
    {
        public BaseResponseModel Get(int? departmentID = null, int? employeeTypeID = null, int pageNumber = 1,
            int pageSize = 8);
        public BaseResponseModel Post(EmployeeRequestPostModule req);
        public BaseResponseModel Delete(string accountName);
        public BaseResponseModel Put(EmployeeRequestPutModule req);
    }
}
