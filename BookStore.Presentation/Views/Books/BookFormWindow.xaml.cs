using BookStore.Presentation.ViewModels.Books;
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

namespace BookStore.Presentation.Views.Books
{
    /// <summary>
    /// Interaction logic for BookFormWindow.xaml
    /// </summary>
    public partial class BookFormWindow : Window
    {
        public BookFormWindow()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                if (DataContext is BookFormViewModel vm)
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
