using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity.Identity;

namespace TradHub.Core.Entity
{
    public class ProductRaiting : BaseEntity
    {
        [Key]
        public int RaitingId { get; set; }
        public int ProductId { get; set; }
        public int RaitingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Product Product { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
