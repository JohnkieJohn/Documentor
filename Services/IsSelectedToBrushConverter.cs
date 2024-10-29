using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Documentor.Services
{
    public class IsSelectedToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // la valeur est un booléen (IsSelected)
            if (value is bool isSelected)
            {
                return isSelected ? Brushes.LightBlue : Brushes.Transparent;
            }

            // par défaut retourne Transparent
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
