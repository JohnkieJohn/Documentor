using Documentor.Models;
using Documentor.Tools;
using Documentor.ViewModels.Details;
using Documentor.Views;
using Documentor.Views.Details;
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

namespace Documentor
{
    /// <summary>
    /// Logique d'interaction pour DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        private readonly TemplateDetailsViewModel viewModel;
        private readonly MDocument currentDoc;

        public DetailsWindow(MDocument document)
        {
            InitializeComponent();
            this.currentDoc = document;
            this.viewModel = new TemplateDetailsViewModel(currentDoc);
            TemplateDetails detailsPage = new TemplateDetails(this.viewModel);
            DocumentInsight documentView = new DocumentInsight(this.viewModel);
            Properties properties = new Properties(this.viewModel.GetPropertiesViewModel());
            this.DataContext = currentDoc;
            this.WindowState = WindowState.Maximized;
            this.ActionFrame.Navigate(detailsPage);
            this.DocumentFrame.Navigate(documentView);
            this.PropertiesFrame.Navigate(properties);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {
                this.viewModel.DocumentThumbZIndex = -1;
            }
            else
            {
                this.viewModel.DocumentThumbZIndex = 1;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            this.viewModel.DocumentThumbZIndex = -1;
        }

        public event EventHandler DetailsWindowClosed;

        private void DetailsWindow_Closed(object sender, EventArgs e)
        {
            this.DetailsWindowClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}
