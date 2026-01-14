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

        private int _totalTitles;
        public int TotalTitles
        {
            get => _totalTitles;
            private set
            {
                _totalTitles = value;
                RaisedPropertyChanged();
            }
        }

        private int _totalQuantity;
        public int TotalQuantity
        {
            get => _totalQuantity;
            private set
            {
                _totalQuantity = value;
                RaisedPropertyChanged();
            }
        }

        private decimal _totalValue;
        public decimal TotalValue
        {
            get => _totalValue;
            private set
            {
                _totalValue = value;
                RaisedPropertyChanged();
            }
        }

        public ObservableCollection<DisplayInventory> Inventory { get; } = new();

        public InventoryViewModel(UserSession session)
        {
            Session = session;

            using var db = new BookStoreContext();

            Stores = new ObservableCollection<Store>(db.Stores.ToList());

            SelectedStore = Stores.FirstOrDefault(s => s.StoreId == Session.CurrentStore?.StoreId);
        }

        private void LoadInventory()
        {
            Inventory.Clear();

            if (SelectedStore == null)
            {
                TotalTitles = 0;
                TotalQuantity = 0;
                TotalValue = 0;
                return;
            }

            using var db = new BookStoreContext();

            var items = db.StoreBooks
                .Where(sb => sb.StoreId == SelectedStore.StoreId)
                .Select(sb => new DisplayInventory
                {
                    Title = sb.Isbn13Navigation.Title,
                    Isbn13 = sb.Isbn13,
                    Language = sb.Isbn13Navigation.Language,
                    Genre = sb.Isbn13Navigation.Genre.Name,
                    Format = sb.Isbn13Navigation.Format.Name,
                    Authors = string
                        .Join(", ", sb.Isbn13Navigation.Authors
                        .Select(a => a.FirstName + " " + a.LastName)),
                    Quantity = sb.QuantityInStock,
                    Price = sb.Isbn13Navigation.Price
                }).ToList();

                foreach (var item in items)
                {
                    Inventory.Add(item);
                }

                TotalTitles = items.Count;
                TotalQuantity = items.Sum(i => i.Quantity);
                TotalValue = items.Sum(i => i.PriceAndQuantitiy);
        }
        public class DisplayInventory
        {
            public string Title { get; set; } = "";
            public string Isbn13 { get; set; } = "";
            public string Language { get; set; } = "";
            public string Genre { get; set; } = "";
            public string Format { get; set; } = "";
            public string Authors { get; set; } = "";
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal PriceAndQuantitiy => Price * Quantity;
        }
    }
}