using christenProject.Data.Repo;
using christenProject.Models;
using christenProject.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace christenProject.Data.Services
{
    public interface ICategoryService
    {
        public CategoryReponseDTO GetById(int id);
    }
    public class CateogryService(IRepository<CategoryModel> repository) : ICategoryService
    {
        public CategoryReponseDTO GetById(int id)
        {
            var data = repository.GetAll().Include(c => c.Products).Select(c => new CategoryReponseDTO
            {
                CId = id,
                CName = c.Name,
                CProductsNames = c.Products.Select(c => c.Name).ToList()
            }).FirstOrDefault();
            return data;
        }
    }
}