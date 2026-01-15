using BookStore.Presentation.ViewModels.Books;
using BookStore.Presentation.ViewModels.Inventory;
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

namespace BookStore.Presentation.Views.Inventory
{
    /// <summary>
    /// Interaction logic for InventoryFormWindow.xaml
    /// </summary>
    public partial class InventoryFormWindow : Window
    {
        public InventoryFormWindow()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                if (DataContext is InventoryFormViewModel vm)
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
