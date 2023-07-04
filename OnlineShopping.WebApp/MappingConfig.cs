
using AutoMapper;
using OnlineShopping.WebApp.Models;

namespace OnlineShopping.WebApp
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductDto, UpdateProductDto>().ReverseMap();
            CreateMap<ProductDto, CreateProductDto>().ReverseMap();
        }
    }
}