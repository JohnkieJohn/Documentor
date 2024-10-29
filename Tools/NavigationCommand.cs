using Documentor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Documentor.Tools
{
    public class NavigationCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;

        public NavigationCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true; // Toujours activée pour l'instant, à revoir...
        }

        public void Execute(object viewName)
        {
            // Si le bouton de fermeture de programme est cliqué
            if(viewName.ToString() == "CloseApp")
            {
                if(Xceed.Wpf.Toolkit.MessageBox.Show("Êtes-vous sûr de vouloir quitter le programme ?", "Documentor - Arrêter l'application", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
            else if (viewName.ToString() == "CloseEditor")
            {

            }
            else
            {
                // Récupération du type de vue en utilisant la classe ViewMapping
                var viewType = ViewMapping.GetViewTypeByName(viewName.ToString());

                if (viewType != null)
                {
                    // Création de l'instance de la vue
                    var view = (Page)Activator.CreateInstance(viewType);

                    // Naviguer vers la vue en utilisant la classe Navigation
                    Navigation.NavigateToPage(view);
                }
            }

        }
    }
}
