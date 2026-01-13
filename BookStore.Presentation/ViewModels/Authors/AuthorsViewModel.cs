using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.Commands;
using BookStore.Presentation.State;
using BookStore.Presentation.Views.Authors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStore.Presentation.ViewModels.Authors
{
    internal class AuthorsViewModel : ViewModelBase
    {
        public UserSession Session { get; }
        public bool IsBeforeSearch { get; private set; } = true;
        public bool HasResults => Authors != null && Authors.Count > 0 && !IsBeforeSearch;
        public bool IsEmptyResult => !HasResults && !IsBeforeSearch;

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                RaisedPropertyChanged();
                _ = SearchAuthors();
            }
        }

        private ObservableCollection<Author> _authors = new();
        public ObservableCollection<Author> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                RaisedPropertyChanged();
            }
        }
        public bool IsAuthorSelected => SelectedAuthor != null;

        private Author? _selectedAuthor;
        public Author? SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                RaisedPropertyChanged();
                RaisedPropertyChanged(nameof(IsAuthorSelected));
                EditAuthorCommand.RaiseAndExecuteChanged();
                DeleteAuthorCommand.RaiseAndExecuteChanged();
            }
        }


        public DelegateCommand SearchCommand { get; }
        public DelegateCommand AddAuthorCommand { get; }
        public DelegateCommand EditAuthorCommand { get; }
        public DelegateCommand DeleteAuthorCommand { get; }

        public AuthorsViewModel(UserSession session)
        {
            Session = session;
            SearchCommand = new DelegateCommand(_ => _ = SearchAuthors());
            AddAuthorCommand = new DelegateCommand(_ => OpenAuthorForm(null));
            EditAuthorCommand = new DelegateCommand(_ => OpenAuthorForm(SelectedAuthor), _ => SelectedAuthor != null);
            DeleteAuthorCommand = new DelegateCommand(_ => DeleteAuthor(), _ => SelectedAuthor != null);

        }

        private async Task SearchAuthors()
        {
            using var db = new BookStoreContext();

            var query = db.Authors
                .Include(a => a.Isbn13s)
                .ThenInclude(a => a.Genre)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.ToLower();

                query = query.Where(a =>
                    a.FirstName.ToLower().Contains(search) ||
                    a.LastName.ToLower().Contains(SearchText) ||
                    a.Isbn13s.Any(b =>
                        b.Title.ToLower().Contains(search) ||
                        b.Isbn13.Contains(SearchText)
                    )
                );
            }

            var result = await query.ToListAsync();

            Authors = new ObservableCollection<Author>(query.ToList());
            RaisedPropertyChanged(nameof(Authors));
            IsBeforeSearch = false;
            RaisedPropertyChanged(nameof(IsBeforeSearch));
            RaisedPropertyChanged(nameof(HasResults));
            RaisedPropertyChanged(nameof(IsEmptyResult));
        }

        private void OpenAuthorForm(Author? selectedAuthor = null)
        {
            var vm = new AuthorFormViewModel(selectedAuthor);
            var window = new AuthorFormWindow
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow
            };
            var result = window.ShowDialog();
            if (result == true)
            {
                _ = SearchAuthors();
            }
        }

        private void DeleteAuthor()
        {
            if (SelectedAuthor == null) return;

            var result = MessageBox.Show(
                $"Är du säker på att du vill ta bort '{SelectedAuthor.FullName}' från databasen?",
                "Bekräfta borttagning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            using var db = new BookStoreContext();

            var author = db.Authors
                .Include(a => a.Isbn13s)
                .FirstOrDefault(a => a.Id == SelectedAuthor.Id);

            if (author == null) return;
         
            author.Isbn13s.Clear();

            db.Authors.Remove(author);

            db.SaveChanges();
          
            _ = SearchAuthors();
        }

    }
}
