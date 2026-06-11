using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class UpdateCompanyDto
    {
        public string BusinessName { get; set; } = default!;
        public string? TaxNumber { get; set; }

        public int BusinessTypeId { get; set; }
        public int LocationId { get; set; }

        public IFormFile? LogoUrl { get; set; }
    }
}
