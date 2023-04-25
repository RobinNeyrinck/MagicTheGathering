using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public bool ValidFilters => validateFilters();

        private bool validateFilters()
        {
            throw new NotImplementedException();
        }
    }
}
