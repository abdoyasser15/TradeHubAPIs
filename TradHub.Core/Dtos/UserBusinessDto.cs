namespace TradeHub.DTOs
{
    public class UserBusinessDto : UserIndividualDto
    {
        public Guid? CompanyId { get; set; }
        public int? BusinessType { get; set; }
        public string BusinessName { get; set; }

    }
}
