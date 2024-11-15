﻿using DTO.Responses;

namespace BLL.Interface
{
    public interface ICategory
    {
        public BaseResponseModel GetCateNameByProductID(int productID);
        public BaseResponseModel GetTop10();

        public BaseResponseModel GetPagition(int? cateId = null, string? cateName = null, int pageNumber = 1, int pageSize = 8);
    }
}
