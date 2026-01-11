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
            }
        }


        public DelegateCommand SearchCommand { get; }
        public AuthorsViewModel(UserSession session)
        {
            Session = session;
            SearchCommand = new DelegateCommand(_ => SearchAuthors());
        }

        private void SearchAuthors()
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

            Authors = new ObservableCollection<Author>(query.ToList());
            RaisedPropertyChanged(nameof(Authors));
            IsBeforeSearch = false;
            RaisedPropertyChanged(nameof(IsBeforeSearch));
            RaisedPropertyChanged(nameof(HasResults));
            RaisedPropertyChanged(nameof(IsEmptyResult));
        }

    }
}
