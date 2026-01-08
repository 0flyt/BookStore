using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
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
        public NavigationViewModel Navigation { get; }
        public MainWindowViewModel()
        {
            Navigation = new NavigationViewModel(this);
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
