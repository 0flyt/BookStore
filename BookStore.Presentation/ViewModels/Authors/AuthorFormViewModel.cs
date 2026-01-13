using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookStore.Presentation.ViewModels.Authors
{
    internal class AuthorFormViewModel : ViewModelBase
    {
        private readonly Author? _originalAuthor;
        public bool IsEditMode => _originalAuthor != null;

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                RaisedPropertyChanged();
            }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                RaisedPropertyChanged();
            }
        }

        private DateTime? _birthDate;
        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                RaisedPropertyChanged();
            }
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public Action<bool>? CloseAction { get; set; }

        public AuthorFormViewModel(Author? author)
        {
            _originalAuthor = author;

            if (author != null)
            {
                FirstName = author.FirstName;
                LastName = author.LastName;
                BirthDate = author.Birth.ToDateTime(TimeOnly.MinValue);
            }

            SaveCommand = new DelegateCommand(async _ => await SaveAsync());
            CancelCommand = new DelegateCommand(_ => CloseAction?.Invoke(false));
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show(
                    "Förnamn och efternamn måste fyllas i.",
                    "Fel",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            using var db = new BookStoreContext();

            Author author;
            if (IsEditMode)
            {
                author = await db.Authors.FirstAsync(a => a.Id == _originalAuthor!.Id);
            }
            else
            {
                author = new Author();
                await db.Authors.AddAsync(author);
            }

            author.FirstName = FirstName;
            author.LastName = LastName;

            if (BirthDate.HasValue)
            {
                author.Birth = DateOnly.FromDateTime(BirthDate.Value);
            }

            await db.SaveChangesAsync();

            CloseAction?.Invoke(true);
        }
    }
}
