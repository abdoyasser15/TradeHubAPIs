using System.ComponentModel.DataAnnotations;
using TradHub.Core.Enums;

namespace TradeHub.DTOs
{
    public class RegisterDto : IValidatableObject
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserRole Role { get; set; }
        //Business
        public string? BusinessName { get; set; }
        public int? LocationId { get; set; }
        public string? LogoUrl { get; set; }
        public int? BusinessTypeId { get; set; }
        public string? TaxNumber { get; set; }

        public string LoginProvider { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AccountType == AccountType.Individual)
            {
                if (string.IsNullOrWhiteSpace(FirstName))
                    yield return new ValidationResult("FirstName is required", new[] { nameof(FirstName) });

                if (string.IsNullOrWhiteSpace(LastName))
                    yield return new ValidationResult("LastName is required", new[] { nameof(LastName) });
            }
            else if (AccountType == AccountType.Business)
            {
                if (string.IsNullOrWhiteSpace(BusinessName))
                    yield return new ValidationResult("BusinessName is required", new[] { nameof(BusinessName) });

                if (LocationId == null)
                    yield return new ValidationResult("LocationId is required", new[] { nameof(LocationId) });

                if (BusinessTypeId == null)
                    yield return new ValidationResult("BusinessTypeId is required", new[] { nameof(BusinessTypeId) });

                if (string.IsNullOrWhiteSpace(FirstName))
                    yield return new ValidationResult("FirstName is required", new[] { nameof(FirstName) });

                if (string.IsNullOrWhiteSpace(LastName))
                    yield return new ValidationResult("LastName is required", new[] { nameof(LastName) });

                if (string.IsNullOrWhiteSpace(TaxNumber))
                    yield return new ValidationResult("TaxNumber is required", new[] { nameof(TaxNumber) });

                if (string.IsNullOrWhiteSpace(LogoUrl))
                    yield return new ValidationResult("TaxNumber is required", new[] { nameof(LogoUrl) });
            }
        }
    }

}
