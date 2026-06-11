using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.Product_Spec
{
    public class ProductsByCompanyWithSubCategorySpecification : BaseSpecification<Product>
    {
        public ProductsByCompanyWithSubCategorySpecification(string companyId)
            : base(p => p.CompanyId.ToString() == companyId)
        {
            AddIncludes();
            ApplyNoTracking();
        }
        private void AddIncludes()
        {
            Include.Add(p => p.SubCategory);
        }
    }
}
