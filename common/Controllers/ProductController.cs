using common.Entities;
using common.Models.Base;
using common.Models.Products;
using common.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace common.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(new ResponseBase<List<ProductDTO>>(products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(new ResponseBase<ProductDTO>(product));
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestBase<ProductReq> productRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _productService.CreateProduct(productRequest.body);

            return CreatedAtAction(nameof(GetById), new ResponseBase<object>(new { id = productId }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, RequestBase<ProductReq> productRequest)
        {
            var updated = await _productService.UpdateProduct(id, productRequest.body);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _productService.DeleteProduct(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
