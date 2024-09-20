using AutoMapper;
using christenProject.Models.DTOs;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace christenProject.Models.DTPsMapper
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductModel, ProductDTO>()
                     .ForMember(p => p.ProductName, p => p.MapFrom(src => src.Name))
                     .ForMember(p => p.ProductDescription, p => p.MapFrom(src => src.Description))
                     .ForMember(p => p.ProductNetPrice, p => p.MapFrom(src => src.Price));
                     
        }
    }
}
