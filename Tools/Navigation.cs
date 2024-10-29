using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Documentor.Tools
{
    public static class Navigation
    {
        private static Frame mainFrame;

        public static void Initialize(Frame Mainframe)
        {
            mainFrame = Mainframe;
        }

        public static Frame GetMainFrame()
        {
            return mainFrame;
        }

        public static void NavigateToPage(Page page)
        {
            mainFrame.Navigate(page);
        }
    }
}
