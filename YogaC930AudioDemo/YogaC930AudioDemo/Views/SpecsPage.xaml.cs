using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class SpecsPage : Page
    {
        private SpecsViewModel ViewModel
        {
            get { return DataContext as SpecsViewModel; }
        }

        public SpecsPage()
        {
            InitializeComponent();
        }
    }
}
