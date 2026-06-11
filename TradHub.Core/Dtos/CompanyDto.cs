using System;
using System.Collections.Generic;

namespace TradHub.Core.Dtos
{
    public class CompanyDto
    {
        public string ID { get; set; } = string.Empty;

        public string BusinessName { get; set; } = string.Empty;

        public int BusinessTypeId { get; set; }

        public string? TaxNumber { get; set; }

        public string? LogoUrl { get; set; }

        public string? CreatedById { get; set; } = string.Empty;

        public int LocationId { get; set; }

        public string BusinessTypeName { get; set; } = string.Empty;

        public string LocationName { get; set; } = string.Empty;

        public double AverageRating { get; set; }

        public int RatingCount { get; set; }

        public List<CategoryDto> Categories { get; set; } = new();
    }
}