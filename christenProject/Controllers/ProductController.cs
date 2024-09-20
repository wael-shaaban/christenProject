using AutoMapper;
using christenProject.Data.Services;
using christenProject.Models;
using christenProject.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace christenProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService,IMapper mapper) : ControllerBase
    {
        [HttpGet("page")]
        public async Task<IActionResult> GetDepartments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await productService.GetPagedAsync(pageNumber, pageSize);
            if(result is null)
                return NotFound(new ApiResponse<PaginatedResult<ProductDTOResponse>>("Error in Getting Page of Data"));
            //var paginationMetadata = new PaginationMetadata
            //{
            //    CurrentPage = result.PageNumber,
            //    PageSize = result.PageSize,
            //    TotalPages = result.TotalPages,
            //    TotalRecords = result.TotalRecords
            //};
            var response = new ApiResponse<PaginatedResult<ProductDTOResponse>>(result);
            return Ok(response);
        }
        //all curd operation for this product
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = productService.GetAllProducts();
            if (products is null)
                return NotFound("No any Product fount!");
            return Ok(products);    
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var product = productService.GetProductById(id);
            if (product is null)
                return NotFound("Product not fount");
            return Ok(product);
        }
        [HttpPost]
        public IActionResult AddProduct(ProductModel product)
        {
            if (product is null)
                return BadRequest("No Product data sent");
            productService.AddProduct(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);   
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateProduct(int id,ProductModel product)
        {
            if (product is null)
                return BadRequest("Bad Request");
            var result = productService.UpdateProduct(id, product);
            if (result is null)
                return NotFound("product not found!");
            return NoContent(); 
        }
        [HttpDelete]
        public IActionResult DeleteById(int id)
        {
            var product = productService.GetProductById(id);
            if (product is null)
                return NotFound("Product not fount");
            return NoContent(); 
        }
    }
}
