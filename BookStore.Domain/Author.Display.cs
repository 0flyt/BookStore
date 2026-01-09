using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Domain
{
    public partial class Author
    {
        public string FullName => $"{FirstName} {LastName}";
        public string BookCountDisplay => Isbn13s?.Count > 0 ? $"{Isbn13s.Count} st" : "0 st";

        [NotMapped]
        public IEnumerable<Book> BooksSortedByRelease =>
        Isbn13s?.OrderByDescending(b => b.ReleaseDate) ?? Enumerable.Empty<Book>();

        //public ICollection<Book> Books => Isbn13s;
    }
}
