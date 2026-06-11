using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CompanyCategoryDto
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CategoryName { get; set; }
        public string LocationName { get; set; }
        public string BusinessTypeName { get; set; }
        public string? LogoUrl { get; set; }
        public string? TaxNumber { get; set; }
    }
}
