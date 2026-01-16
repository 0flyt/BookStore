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
using System.Windows;

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

        private DateTime? _released;
        public DateTime? Released
        {
            get => _released;
            set
            {
                _released = value;
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
                Released = selectedBook.ReleaseDate.ToDateTime(TimeOnly.MinValue);
                Isbn = selectedBook.Isbn13;
                SelectedFormat = Formats.FirstOrDefault(f => f.FormatId == selectedBook.FormatId);
                SelectedGenre = Genres.FirstOrDefault(g => g.GenreId == selectedBook.GenreId);
                SelectedAuthors = new ObservableCollection<Author>(selectedBook.Authors);
            }
            else
            {
                Released = null;
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
            SaveCommand = new DelegateCommand(async _ => await Save());
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

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Titel måste anges.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Isbn))
            {
                MessageBox.Show("ISBN måste anges.");
                return;
            }

            if (Isbn.Length != 13 || !Isbn.All(char.IsDigit))
            {
                MessageBox.Show(
                    "ISBN måste bestå av exakt 13 siffror.",
                    "Fel",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (SelectedFormat == null || SelectedGenre == null)
            {
                MessageBox.Show("Format och genre måste väljas.");
                return;
            }

            if (SelectedAuthors.Count == 0)
            {
                MessageBox.Show("Minst en författare måste anges. Om författare inte finns i listan måste denna läggas in innan bok kan skapas.");
                return;
            }

            using var db = new BookStoreContext();

            Book book;

            if (IsEditMode)
            {
                book = await db.Books
                    .Include(b => b.Authors)
                    .FirstAsync(b => b.Isbn13 == _originalBook!.Isbn13);
            }
            else
            {
                if (await db.Books.AnyAsync(b => b.Isbn13 == Isbn))
                {
                    MessageBox.Show("En bok med detta ISBN finns redan.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                book = new Book { Isbn13 = Isbn };
                db.Books.Add(book);
            }

            book.Title = Title;
            book.Isbn13 = Isbn;
            book.Price = Price ?? 0;
            book.Language = Language;

            if (Released.HasValue)
            {
                book.ReleaseDate = DateOnly.FromDateTime(Released.Value);
            }

            book.FormatId = SelectedFormat!.FormatId;
            book.GenreId = SelectedGenre!.GenreId;

            book.Authors.Clear();

            foreach (var author in SelectedAuthors)
            {
                var authorInDb = await db.Authors.FindAsync(author.Id);
                if (authorInDb != null)
                {
                    book.Authors.Add(authorInDb);
                }
            }


            try
            {
                await db.SaveChangesAsync();
                CloseAction?.Invoke(true);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show(
                    "Kunde inte spara boken.\nKontrollera att ISBN är unikt och att alla fält är korrekt ifyllda.",
                    "Databasfel",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Ett oväntat fel inträffade.",
                    "Fel",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}
