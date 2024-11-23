using DTO.Responses;
using DTO.Supplier;

namespace BLL.Interface
{
    public interface ISupplier
    {
        public BaseResponseModel GetById(int supplierID);
        public BaseResponseModel GetPagition(string? suppliersName = null, int pageNumber = 1, int pageSize = 8);
        public BaseResponseModel Put(SupplierResponseModule req);
        public BaseResponseModel Post(SupplierRequestModule req);
        public BaseResponseModel Delete(int id);
    }
}
