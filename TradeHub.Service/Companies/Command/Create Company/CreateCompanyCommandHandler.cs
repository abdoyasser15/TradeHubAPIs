using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Repository_Contract;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service.Companies.Command.Create_Company
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyToDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CreateCompanyCommandHandler(IUnitOfWork unitOfWork , ILoggerManager logger , IMapper mapper,IImageService imageService) 
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<CompanyToDto?> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.Company;

                _logger.LogInfo("Attempting to create company: {@Company}", dto);

                var locationExists = await _unitOfWork.Repository<Location>()
                    .AnyAsync(l => l.Id == dto.LocationId);

                if (!locationExists)
                    throw new ArgumentException($"LocationId '{dto.LocationId}' is invalid.");

                var businessTypeExists = await _unitOfWork.Repository<BusinessType>()
                    .AnyAsync(bt => bt.BusinessTypeId == dto.BusinessTypeId);

                if (!businessTypeExists)
                    throw new ArgumentException($"BusinessTypeId '{dto.BusinessTypeId}' is invalid.");

                if (!string.IsNullOrEmpty(dto.TaxNumber))
                {
                    var duplicateTax = await _unitOfWork.Repository<Company>()
                        .AnyAsync(c => c.TaxNumber == dto.TaxNumber);

                    if (duplicateTax)
                        throw new DuplicateNameException($"TaxNumber '{dto.TaxNumber}' already exists.");
                }

                string? logoUrl = await _imageService.UploadImageAsync(
                    dto.LogoUrl!,
                    "images/companies"
                );

                var company = _mapper.Map<Company>(dto);

                company.LogoUrl = logoUrl;

                await _unitOfWork.Repository<Company>().AddAsync(company);
                await _unitOfWork.CompleteAsync();

                var response = _mapper.Map<CompanyToDto>(company);

                _logger.LogInfo("Company created successfully with Id={Id}", company.CompanyId);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating company");
                throw;
            }
        }
    }
}
