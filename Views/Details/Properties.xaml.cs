using Documentor.Models;
using Documentor.ViewModels.Details;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Documentor.Views.Details
{
    /// <summary>
    /// Logique d'interaction pour Properties.xaml
    /// </summary>
    public partial class Properties : Page
    {
        private PropertiesViewModel ViewModel;
        public Properties(PropertiesViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
