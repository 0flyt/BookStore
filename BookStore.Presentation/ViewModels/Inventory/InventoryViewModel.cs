using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.Commands;
using BookStore.Presentation.State;
using BookStore.Presentation.Views.Inventory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                _ = LoadInventory();
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisedPropertyChanged();
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

        public bool IsInventorySelected => SelectedInventory != null;

        private DisplayInventory? _selectedInventory;
        public DisplayInventory? SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;
                RaisedPropertyChanged();
                RaisedPropertyChanged(nameof(IsInventorySelected));
                EditInventoryCommand.RaiseAndExecuteChanged();
                DeleteInventoryCommand.RaiseAndExecuteChanged();
            }
        }


        public ObservableCollection<DisplayInventory> Inventory { get; } = new();

        public DelegateCommand SearchInventoryCommand { get; }
        public DelegateCommand EditInventoryCommand { get; }
        public DelegateCommand DeleteInventoryCommand { get; }

        public InventoryViewModel(UserSession session)
        {
            Session = session;

            SearchInventoryCommand = new DelegateCommand(async _ => await LoadInventory());
            EditInventoryCommand = new DelegateCommand(_ => EditInventory(), _ => SelectedInventory != null);
            DeleteInventoryCommand = new DelegateCommand(_ => DeleteInventory(), _ => SelectedInventory != null);


            using var db = new BookStoreContext();

            Stores = new ObservableCollection<Store>(db.Stores.ToList());

            SelectedStore = Stores.FirstOrDefault(s => s.StoreId == Session.CurrentStore?.StoreId);
        }

        private void EditInventory()
        {
            if (SelectedInventory == null) return;
            OpenInventoryForm(SelectedInventory);
        }
        private async void DeleteInventory()
        {
            if (SelectedInventory == null || SelectedStore == null)
                return;

            var result = MessageBox.Show(
                $"Vill du ta bort '{SelectedInventory.Title}' från lagret?",
                "Bekräfta borttagning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            using var db = new BookStoreContext();

            var storeBook = await db.StoreBooks.FirstOrDefaultAsync(sb => sb.StoreId == SelectedStore.StoreId && sb.Isbn13 == SelectedInventory.Isbn13);

            if (storeBook != null)
            {
                storeBook.QuantityInStock = 0;
                await db.SaveChangesAsync();
            }

            await LoadInventory();
        }

        private void OpenInventoryForm(DisplayInventory? inventory)
        {
            var vm = new InventoryFormViewModel(SelectedStore!, inventory);

            var window = new InventoryFormWindow
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow
            };

            if (window.ShowDialog() == true)
            {
                _ = LoadInventory();
            }
        }

        private async Task LoadInventory()
        {
            IsBusy = true;
            Inventory.Clear();

            if (SelectedStore == null)
            {
                TotalTitles = 0;
                TotalQuantity = 0;
                TotalValue = 0;
                return;
            }

            var items = await SearchInventory();
            foreach (var item in items)
                Inventory.Add(item);

            TotalTitles = items.Count(i => i.Quantity > 0);
            TotalQuantity = items.Sum(i => i.Quantity);
            TotalValue = items.Sum(i => i.PriceAndQuantitiy);
        }
        private async Task<List<DisplayInventory>> SearchInventory()
        {
            using var db = new BookStoreContext();

            var query = db.Books
                .Include(b => b.Authors)
                .Include(b => b.Genre)
                .Include(b => b.Format)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(search) ||
                    b.Isbn13.Contains(SearchText) ||
                    b.Authors.Any(a =>
                        a.FirstName.ToLower().Contains(search) ||
                        a.LastName.ToLower().Contains(search))
                );
            }

            var books = await query.ToListAsync();

            var storeBooks = await db.StoreBooks
                .Where(sb => sb.StoreId == SelectedStore!.StoreId)
                .ToListAsync();

            var items = books.Select(b =>
            {
                var sb = storeBooks.FirstOrDefault(s => s.Isbn13 == b.Isbn13);
                return new DisplayInventory
                {
                    Title = b.Title,
                    Isbn13 = b.Isbn13,
                    Language = b.Language,
                    Genre = b.Genre.Name,
                    Format = b.Format.Name,
                    Authors = string.Join(", ", b.Authors.Select(a => a.FirstName + " " + a.LastName)),
                    Quantity = sb?.QuantityInStock ?? 0,
                    Price = b.Price
                };
            }).ToList();

            IsBusy = false;
            return items;

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