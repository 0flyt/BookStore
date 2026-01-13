using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.Commands;
using BookStore.Presentation.State;
using BookStore.Presentation.Views.Books;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStore.Presentation.ViewModels.Books
{
    internal class BooksViewModel : ViewModelBase
    {
        public UserSession Session { get; }
        public bool IsBeforeSearch { get; private set; } = true;
        public bool HasResults => Books != null && Books.Count > 0 && !IsBeforeSearch;
        public bool IsEmptyResult => !HasResults && !IsBeforeSearch;

        private bool _showOnlyCurrentStore = true;
        public bool ShowOnlyCurrentStore
        {
            get => _showOnlyCurrentStore;
            set
            {
                _showOnlyCurrentStore = value;
                RaisedPropertyChanged();
                SearchBooks();
            }
        }


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
                EditBookCommand.RaiseAndExecuteChanged();
                DeleteBookCommand.RaiseAndExecuteChanged();
            }
        }


        public DelegateCommand SearchCommand { get; }
        public DelegateCommand AddBookCommand { get; }
        public DelegateCommand EditBookCommand { get; }
        public DelegateCommand DeleteBookCommand { get; }
        public BooksViewModel(UserSession session)
        {
            Session = session;
			SearchCommand = new DelegateCommand(_ => SearchBooks());
            AddBookCommand = new DelegateCommand(_ => AddBook());
            EditBookCommand = new DelegateCommand(_ => EditBook(), _ => SelectedBook != null);
            DeleteBookCommand = new DelegateCommand(_ => DeleteBook(), _ => SelectedBook != null);


            LoadBooks();
        }

        private void LoadBooks()
        {
            using var db = new BookStoreContext();

            Books = new ObservableCollection<Book>(db.Books
                .Include(b => b.Authors)
                .Include(b => b.Format)
                .Include(b => b.Genre)
                .Include(b => b.StoreBooks)
                    .ThenInclude(sb => sb.Store)
                .ToList()
                );
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

            //if (Session.CurrentStore != null)
            //{
            //    query = query.Where(b =>
            //        b.StoreBooks.Any(sb => sb.StoreId == Session.CurrentStore.StoreId)
            //    );
            //}

            if (ShowOnlyCurrentStore && Session.CurrentStore != null)
            {
                query = query.Where(b =>
                    b.StoreBooks.Any(sb => sb.StoreId == Session.CurrentStore.StoreId)
                );
            }

            Books = new ObservableCollection<Book>(query.ToList());
            RaisedPropertyChanged(nameof(Books));
            IsBeforeSearch = false;
            RaisedPropertyChanged(nameof(IsBeforeSearch));
            RaisedPropertyChanged(nameof(HasResults));
            RaisedPropertyChanged(nameof(IsEmptyResult));
        }

        private void AddBook()
        {
            OpenBookForm(null);
        }

        private void EditBook()
        {
            if (SelectedBook == null) return;
            OpenBookForm(SelectedBook);
        }

        private void OpenBookForm(Book selectedBook)
        {
            var vm = new BookFormViewModel(selectedBook);

            var window = new BookFormWindow
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow
            };

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadBooks();
            }
        }

        private void DeleteBook()
        {
            if (SelectedBook == null) return;

            using var db = new BookStoreContext();
            var book = db.Books.Find(SelectedBook.Isbn13);
            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();
            }

            LoadBooks();
        }

    }
}
