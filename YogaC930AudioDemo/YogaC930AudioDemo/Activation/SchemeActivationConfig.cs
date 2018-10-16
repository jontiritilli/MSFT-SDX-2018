using System;
using System.Collections.Generic;
using System.Linq;

namespace YogaC930AudioDemo.Activation
{
    public static class SchemeActivationConfig
    {
        private static readonly Dictionary<string, string> _activationViewModels = new Dictionary<string, string>()
        {
            { "attractor", typeof(ViewModels.AttractorLoopViewModel).FullName },
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
