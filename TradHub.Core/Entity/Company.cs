using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity.Identity;

namespace TradHub.Core.Entity
{
    public class Company : BaseEntity
    {
        public Guid CompanyId { get; set; } = Guid.NewGuid();
        public string BusinessName { get; set; }
        public int BusinessTypeId { get; set; }  
        public BusinessType BusinessType { get; set; }
        public string TaxNumber { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string LogoUrl { get; set; }
        public string CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        public ICollection<AppUser> Staff { get; set; } = new List<AppUser>();
        public ICollection<CompanyCategory> CompanyCategories { get; set; } = new List<CompanyCategory>();
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
