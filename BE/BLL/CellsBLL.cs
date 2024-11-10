using BLL.Interface;
using DTO.Cells;
using DTO.Responses;

namespace BLL
{
    public class CellsBLL : ICell
    {
        public BaseResponseModel Get()
        {
            List<CellsResponseModel> responseModels = new List<CellsResponseModel>();
            try
            {

            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex}"
                };
            }
            return new BaseResponseModel();
        }
    }
}
