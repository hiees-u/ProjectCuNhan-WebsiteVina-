using DTO.Responses;
using DTO.Supplier;

namespace BLL.Interface
{
    public interface ISupplier
    {
        public BaseResponseModel GetById(int supplierID);
        public BaseResponseModel GetPagition(int? productID);
        public BaseResponseModel Put(SupplierResponseModule req);
        public BaseResponseModel Post(SupplierRequestModule req);
        public BaseResponseModel Delete(int id);
    }
}
