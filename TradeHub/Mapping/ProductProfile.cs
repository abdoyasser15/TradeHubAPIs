using AutoMapper;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;

namespace TradeHub.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // 🟢 من CreateProductDto إلى Product
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ProductAttributes, opt => opt.MapFrom(src => src.Attributes))
                .ForMember(dest => dest.Description, opt => opt.Ignore())     // مش موجود في DTO
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())        // يضاف لاحقًا
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)) // افتراضيًا نشط
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow)); // وقت الإنشاء

            // 🟢 من CreateProductAttributeDto إلى ProductAttribute
            CreateMap<CreateProductAttributeDto, ProductAttribute>()
                .ForMember(dest => dest.Value, opt => opt.Ignore()) // DTO مش فيه Value حاليًا
                .ForMember(dest => dest.CategoryAttribute, opt => opt.Ignore()) // مهم جدًا
                .ForMember(dest => dest.Product, opt => opt.Ignore()); // لتجنب loop داخل EF

            // 🟢 من Product إلى ProductDto
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

            // 🟢 من ProductAttribute إلى productattributedto
            CreateMap<ProductAttribute, productattributedto>()
                .ForMember(dest => dest.CategoryAttributeName,
                    opt => opt.MapFrom(src =>
                        src.CategoryAttribute != null ? src.CategoryAttribute.Name : string.Empty));
        }
    }
}
