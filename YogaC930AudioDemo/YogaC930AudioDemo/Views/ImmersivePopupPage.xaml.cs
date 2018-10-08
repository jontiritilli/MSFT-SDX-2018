using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class ImmersivePopupPage : Page
    {
        private ImmersivePopupViewModel ViewModel
        {
            get { return DataContext as ImmersivePopupViewModel; }
        }

        public ImmersivePopupPage()
        {
            InitializeComponent();
        }
    }
}
