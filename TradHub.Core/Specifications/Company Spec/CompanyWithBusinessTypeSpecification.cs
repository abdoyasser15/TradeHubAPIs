using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.Company_Spec
{
    public class CompanyWithBusinessTypeSpecification : BaseSpecification<Company>
    {
        public CompanyWithBusinessTypeSpecification(CompanySpecificationParams specParam)
            : base (C=>
            (string.IsNullOrEmpty(specParam.Search)||(C.BusinessName.ToLower().Contains(specParam.Search)))
            &&
            (!specParam.BusinessTypeId.HasValue || C.BusinessTypeId == specParam.BusinessTypeId)
            && (!specParam.LocationId.HasValue || C.LocationId == specParam.LocationId)
            )
        {
            AddIncludes();
            if (!string.IsNullOrEmpty(specParam.sort))
            {
                switch (specParam.sort)
                {
                    case "nameAsc":
                        AddOrderBy(c => c.BusinessName);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(c => c.BusinessName);
                        break;
                    default:
                        AddOrderBy(c => c.BusinessName);
                        break;
                }
            }
            ApplyPaging(specParam.PageSize * (specParam.pageIndex - 1), specParam.PageSize);
            ApplyNoTracking();
        }
        public CompanyWithBusinessTypeSpecification(Guid id)
            :base(C=>C.CompanyId==id)
        {
            AddIncludes();
            ApplyNoTracking();
        }
        private void AddIncludes()
        {
            Include.Add(c => c.BusinessType);
            Include.Add(c => c.Location);
        }
    }
}
