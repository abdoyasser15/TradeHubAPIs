using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TradeHub.DTOs;
using TradeHub.Errors;
using TradeHub.Service.Companies.Command.Create_Company;
using TradeHub.Service.Companies.Command.Delete_Company;
using TradeHub.Service.Companies.Command.Update_Company;
using TradeHub.Service.Companies.Queries;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Specifications.Company_Spec;

namespace TradeHub.Controllers
{
    public class CompanyController : BaseApiController
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Admin,CompanyOwner")]
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyToDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new ApiResponse(401, "Unauthorized"));

                if (User.IsInRole("CompanyOwner"))
                {
                    var existing = await _mediator.Send(new GetCompaniesCreatedByUserIdQuery(userId));

                    if (existing.Any())
                        return BadRequest(new ApiResponse(400, "You already have a company."));
                }
                dto.CreatedById = userId;
                var result = await _mediator.Send(new CreateCompanyCommand(dto));
                if (result == null)
                    return BadRequest(new ApiResponse(400, "Problem Creating Company"));
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
        [Authorize(Roles = "Admin,CompanyOwner")]
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<CompanyDto>> UpdateCompany(Guid id,[FromBody] CompanyToDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (User.IsInRole("CompanyOwner"))
            {
                var company = await _mediator.Send(new GetCompanyByIdQuery(id));

                if (company == null)
                    return NotFound(new ApiResponse(404, "Company Not Found"));

                if (company.CreatedById != userId)
                    return Forbid();
            }
            var result = await _mediator.Send(new UpdateCompanyCommand { Id = id, Company = dto });
            if (result == null) return NotFound(new ApiResponse(404,"Company Not Found"));
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteCompany(Guid id)
        {
            var result = await _mediator.Send(new DeleteCompanyCommand(id));
            if (!result) return NotFound(new ApiResponse(404,"Company Not Found"));
            return Ok(new ApiResponse(200,"Company Deleted Successfully"));
        }
        [Authorize(Roles = "Admin,CompanyOwner")]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyById(Guid id)
        {
            var company = await _mediator.Send(new GetCompanyByIdQuery(id));
            if (company == null) return NotFound(new ApiResponse(404,"Company Not Found"));
            return Ok(company);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CompanyDto>>> GetCompanies([FromQuery] CompanySpecificationParams specParams)
        {
            var query = new GetCompaniesQuery(specParams);
            var companies = await _mediator.Send(query);
            return Ok(companies);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{UserId}/companies")]
        public async Task<ActionResult<IReadOnlyList<CompanyDto>>> GetCompaniesByUserId(string UserId)
        {
            var companies = await _mediator.Send(new GetCompaniesCreatedByUserIdQuery(UserId));
            if(companies == null || !companies.Any()) 
                return NotFound(new ApiResponse(404,"No Companies Found for the Given User"));
            return Ok(companies);
        }
        [Authorize(Roles = "Admin,CompanyOwner")]
        [HttpPut("{companyId:Guid}/logo")]
        public async Task<ActionResult> UpdateCompanyLogo(Guid companyId, [FromBody] LogoDto logoUrl)
        {
            var result = await _mediator.Send(new UpdateCompanyLogoCommand(companyId, logoUrl.LogoUrl));
            if (!result) return NotFound(new ApiResponse(404,"Company Not Found"));
            return Ok(new ApiResponse(200,"Company Logo Updated Successfully"));
        }
    }
}
