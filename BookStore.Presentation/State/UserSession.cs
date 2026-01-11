using BookStore.Domain;
using BookStore.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Presentation.State
{
    internal class UserSession : ViewModelBase
    {
        private Employee? _currentUser;
        public Employee? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                RaisedPropertyChanged();
                RaisedPropertyChanged(nameof(IsLoggedIn));
            }
        }

        private Store? _currentStore;
        public Store? CurrentStore
        {
            get => _currentStore;
            set
            {
                _currentStore = value;
                RaisedPropertyChanged();
                RaisedPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public bool IsLoggedIn => CurrentUser != null && CurrentStore != null;
    }
}
