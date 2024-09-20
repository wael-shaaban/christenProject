using christenProject.Data.Repo;
using christenProject.Data.Services;
using christenProject.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace christenProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public ActionResult<ApiResponse<CategoryReponseDTO>> GetCategoryByID(int id)
        {
            if (ModelState.IsValid)
            {
                var data = categoryService.GetById(id);
                if (data == null)
                    return NotFound(new ApiBasicResponse<CategoryReponseDTO>("Item Not Found"));
                return Ok(new ApiBasicResponse<CategoryReponseDTO>(data));
            }
            return BadRequest(ModelState.Select(c=>c));
        }
    }
}