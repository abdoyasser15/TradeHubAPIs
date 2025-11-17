using AutoMapper;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;

namespace TradeHub.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ProductAttributes, opt => opt.MapFrom(src => src.Attributes))
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())    
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)) 
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<CreateProductAttributeDto, ProductAttribute>()
                .ForMember(dest => dest.Value, opt => opt.Ignore()) 
                .ForMember(dest => dest.CategoryAttribute, opt => opt.Ignore()) 
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src =>
                        src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.CompanyName,
                    opt => opt.MapFrom(src =>
                        src.Company != null ? src.Company.BusinessName : string.Empty))
                .ForMember(dest => dest.Attributes,
                    opt => opt.MapFrom(src => src.ProductAttributes))
                .ForMember(dest => dest.AverageRating,
                    opt => opt.MapFrom(src =>
                        src.ProductRaitings != null && src.ProductRaitings.Any()
                            ? src.ProductRaitings.Average(r => r.RaitingValue)
                            : 0))
                .ForMember(dest => dest.RatingCount,
                    opt => opt.MapFrom(src =>
                        src.ProductRaitings != null ? src.ProductRaitings.Count : 0));

            CreateMap<ProductAttribute, productattributedto>()
                .ForMember(dest => dest.CategoryAttributeName,
                    opt => opt.MapFrom(src =>
                        src.CategoryAttribute != null ? src.CategoryAttribute.Name : string.Empty));
        }
    }
}
