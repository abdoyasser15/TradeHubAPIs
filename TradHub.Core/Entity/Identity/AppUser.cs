using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Enums;

namespace TradHub.Core.Entity.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public UserRole Role { get; set; }
        public AccountType? AccountType { get; set; }
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string LoginProvider { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<ProductRaiting> ProductRatings { get; set; }
    }
}
