using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.Company_Spec
{
    public class CompanyWithCountSpecification : BaseSpecification<Company>
    {
        public CompanyWithCountSpecification(CompanySpecificationParams specParam):
            base (C=>
            (string.IsNullOrEmpty(specParam.Search)||(C.BusinessName.ToLower().Contains(specParam.Search)))
            &&
            (!specParam.BusinessTypeId.HasValue || C.BusinessTypeId == specParam.BusinessTypeId)
            && (!specParam.LocationId.HasValue || C.LocationId == specParam.LocationId)
            )
        {
            
        }
    }
}
