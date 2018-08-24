using System;

using SurfaceProDemo.Services;
using SurfaceProDemo.ViewModels;

using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SurfaceProDemo.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        private FlipViewViewModel ViewModel
        {
            get { return DataContext as FlipViewViewModel; }
        }

        public ShellPage()
        {
            InitializeComponent();
            HideNavViewBackButton();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView);
            KeyboardAccelerators.Add(ActivationService.AltLeftKeyboardAccelerator);
            KeyboardAccelerators.Add(ActivationService.BackKeyboardAccelerator);
        }

        private void HideNavViewBackButton()
        {
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {
                navigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            }
        }
    }
}
