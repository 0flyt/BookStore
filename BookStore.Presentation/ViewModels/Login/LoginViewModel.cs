using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.Commands;
using BookStore.Presentation.State;
using BookStore.Presentation.ViewModels.Books;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStore.Presentation.ViewModels.Login
{
    internal class LoginViewModel : ViewModelBase
    {
		private readonly UserSession _session;
		private readonly MainWindowViewModel _main;
		public ObservableCollection<Employee> Users { get; set; }
        public ObservableCollection<Store> Stores { get; set; }

        private Employee? _selectedUser;
		public Employee? SelectedUser
		{
			get => _selectedUser;
			set 
			{ 
				_selectedUser = value;
				RaisedPropertyChanged();
				LoginCommand.RaiseAndExecuteChanged();
			}
		}

		private Store? _selectedStore;
        public Store? SelectedStore
		{
			get => _selectedStore;
			set 
			{ 
				_selectedStore = value;
				RaisedPropertyChanged();
				LoginCommand.RaiseAndExecuteChanged();
			}
		}

        public DelegateCommand LoginCommand { get; }

        public LoginViewModel(UserSession session, MainWindowViewModel main)
        {
            _session = session;
            _main = main;

            Users = new ObservableCollection<Employee>();
            Stores = new ObservableCollection<Store>();

            LoginCommand = new DelegateCommand(_ => Login(), _ => CanLogin());

            _ = Load();
        }

        private async Task Load()
        {
            _main.IsBusy = true;
            try
            {
                using var db = new BookStoreContext();

                var users = await db.Employees.ToListAsync();
                var stores = await db.Stores.ToListAsync();

                Users.Clear();
                foreach (var user in users)
                    Users.Add(user);

                Stores.Clear();
                foreach (var store in stores)
                    Stores.Add(store);
            }
            catch
            {
                MessageBox.Show("Kunde inte komma åt databasen.");
            }
            finally
            {
                _main.IsBusy = false;
            }
        }

        private bool CanLogin() => SelectedUser != null && SelectedStore != null;
        private void Login()
        {
			if (SelectedUser == null || SelectedStore == null) return;

            _session.CurrentUser = SelectedUser;
            _session.CurrentStore = SelectedStore;

            _main.CurrentView = new BooksViewModel(_session, _main);
        }
    }
}
