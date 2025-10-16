using System.ComponentModel.DataAnnotations;

namespace TradeHub.DTOs
{
    public class LogoDto
    {
        [Required]
        public string LogoUrl { get; set; }
    }
}
