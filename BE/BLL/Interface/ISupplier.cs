using DTO.Responses;
using DTO.Supplier;

namespace BLL.Interface
{
    public interface ISupplier
    {
        public BaseResponseModel GetById(int supplierID);
        public BaseResponseModel GetPagition(int pageNumber, int pageSize, string? suppliersName = null);
        public BaseResponseModel Put(SupplierResponseModule req);
        public BaseResponseModel Post(SupplierRequestModule req);
        public BaseResponseModel Delete(int id);
    }
}
