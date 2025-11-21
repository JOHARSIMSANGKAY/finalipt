using Microsoft.Extensions.DependencyInjection;
using Sangkay.Domain.Entities;
using System.Windows;
// Basser.WPF/MainWindow.xaml.cs
using System;
// Basser.WPF/MainWindow.xaml.cs
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sangkay.Supplier
{
    public partial class MainWindow : Window
    {
        public MainWindow(IServiceProvider services)
        {
            InitializeComponent();

            // Resolve the view model from DI using the type in the current namespace
            DataContext = services.GetRequiredService<SupplierViewModel>();
        }
    }
}