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
        private readonly MainWindowViewModel _main;
        public bool IsBeforeSearch { get; private set; } = true;
        public bool HasResults => Books != null && Books.Count > 0 && !IsBeforeSearch;
        public bool IsEmptyResult => !HasResults && !IsBeforeSearch;

        private bool _showOnlyCurrentStore = false;
        public bool ShowOnlyCurrentStore
        {
            get => _showOnlyCurrentStore;
            set
            {
                _showOnlyCurrentStore = value;
                RaisedPropertyChanged();
                _ = SearchBooks();
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
        public BooksViewModel(UserSession session, MainWindowViewModel main)
        {
            Session = session;
            _main = main;
			SearchCommand = new DelegateCommand(async _ => await SearchBooks());
            AddBookCommand = new DelegateCommand(_ => AddBook());
            EditBookCommand = new DelegateCommand(_ => EditBook(), _ => SelectedBook != null);
            DeleteBookCommand = new DelegateCommand(async _ => await DeleteBook(), _ => SelectedBook != null);
        }

        private async Task LoadBooks()
        {
            _main.IsBusy = true;
            try
            {
                using var db = new BookStoreContext();

                var books = await db.Books
                    .Include(b => b.Authors)
                    .Include(b => b.Format)
                    .Include(b => b.Genre)
                    .Include(b => b.StoreBooks)
                        .ThenInclude(sb => sb.Store)
                    .ToListAsync();

                Books = new ObservableCollection<Book>(books);
                IsBeforeSearch = true;
                RaisedPropertyChanged(nameof(Books));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kunde inte ladda böcker: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SearchBooks()
		{
            _main.IsBusy = true;
            try
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

                if (ShowOnlyCurrentStore && Session.CurrentStore != null)
                {
                    query = query.Where(b =>
                        b.StoreBooks.Any(sb => sb.StoreId == Session.CurrentStore.StoreId)
                    );
                }

                var results = await query.ToListAsync();

                Books = new ObservableCollection<Book>(results);
                IsBeforeSearch = false;

                RaisedPropertyChanged(nameof(Books));
                RaisedPropertyChanged(nameof(IsBeforeSearch));
                RaisedPropertyChanged(nameof(HasResults));
                RaisedPropertyChanged(nameof(IsEmptyResult));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kunde inte söka böcker: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _main.IsBusy = false;
            }
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

        private void OpenBookForm(Book? selectedBook)
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
                _ = LoadBooks();
            }
        }

        private async Task DeleteBook()
        {
            if (SelectedBook == null) return;

            var result = MessageBox.Show(
                $"Är du säker på att du vill ta bort '{SelectedBook.Title}' från databasen?",
                "Bekräfta borttagning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            _main.IsBusy = true;
            try
            {
                using var db = new BookStoreContext();

                var book = await db.Books
                    .Include(b => b.Authors)
                    .Include(b => b.StoreBooks)
                    .FirstOrDefaultAsync(b => b.Isbn13 == SelectedBook.Isbn13);

                if (book != null)
                {
                    book.Authors.Clear();
                    book.StoreBooks.Clear();

                    db.Books.Remove(book);

                    await db.SaveChangesAsync();
                    await LoadBooks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kunde inte ta bort boken: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _main.IsBusy = false;
            }
        }
    }
}
