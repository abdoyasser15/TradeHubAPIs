using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.CompanyCategorySepc
{
    public class CompanyCategoryByCompanyIdSpecification : BaseSpecification<CompanyCategory>
    {
        public CompanyCategoryByCompanyIdSpecification(Guid companyId)
            :base(cc => cc.CompanyId == companyId)
        {
            AddIncludes();
        }
        private void AddIncludes() 
        {
            Include.Add(cc => cc.Category);
            Include.Add(cc => cc.Company);
        }
    }
}
