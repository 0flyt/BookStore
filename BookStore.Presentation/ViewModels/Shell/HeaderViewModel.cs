using BookStore.Presentation.Commands;
using BookStore.Presentation.State;
using BookStore.Presentation.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore.Presentation.ViewModels.Shell
{
    internal class HeaderViewModel : ViewModelBase
    {
        private readonly UserSession _session;
        private readonly MainWindowViewModel _main;

        public UserSession Session => _session;

        public ICommand LogoutCommand { get; }

        public HeaderViewModel(UserSession session, MainWindowViewModel main)
        {
            _session = session;
            _main = main;

            LogoutCommand = new DelegateCommand(_ => Logout());
        }

        private void Logout()
        {
            _session.CurrentUser = null;
            _session.CurrentStore = null;

            _main.CurrentView = new LoginViewModel(_session, _main);
        }
    }
}
