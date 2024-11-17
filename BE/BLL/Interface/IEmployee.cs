using DTO.Responses;

namespace BLL.Interface
{
    public interface IEmployee
    {
        public BaseResponseModel Get(int? departmentID = null, int? employeeTypeID = null);
    }
}
