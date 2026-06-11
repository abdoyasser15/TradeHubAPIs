using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class RatingResponseDto
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public string? Comment { get; set; }
        public string UserId { get; set; } = null!;
        public string UserFullname { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
