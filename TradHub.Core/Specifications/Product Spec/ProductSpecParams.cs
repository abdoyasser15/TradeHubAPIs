using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Specifications.Product_Spec
{
    public class ProductSpecParams
    {
        private const int maxSize = 10;
        private int pageSize = 5;
        private string? search;


        public int? categoryId { get; set; }
        public Guid? CompanyId { get; set; }
        public string? Sort { get; set; }

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > maxSize ? maxSize : value; }
        }
        public int pageIndex { get; set; } = 1;
    }
}
