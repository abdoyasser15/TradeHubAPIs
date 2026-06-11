using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CreateCompanyDto
    {
        public string BusinessName { get; set; } = string.Empty;
        public int BusinessTypeId { get; set; }
        public string? TaxNumber { get; set; }
        public IFormFile? LogoUrl { get; set; }
        public string CreatedById { get; set; }
        public int LocationId { get; set; }
    }
}
