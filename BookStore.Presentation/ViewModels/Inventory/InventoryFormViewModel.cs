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
using static BookStore.Presentation.ViewModels.Inventory.InventoryViewModel;

namespace BookStore.Presentation.ViewModels.Inventory
{
    internal class InventoryFormViewModel : ViewModelBase
    {
        private readonly Store _store;
        public Store Store => _store;
        private readonly string _isbn13;

        public string Title { get; }
        public string Isbn13 => _isbn13;

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                RaisedPropertyChanged();
                SaveCommand?.RaiseAndExecuteChanged();
            }
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public Action<bool>? CloseAction { get; set; }

        public InventoryFormViewModel(Store store, DisplayInventory inventory)
        {
            _store = store;
            _isbn13 = inventory.Isbn13;
            Title = inventory.Title;
            Quantity = inventory.Quantity;
            RaisedPropertyChanged(nameof(Quantity));

            SaveCommand = new DelegateCommand(_ => Save(), _ => Quantity >= 0);
            CancelCommand = new DelegateCommand(_ => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            using var db = new BookStoreContext();

            var storeBook = db.StoreBooks.FirstOrDefault(sb =>
                sb.StoreId == _store.StoreId &&
                sb.Isbn13 == _isbn13
            );

            if (storeBook == null)
            {
                storeBook = new StoreBook
                {
                    StoreId = _store.StoreId,
                    Isbn13 = _isbn13
                };
                db.StoreBooks.Add(storeBook);
            }

            storeBook.QuantityInStock = Quantity;

            try
            {
                db.SaveChanges();
                CloseAction?.Invoke(true);
            }
            catch
            {
                MessageBox.Show(
                    "Något gick fel. Kunde inte spara.",
                    "Fel",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}