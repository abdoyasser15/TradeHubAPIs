using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class BusinessType : BaseEntity
    {
        public int BusinessTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}
