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
        private readonly StoreBook? _originalStoreBook;

        public bool IsEditMode => _originalStoreBook != null;

        public ObservableCollection<Book> Books { get; }

        private Book? _selectedBook;
        public Book? SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                RaisedPropertyChanged();
                SaveCommand?.RaiseAndExecuteChanged();
            }
        }

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

        public InventoryFormViewModel(Store store, DisplayInventory? inventory)
        {
            _store = store;

            using var db = new BookStoreContext();

            var existingBooksInStore = db.StoreBooks
                .Where(sb => sb.StoreId == store.StoreId)
                .Select(sb => sb.Isbn13)
                .ToHashSet();

            Books = new ObservableCollection<Book>(
                db.Books
                  .Where(b => !existingBooksInStore.Contains(b.Isbn13))
                  .OrderBy(b => b.Title)
                  .ToList()
            );

            if (inventory != null)
            {
                _originalStoreBook = db.StoreBooks
                    .Include(sb => sb.Isbn13Navigation)
                    .First(sb =>
                        sb.StoreId == store.StoreId &&
                        sb.Isbn13 == inventory.Isbn13
                    );

                SelectedBook = Books.First(b => b.Isbn13 == inventory.Isbn13);
                Quantity = inventory.Quantity;
            }
            else
            {
                Quantity = 1;
            }

            SaveCommand = new DelegateCommand(
                _ => Save(),
                _ => CanSave()
            );

            CancelCommand = new DelegateCommand(_ => CloseAction?.Invoke(false));
        }

        private bool CanSave()
        {
            return SelectedBook != null && Quantity >= 0;
        }

        private void Save()
        {
            if (SelectedBook == null)
            {
                MessageBox.Show("En bok måste väljas.");
                return;
            }

            using var db = new BookStoreContext();

            StoreBook storeBook;

            if (IsEditMode)
            {
                storeBook = db.StoreBooks.First(sb =>
                    sb.StoreId == _store.StoreId &&
                    sb.Isbn13 == _originalStoreBook!.Isbn13
                );
            }
            else
            {
                bool exists = db.StoreBooks.Any(sb =>
                    sb.StoreId == _store.StoreId &&
                    sb.Isbn13 == SelectedBook.Isbn13
                );

                if (exists)
                {
                    MessageBox.Show(
                        "Denna bok finns redan i butikens lager.",
                        "Fel",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                storeBook = new StoreBook
                {
                    StoreId = _store.StoreId,
                    Isbn13 = SelectedBook.Isbn13
                };

                db.StoreBooks.Add(storeBook);
            }

            storeBook.QuantityInStock = Quantity;

            try
            {
                db.SaveChanges();
                CloseAction?.Invoke(true);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Kunde inte spara lagersaldot.",
                    "Fel",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}