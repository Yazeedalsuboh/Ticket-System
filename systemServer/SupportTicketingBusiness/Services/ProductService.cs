using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Interface;
using SupportTicketingData.Entities;
using SupportTicketingData.Interface;
using SupportTicketingData.Repositories;
using Mapster;

namespace SupportTicketingBusiness.Services
{
    public class ProductService :IProductService
    {
        private IGenericRepo<Product> _productRepo;

        public ProductService(IGenericRepo<Product> productRepo)
        {
            _productRepo = productRepo;
        }
        public async Task<ServiceResponse<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productRepo.GetAllAsync();
            if (products == null || !products.Any())
            {
                return ServiceResponse<List<ProductDto>>.FailureResponse("No products found.");
            }

            var productDtos = products.Adapt<List<ProductDto>>();
            return ServiceResponse<List<ProductDto>>.SuccessResponse(productDtos, "Products retrieved successfully.");
        }
    }
}
