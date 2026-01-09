using BookStore.Infrastructure.Data.Model;
using BookStore.Presentation.Commands;
using BookStore.Presentation.ViewModels.Authors;
using BookStore.Presentation.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore.Presentation.ViewModels.Shell
{
    internal class NavigationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _main;
        public ICommand ShowBooksCommand { get; }
        public ICommand ShowAuthorsCommand { get; }
        public NavigationViewModel(MainWindowViewModel main)
        {
            _main = main;

            ShowBooksCommand = new DelegateCommand(_ =>
            {
                _main.CurrentView = new BooksViewModel();
            });

            ShowAuthorsCommand = new DelegateCommand(_ =>
            {
                _main.CurrentView = new AuthorsViewModel();
            });
        }
    }
}
