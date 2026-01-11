using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.State;
using BookStore.Presentation.ViewModels.Authors;
using BookStore.Presentation.ViewModels.Books;
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
            CurrentView = new BooksViewModel(Session);
        }
        public void ShowAuthorsView()
        {
            CurrentView = new AuthorsViewModel(Session);
        }

        //public ObservableCollection<Store> Stores { get; private set; }

        //private Store? _selectedStore;

        //public Store SelectedStore
        //{
        //    get => _selectedStore;
        //    set 
        //    {
        //        _selectedStore = value;
        //        RaisedPropertyChanged();
        //        LoadOrders();
        //    }
        //}

        //public ObservableCollection<Order> Orders { get; private set; }

        //public MainWindowViewModel()
        //{
        //    LoadStores();
        //}

        //private void LoadStores()
        //{
        //    using var db = new BookStoreContext();

        //    Stores = new ObservableCollection<Store>(
        //        db.Stores.ToList()
        //    );

        //    SelectedStore = Stores.FirstOrDefault();
        //}

        //private void LoadOrders()
        //{
        //    if (SelectedStore == null) return;

        //    using var db = new BookStoreContext();

        //    Orders = new ObservableCollection<Order>(
        //        db.Orders
        //        .Where(o => o.DestinationStoreId == SelectedStore.StoreId)
        //        .ToList()
        //    );
        //}
    }
}
