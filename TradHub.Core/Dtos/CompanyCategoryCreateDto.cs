using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CompanyCategoryCreateDto
    {
        public Guid CompanyId { get; set; }
        public int CategoryId { get; set; }
    }
}
