using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.State;
using BookStore.Presentation.ViewModels.Authors;
using BookStore.Presentation.ViewModels.Books;
using BookStore.Presentation.ViewModels.Inventory;
using BookStore.Presentation.ViewModels.Login;
using BookStore.Presentation.ViewModels.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Presentation.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public UserSession Session { get; }
        public NavigationViewModel Navigation { get; }
        public HeaderViewModel Header { get; }

        private ViewModelBase? _currentView;
        public ViewModelBase? CurrentView
        {
            get => _currentView;
            set 
            { 
                _currentView = value;
                RaisedPropertyChanged();
            }
        }
        public MainWindowViewModel()
        {
            Session = new UserSession();
            Header = new HeaderViewModel(Session, this);
            Navigation = new NavigationViewModel(this);
            CurrentView = new LoginViewModel(Session, this);
        }
        public void ShowBooksView()
        {
            CurrentView = new BooksViewModel(Session, this);
        }
        public void ShowAuthorsView()
        {
            CurrentView = new AuthorsViewModel(Session, this);
        }
        public void ShowInventoryView()
        {
            CurrentView = new InventoryViewModel(Session, this);
        }
    }
}
