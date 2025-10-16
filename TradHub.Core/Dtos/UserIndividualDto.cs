using System.Text.Json.Serialization;

namespace TradeHub.DTOs
{
    public class UserIndividualDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string>? Roles { get; set; }
        public string LoginProvider { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
