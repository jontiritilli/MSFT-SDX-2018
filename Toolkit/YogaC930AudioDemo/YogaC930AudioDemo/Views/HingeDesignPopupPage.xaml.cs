using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class HingeDesignPopupPage : Page
    {
        private HingeDesignPopupViewModel ViewModel
        {
            get { return DataContext as HingeDesignPopupViewModel; }
        }

        public HingeDesignPopupPage()
        {
            InitializeComponent();
        }
    }
}
