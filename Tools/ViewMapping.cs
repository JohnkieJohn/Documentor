using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Documentor.Tools
{
    public static class ViewMapping
    {
        private static string ViewName; // vue en cours
        public static Type GetViewTypeByName(string viewName)
        {
            if (ViewName != viewName)
            {
                SetViewName(viewName);

                // Recherche parmis les Pages du projet si une correspond au viewName
                var assembly = Assembly.GetExecutingAssembly();
                var pageTypes = assembly.GetTypes()
                .Where(t => typeof(Page).IsAssignableFrom(t) && t != typeof(Page) && !t.IsNested)
                .ToList();

                foreach (var pageType in pageTypes)
                {
                    if (pageType.Name == viewName)
                    {
                        // Retourne le type de vue qui correspond à la Page demandée
                        return pageType;
                    }
                }
            }
            return null;
        }

        public static void SetViewName(string viewName)
        {
            ViewName = viewName;
        }
    }
}
