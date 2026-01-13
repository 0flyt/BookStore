using BookStore.Presentation.ViewModels.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStore.Presentation.Views.Authors
{
    /// <summary>
    /// Interaction logic for AuthorFormWindow.xaml
    /// </summary>
    public partial class AuthorFormWindow : Window
    {
        public AuthorFormWindow()
        {
            InitializeComponent();

            Loaded += (_, __) =>
            {
                if (DataContext is AuthorFormViewModel vm)
                {
                    vm.CloseAction = result =>
                    {
                        DialogResult = result;
                        Close();
                    };
                }
            };
        }
    }
}
