using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Presentation.ViewModels.Books
{
    internal class BookFormViewModel : ViewModelBase
    {
        private readonly Book? _originalBook;
        public bool IsEditMode => _originalBook != null;
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisedPropertyChanged();
            }
        }

        private string _isbn = string.Empty;
        public string Isbn
        {
            get => _isbn;
            set 
            { 
                _isbn = value;
                RaisedPropertyChanged();
            }
        }

        private decimal? _price;

        public decimal? Price
        {
            get => _price;
            set 
            { 
                _price = value;
                RaisedPropertyChanged();
            }
        }

        private string _language = string.Empty;

        public string Language
        {
            get => _language;
            set 
            { 
                _language = value;
                RaisedPropertyChanged();
            }
        }

        private int? _releaseYear;
        public int? ReleaseYear
        {
            get => _releaseYear;
            set
            {
                _releaseYear = value;
                RaisedPropertyChanged();
            }
        }


        public ObservableCollection<Format> Formats { get; }

        private Format? _selectedFormat;
        public Format? SelectedFormat
        {
            get => _selectedFormat;
            set
            {
                _selectedFormat = value;
                RaisedPropertyChanged();
            }
        }

        public ObservableCollection<Genre> Genres { get; }

        private Genre? _selectedGenre;
        public Genre? SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                RaisedPropertyChanged();
            }
        }

        public ObservableCollection<Author> AllAuthors { get; }

        private Author? _selectedAuthor;

        public Author? SelectedAuthor
        {
            get => _selectedAuthor;
            set 
            { 
                _selectedAuthor = value;
                RaisedPropertyChanged();
                AddAuthorCommand.RaiseAndExecuteChanged();
            }
        }

        public ObservableCollection<Author> SelectedAuthors { get; }

        public DelegateCommand AddAuthorCommand { get; }
        public DelegateCommand RemoveAuthorCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public Action<bool>? CloseAction { get; set; }

        public BookFormViewModel(Book? selectedBook)
        {
            _originalBook = selectedBook;

            using var db = new BookStoreContext();

            Formats = new ObservableCollection<Format>(db.Formats.ToList());
            Genres = new ObservableCollection<Genre>(db.Genres.ToList());
            AllAuthors = new ObservableCollection<Author>(db.Authors
                .OrderBy(a => a.LastName)
                .ToList()
            );

            if (selectedBook != null)
            {
                Title = selectedBook.Title;
                Price = selectedBook.Price;
                Language = selectedBook.Language;
                ReleaseYear = selectedBook.ReleaseDate.Year;
                Isbn = selectedBook.Isbn13;
                SelectedFormat = Formats.FirstOrDefault(f => f.FormatId == selectedBook.FormatId);
                SelectedGenre = Genres.FirstOrDefault(g => g.GenreId == selectedBook.GenreId);
                SelectedAuthors = new ObservableCollection<Author>(selectedBook.Authors);
            }
            else
            {
                //Title = string.Empty;
                //Price = 0;
                //Language = string.Empty;
                //Isbn = string.Empty;
                //SelectedFormat = string.Empty;
                ReleaseYear = null;
                SelectedAuthors = new ObservableCollection<Author>();
            }

            AddAuthorCommand = new DelegateCommand(_ => AddAuthor(), _ => SelectedAuthor != null);
            RemoveAuthorCommand = new DelegateCommand(
                a =>
                {
                    if (a is Author author)
                        RemoveAuthor(author);
                },
                a => a is Author
            );
            SaveCommand = new DelegateCommand(_ => Save());
            CancelCommand = new DelegateCommand(_ => CloseAction?.Invoke(false));
        }

        private void AddAuthor()
        {

            if (SelectedAuthor == null) return;
            if (SelectedAuthors.Contains(SelectedAuthor)) return;

            SelectedAuthors.Add(SelectedAuthor);
        }

        private void RemoveAuthor(Author author)
        {
            SelectedAuthors.Remove(author);
        }



        private void Save()
        {
            using var db = new BookStoreContext();

            Book book;

            if (IsEditMode)
            {
                book = db.Books
                    .Include(b => b.Authors)
                    .First(b => b.Isbn13 == _originalBook!.Isbn13);
            }
            else
            {
                book = new Book { Isbn13 = Isbn };
                db.Books.Add(book);
            }

            book.Title = Title;
            book.Isbn13 = Isbn;
            book.Price = Price ?? 0;
            book.Language = Language;
            book.ReleaseDate = ReleaseYear.HasValue ? new DateOnly(ReleaseYear.Value, 1, 1) : new DateOnly();

            book.FormatId = SelectedFormat!.FormatId;
            book.GenreId = SelectedGenre!.GenreId;

            book.Authors.Clear();

            foreach (var author in SelectedAuthors)
            {
                var authorInDb = db.Authors.Find(author.Id);
                if (authorInDb != null)
                {
                    book.Authors.Add(authorInDb);
                }
            }

            db.SaveChanges();

            CloseAction?.Invoke(true);
        }


    }
}
