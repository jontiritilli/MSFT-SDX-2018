using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class AccessoriesPage : Page
    {
        private AccessoriesViewModel ViewModel
        {
            get { return DataContext as AccessoriesViewModel; }
        }

        public AccessoriesPage()
        {
            InitializeComponent();
        }
    }
}
