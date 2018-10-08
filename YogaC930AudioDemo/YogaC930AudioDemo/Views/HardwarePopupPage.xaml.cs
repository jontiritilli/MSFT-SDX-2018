using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class HardwarePopupPage : Page
    {
        private HardwarePopupViewModel ViewModel
        {
            get { return DataContext as HardwarePopupViewModel; }
        }

        public HardwarePopupPage()
        {
            InitializeComponent();
        }
    }
}
