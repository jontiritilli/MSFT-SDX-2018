using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
{
    public sealed partial class AccessoriesKeyboardPage : Page
    {
        private AccessoriesKeyboardViewModel ViewModel
        {
            get { return DataContext as AccessoriesKeyboardViewModel; }
        }

        public AccessoriesKeyboardPage()
        {
            InitializeComponent();
        }
    }
}
