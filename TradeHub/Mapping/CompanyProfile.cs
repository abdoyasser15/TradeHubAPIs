using AutoMapper;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;

namespace TradeHub.Mapping
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<CompanyToDto, Company>()
             .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
             .ForMember(dest => dest.BusinessType, opt => opt.Ignore())
             .ForMember(dest => dest.Location, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
             .ForMember(dest => dest.Staff, opt => opt.Ignore()) 
             .ForMember(dest => dest.Products, opt => opt.Ignore())
             .ForMember(dest => dest.CompanyCategories, opt => opt.Ignore())
             .ForMember(dest => dest.CompanyId, opt => opt.Ignore());

            CreateMap<Company, CompanyToDto>();
        }
    }
}
