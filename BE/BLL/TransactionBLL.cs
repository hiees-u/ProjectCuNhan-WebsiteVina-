using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Responses;
using DTO.Transaction;
using Mapster;

namespace BLL;

public class TransactionBLL : ITransactionBLL
{
    private readonly DbVINA _dbVINA;

    public TransactionBLL()
    {
        _dbVINA = new DbVINA(ConnectionStringHelper.Get());
    }

    public BaseResponseModel CreateTransaction(TransactionDto dto)
    {
        try
        {
            var entity = _dbVINA.Transactions.Add(dto.Adapt<Transaction>());
            _dbVINA.SaveChanges();
            return new BaseResponseModel()
            {
                IsSuccess = true,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel()
            {
                IsSuccess = false,
            };
        }
    }

    public BaseResponseModel GetTansaction(int orderId)
    {
        try
        {
            var entity = _dbVINA.Transactions.FirstOrDefault(s => s.OrderId == orderId);
            return new BaseResponseModel()
            {
                IsSuccess = true,
                Data = entity.Adapt<TransactionDto>()
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel()
            {
                IsSuccess = false,
            };
        }
    }

    public BaseResponseModel GetTansactions()
    {
        try
        {
            var entities = _dbVINA.Transactions.ToList();
            return new BaseResponseModel()
            {
                IsSuccess = true,
                Data = entities.Adapt<List<TransactionDto>>()
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel()
            {
                IsSuccess = false,
            };
        }
    }

    public BaseResponseModel UpdateTransaction(TransactionDto dto)
    {
        try
        {
            var entity = _dbVINA.Transactions.FirstOrDefault(s => s.LocalTransactionId == dto.LocalTransactionId);

            return new BaseResponseModel()
            {
                IsSuccess = true,
                Data = entity.Adapt<TransactionDto>()
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel()
            {
                IsSuccess = false,
            };
        }
    }
}
