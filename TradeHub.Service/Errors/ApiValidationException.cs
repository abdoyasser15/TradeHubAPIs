using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Errors;

namespace TradeHub.Service.Errors
{
    public class ApiValidationException : Exception
    {
        public ApiValidationErrorResponse ErrorResponse { get; }

        public ApiValidationException(IEnumerable<string> errors)
            : base(string.Join(", ", errors))
        {
            ErrorResponse = new ApiValidationErrorResponse { Errors = errors };
        }
    }
}
