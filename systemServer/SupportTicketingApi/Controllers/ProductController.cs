using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportTicketingBusiness.Interface;

namespace SupportTicketingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
       

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("product-list")]
        [Authorize(Roles = "Client , Manager,Support")]
        public async Task<IActionResult> GetPaged()
        {
            var response = await _productService.GetAllAsync();

            if (response.Success)
            {
                return Ok(new { data = response.Data, message = response.Message });
            }

            return NotFound(new { message = response.Message });
        }
    }
}
