using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;


namespace YogaC930AudioDemo.Helpers
{
    public static class StyleHelper
    {
        public static Style GetApplicationStyle(String name)
        {
            Style style = null;
            try
            {
                style = (Style)Application.Current.Resources[name];
            }
            catch
            {
            }

            return style;
        }
    }
}
