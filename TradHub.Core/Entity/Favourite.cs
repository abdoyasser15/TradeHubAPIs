using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity.Identity;

namespace TradHub.Core.Entity
{
    public class Favourite : BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public DateTime CreatedAt { get; set;}
    }
}
