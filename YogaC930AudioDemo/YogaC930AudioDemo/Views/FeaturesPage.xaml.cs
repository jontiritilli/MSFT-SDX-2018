using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class FeaturesPage : Page
    {
        private FeaturesViewModel ViewModel
        {
            get { return DataContext as FeaturesViewModel; }
        }

        public FeaturesPage()
        {
            InitializeComponent();
        }
    }
}
