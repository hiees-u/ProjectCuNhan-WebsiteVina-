using DTO.Product;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IProduct
    {
        public BaseResponseModel GetProducts(int? productId, int? cateId, int? subCateId, int? supplierId, string? productName, int pageNumber = 1, int pageSize = 10, int sortByName = 0, int sortByPrice = 0);
        public BaseResponseModel Put(ProductRequesModule req);
        public BaseResponseModel Post(ProductRequestPostModule req);
        public BaseResponseModel GetProductsModerator(
            int? productId,
            int? cateId,
            int? subCateId,
            int? supplierId,
            string? productName,
            int pageNumber = 1,
            int pageSize = 8,
            int sortByName = 0 /*1: tăng, -1: giảm, 0:*/,
            int sortByPrice = 0 /*1: tăng, -1: giảm, 0:*/
        );
        public BaseResponseModel Delete(int? productId);
    }
}
