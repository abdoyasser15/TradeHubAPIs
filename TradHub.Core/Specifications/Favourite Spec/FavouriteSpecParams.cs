using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Specifications.Favourite_Spec
{
    public class FavouriteSpecParams
    {
        private const int MaxPageSize = 50;

        public string? Search { get; set; }

        public string? UserId { get; set; }

        public int? ProductId { get; set; }

        public int PageIndex { get; set; } = 1;

        private int pageSize = 10;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
