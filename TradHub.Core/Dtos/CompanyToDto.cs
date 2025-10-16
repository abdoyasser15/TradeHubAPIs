using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CompanyToDto
    {
        public string BusinessName { get; set; } = string.Empty;
        public int BusinessTypeId { get; set; }
        public string? TaxNumber { get; set; }
        public string? LogoUrl { get; set; }
        public string CreatedById { get; set; }
        public int LocationId { get; set; }
    }
}
