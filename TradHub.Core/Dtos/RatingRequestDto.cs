using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class RatingRequestDto
    {
        public int RatingValue { get; set; }
        public string? Comment { get; set; }
    }
}
