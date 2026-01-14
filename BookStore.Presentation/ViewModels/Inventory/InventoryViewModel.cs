using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.State;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Presentation.ViewModels.Inventory
{
    internal class InventoryViewModel : ViewModelBase
    {
        public UserSession Session { get; }

        public ObservableCollection<Store> Stores { get; }
        private Store? _selectedStore;
        public Store? SelectedStore
        {
            get => _selectedStore;
            set
            {
                _selectedStore = value;
                RaisedPropertyChanged();
                LoadInventory();
            }
        }

        private ObservableCollection<StoreBook> _inventory = new();
        public ObservableCollection<StoreBook> Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                RaisedPropertyChanged();
            }
        }

        public InventoryViewModel(UserSession session)
        {
            Session = session;

            using var db = new BookStoreContext();

            Stores = new ObservableCollection<Store>(db.Stores.ToList());

            SelectedStore = Stores.FirstOrDefault(s => s.StoreId == Session.CurrentStore?.StoreId);
        }

        private void LoadInventory()
        {
            if (SelectedStore == null)
            {
                Inventory.Clear();
                return;
            }

            using var db = new BookStoreContext();

            var items = db.StoreBooks
                .Include(sb => sb.Isbn13Navigation)
                    .ThenInclude(b => b.Authors)
                .Where(sb => sb.StoreId == SelectedStore.StoreId)
                .ToList();

            Inventory = new ObservableCollection<StoreBook>(items);
        }
    }
}