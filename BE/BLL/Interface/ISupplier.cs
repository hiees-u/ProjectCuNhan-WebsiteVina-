using DTO.Responses;

namespace BLL.Interface
{
    public interface ISupplier
    {
        public BaseResponseModel GetById(int supplierID);
    }
}
