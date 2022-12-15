using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Entity.Helper
{
    public class RequestParameter
    {
        const int MaxPageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int _PageSize { get; set; } = 10;
        public int PageSize { get { return _PageSize; } set { _PageSize = (value > PageSize) ? MaxPageSize : value; } }
    }
}