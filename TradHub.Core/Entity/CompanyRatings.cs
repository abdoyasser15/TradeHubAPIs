using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity.Identity;

namespace TradHub.Core.Entity
{
    public class CompanyRatings
    {
        [Key]
        public int RaitingId { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;

        public int RatingValue { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
       
    }
}
