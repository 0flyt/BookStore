using BookStore.Domain;
using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.Commands;
using BookStore.Presentation.State;
using BookStore.Presentation.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            using var db = new BookStoreContext();

			_session = session;
			_main = main;

			Users = new ObservableCollection<Employee>(db.Employees.ToList());
			Stores = new ObservableCollection<Store>(db.Stores.ToList());

			LoginCommand = new DelegateCommand(_ => Login(), _ => CanLogin());
        }

        private bool CanLogin() => SelectedUser != null || SelectedStore != null;
        private void Login()
        {
			if (SelectedUser == null || SelectedStore == null) return;

			if (App.Current.MainWindow.DataContext is MainWindowViewModel main)
			{
				_session.CurrentUser = SelectedUser;
				_session.CurrentStore = SelectedStore;

				main.CurrentView = new BooksViewModel(_session);
			}
        }
    }
}
