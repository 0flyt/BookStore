using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain
{
    public partial class Book
    {
        public string AuthorNames => string.Join(", ", Authors.Select(a => $"{a.FirstName} {a.LastName}"));
    }
}
