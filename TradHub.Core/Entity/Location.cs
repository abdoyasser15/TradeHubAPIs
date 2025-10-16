using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class Location : BaseEntity
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public ICollection<Company> Companies { get; set; } = new List<Company>();

    }
}
