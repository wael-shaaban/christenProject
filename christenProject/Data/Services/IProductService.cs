using christenProject.Data.Repo;
using christenProject.Model;
using christenProject.Models;
using christenProject.Models.DTOs;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace christenProject.Data.Services
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public PaginatedResult(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
        }
    }
    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
    public interface IProductService
    {
        public IQueryable<ProductModel> GetAllProducts();
        Task<PaginatedResult<ProductDTOResponse>> GetPagedAsync(int pageNumber, int pageSize);
        public ProductModel GetProductById(int id);
        public ProductModel GetProductByName(string name);
        public ProductModel GetProductByCategory(string category);
        public ProductModel AddProduct(ProductModel product);
        public ProductModel UpdateProduct(int productId, ProductModel product);
        public ProductModel DeleteProduct(int id);
    }
    public class ProductService(IRepository<ProductModel> prodDBContext) : IProductService
    {
        //TODO: Write here custom methods that are required for specific requirements.
        public ProductModel AddProduct(ProductModel product)
        {
            var myproduct = prodDBContext.Add(product);
            prodDBContext.Commit();
            return product;
        }

        public ProductModel DeleteProduct(int id)
        {
            var productModel = GetProductById(id);
            productModel = prodDBContext.Delete(productModel);
            prodDBContext.Commit();
            return productModel;
        }

        public IQueryable<ProductModel> GetAllProducts() =>
             prodDBContext.GetAll();

        public ProductModel GetProductById(int id) =>
             prodDBContext.GetById(id);

        public ProductModel GetProductByName(string name)
        {
            return GetAllProducts().Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public ProductModel UpdateProduct(int productId, ProductModel product)
        {
            var myproduct = GetProductById(productId);
            if (myproduct is not null)
            {
                myproduct = prodDBContext.AttachIfNot(myproduct);
                myproduct = prodDBContext.Update(myproduct);
                prodDBContext.Commit();
                return myproduct;
            }
            return null;
        }
        public ProductModel GetProductByCategory(string category) => 
            GetAllProducts().Include(x=>x.Category).Where(x => x.Category.Name.ToLower() == category.ToLower()).FirstOrDefault();

        public async Task<PaginatedResult<ProductDTOResponse>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var Customdata = GetAllProducts().Select(x => new ProductDTOResponse
            {
                ID = x.Id,
                ProductDescription = x.Description,
                ProductName = x.Name,
                ProductNetPrice = x.Price
            }).AsQueryable();
            int total = Customdata.Count() - ((pageNumber - 1) * pageNumber);
            if (total > 0)
            {
                int totalRecords = Customdata.Count();
                var data = await Customdata.Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

                return new PaginatedResult<ProductDTOResponse>(data, pageNumber, pageSize, totalRecords);
            }
            return null;
        }
    }
}