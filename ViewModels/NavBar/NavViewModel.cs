using Documentor.Tools;
using Documentor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Documentor.ViewModels.NavBar
{
    public class NavViewModel
    {
        public ICommand ViewCommand { get; set; }

        public NavViewModel()
        {
            ViewCommand = new NavigationCommand();
        }
    }
}
