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

        public CreateCompanyCommandHandler(IUnitOfWork unitOfWork , ILoggerManager logger , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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
                {
                    _logger.LogWarn("Invalid LocationId: {Id}", dto.LocationId);
                    throw new ArgumentException($"LocationId '{dto.LocationId}' is invalid.");
                }

                var businessTypeExists = await _unitOfWork.Repository<BusinessType>()
                    .AnyAsync(bt => bt.BusinessTypeId == dto.BusinessTypeId);

                if (!businessTypeExists)
                {
                    _logger.LogWarn("Invalid BusinessTypeId: {Id}", dto.BusinessTypeId);
                    throw new ArgumentException($"BusinessTypeId '{dto.BusinessTypeId}' is invalid.");
                }
                if (!string.IsNullOrEmpty(dto.TaxNumber))
                {
                    var duplicateTax = await _unitOfWork.Repository<Company>()
                        .AnyAsync(c => c.TaxNumber == dto.TaxNumber);

                    if (duplicateTax)
                    {
                        _logger.LogWarn("Duplicate TaxNumber: {Tax}", dto.TaxNumber);
                        throw new DuplicateNameException($"TaxNumber '{dto.TaxNumber}' already exists.");
                    }
                }
                var company = _mapper.Map<Company>(dto);

                var createdCompany = _unitOfWork.Repository<Company>().AddAsync(company);
                await _unitOfWork.CompleteAsync();
                var response = _mapper.Map<CompanyToDto>(createdCompany);

                _logger.LogInfo("Company created successfully with Id={Id}", company.CompanyId);

                return response;
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogWarn("Duplicate update error: {Message}", ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error while Creatng Company");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating company");
                throw;
            }
        }
    }
}
