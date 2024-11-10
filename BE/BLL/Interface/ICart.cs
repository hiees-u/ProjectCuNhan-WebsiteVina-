using DTO.Cart;
using DTO.Responses;

namespace BLL.Interface
{
    public interface ICart
    {
        public BaseResponseModel Get();
        public BaseResponseModel Post(CartRequestModule resquest);
        public BaseResponseModel Delete(int productId);
        public BaseResponseModel Put(CartRequestModule resquest);
    }
}
