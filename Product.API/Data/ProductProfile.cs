using AutoMapper;
using Product.API.DTOs;
using Product.API.Models;

namespace Product.API.Data
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            //CreateMap<ProductModel, ProductDTO>().ReverseMap();// đổi chiều
            CreateMap<ProductModel, ProductDTO>();
            CreateMap<ProductDTO, ProductModel>()
               .ForMember(dest => dest.id, opt => opt.Ignore())
               .ForMember(dest => dest.created_at, opt => opt.Ignore());
        }
    }
}
