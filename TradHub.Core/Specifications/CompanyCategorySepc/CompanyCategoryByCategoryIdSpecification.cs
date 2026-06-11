using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.CompanyCategorySepc
{
    public class CompanyCategoryByCategoryIdSpecification : BaseSpecification<CompanyCategory>
    {
        public CompanyCategoryByCategoryIdSpecification(int categoryId)
            :base(cc => cc.CategoryId == categoryId)
        {
            AddIncludes();
        }
        private void AddIncludes() 
        {
            Include.Add(cc => cc.Category);
            Include.Add(cc => cc.Company);
            Include.Add(cc => cc.Company.BusinessType);
            Include.Add(cc => cc.Company.Location);
        }
    }
}
