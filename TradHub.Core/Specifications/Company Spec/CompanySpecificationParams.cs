using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Specifications.Company_Spec
{
    public class CompanySpecificationParams
    {
        private const int maxPageSize = 10;
        public string? sort { get; set; }
        public int? BusinessTypeId { get; set; }
        public int? LocationId { get; set; }

        private int pageSize = 5;
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > maxPageSize ? maxPageSize : value; }
        }
        public int pageIndex { get; set; } = 1;
    }
}
