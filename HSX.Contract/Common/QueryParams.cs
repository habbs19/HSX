using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSX.Contract.Common
{
    public class QueryParams
    {
        public int Limit { get; set; } = 10;
        public int Offset { get; set; } = 0;

        //public int Page { get; set; } = 1;
        //public int PageSize { get; set; }
    }
}
