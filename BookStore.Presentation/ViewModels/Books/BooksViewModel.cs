using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.Commands;
using BookStore.Presentation.State;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Presentation.ViewModels.Books
{
    internal class BooksViewModel : ViewModelBase
    {
        public UserSession Session { get; }
        public bool IsBeforeSearch { get; private set; } = true;
        public bool HasResults => Books != null && Books.Count > 0 && !IsBeforeSearch;
        public bool IsEmptyResult => !HasResults && !IsBeforeSearch;

        private string _searchText = string.Empty;
        public string SearchText
		{
			get => _searchText;
			set 
			{ 
				_searchText = value;
				RaisedPropertyChanged();
			}
		}

		private ObservableCollection<Book> _books = new();
		public ObservableCollection<Book> Books
		{
			get => _books;
			set 
			{ 
				_books = value;
				RaisedPropertyChanged();
			}
		}
        public bool IsBookSelected => SelectedBook != null;
        private Book? _selectedBook;
        public Book? SelectedBook
        {
            get => _selectedBook;
            set 
            { 
                _selectedBook = value;
                RaisedPropertyChanged();
                RaisedPropertyChanged(nameof(IsBookSelected));
            }
        }


        public DelegateCommand SearchCommand { get; }
        public BooksViewModel(UserSession session)
        {
            Session = session;
			SearchCommand = new DelegateCommand(_ => SearchBooks());
        }

		private void SearchBooks()
		{
            using var db = new BookStoreContext();

            var query = db.Books
                .Include(b => b.Authors)
                .Include(b => b.Format)
                .Include(b => b.Genre)
                .Include(b => b.StoreBooks)
                    .ThenInclude(sb => sb.Store)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.ToLower();

                query = query.Where(b =>
                    b.Title.ToLower().Contains(search) ||
                    b.Isbn13.Contains(SearchText) ||
                    b.Authors.Any(a =>
                        a.FirstName.ToLower().Contains(search) ||
                        a.LastName.ToLower().Contains(search))
                );
            }

            Books = new ObservableCollection<Book>(query.ToList());
            RaisedPropertyChanged(nameof(Books));
            IsBeforeSearch = false;
            RaisedPropertyChanged(nameof(IsBeforeSearch));
            RaisedPropertyChanged(nameof(HasResults));
            RaisedPropertyChanged(nameof(IsEmptyResult));
        }

    }
}
