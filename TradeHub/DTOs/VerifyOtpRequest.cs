namespace TradeHub.DTOs
{
    public class VerifyOtpRequest
    {
        public string PhoneOrEmail { get; set; }
        public string Code { get; set; }
    }
}
