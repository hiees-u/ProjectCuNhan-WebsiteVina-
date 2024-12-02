using DTO.Responses;
using DTO.Transaction;

namespace BLL.Interface;

public interface ITransactionBLL
{
    BaseResponseModel CreateTransaction(TransactionDto dto);
    BaseResponseModel UpdateTransaction(TransactionDto dto);
    BaseResponseModel GetTansactions();
    BaseResponseModel GetTansaction(int orderId);
}
