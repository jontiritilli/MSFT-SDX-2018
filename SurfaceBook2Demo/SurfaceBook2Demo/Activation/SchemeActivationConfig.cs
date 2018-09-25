using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceBook2Demo.Activation
{
    public static class SchemeActivationConfig
    {
        private static readonly Dictionary<string, string> _activationViewModels = new Dictionary<string, string>()
        {
            // TODO WTS: Add the pages that can be opened from scheme activation in your app here.
            { "attractor", typeof(ViewModels.AttractorLoopViewModel).FullName },
            //{ "choosepath", typeof(ViewModels.ChoosePathViewModel).FullName },
            { "main", typeof(ViewModels.FlipViewViewModel).FullName },
            { "", typeof(ViewModels.FlipViewViewModel).FullName }
        };

        public static string GetViewModelName(string viewModelKey)
        {
            return _activationViewModels
                    .FirstOrDefault(p => p.Key == viewModelKey)
                    .Value;
        }

        public static string GetViewModelKey(string viewModelName)
        {
            return _activationViewModels
                    .FirstOrDefault(v => v.Value == viewModelName)
                    .Key;
        }
    }
}
