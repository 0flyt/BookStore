using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain
{
    public partial class Store
    {
        public string DisplayStoreName => $"{StoreName} {Address}";
        public string CityAndAddress => $"{City}, {Address}";
    }
}
