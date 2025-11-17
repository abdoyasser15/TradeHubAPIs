using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeHub.DTOs;
using TradeHub.Errors;
using TradHub.Core.Dtos;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BusinessTypeController : BaseApiController
    {
        private readonly IBusinessTypeService _businessType;

        public BusinessTypeController(IBusinessTypeService businessType)
        {
            _businessType = businessType;
        }
        [HttpGet]
        public async Task<ActionResult<BusinessTypeDto>> GetBusinessTypes()
        {
            var businessTypes = await _businessType.GetBusinessTypesAsync();
            if (businessTypes == null || !businessTypes.Any())
                return NotFound(new ApiResponse(404, "No Business Types Found"));
            return Ok(businessTypes);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BusinessTypeDto>> GetBusinessTypeById(int id)
        {
            var businessType = await _businessType.GetBusinessTypeByIdAsync(id);
            if (businessType == null)
                return NotFound(new ApiResponse(404, "Business Type Not Found"));
            return Ok(businessType);
        }
        [HttpPost]
        public async Task<ActionResult<BusinessTypeDto>> AddBusinessType([FromBody] BusinessTypeDto businessType)
        {
            try
            {
                var createdBusinessType = await _businessType.AddBusinessTypeAsync(businessType);
                if (createdBusinessType is null)
                    return BadRequest(new ApiResponse(400, "Problem Creating Business Type"));
                return Ok(createdBusinessType);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<BusinessTypeDto>> UpdateBusinessType(int id, [FromBody] BusinessTypeDto businessType)
        {
            var updatedBusinessType = await _businessType.UpdateBusinessTypeAsync(id, businessType);
            if (updatedBusinessType == null)
                return NotFound(new ApiResponse(404, "Business Type Not Found"));
            return Ok(updatedBusinessType);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBusinessType(int id)
        {
            try
            {
                var result = await _businessType.DeleteBusinessTypeAsync(id);
                if (!result)
                    return NotFound(new ApiResponse(404, "Business Type Not Found"));
                return Ok(new ApiResponse(200, "Business Type Deleted Successfully"));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }
    }
}
