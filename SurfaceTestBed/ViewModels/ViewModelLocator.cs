using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using SurfaceTestBed.Services;
using SurfaceTestBed.Views;

namespace SurfaceTestBed.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            Register<PivotViewModel, PivotPage>();
            Register<FlipViewViewModel, FlipViewPage>();
            Register<Page1ViewModel, Page1Page>();
            Register<Page2ViewModel, Page2Page>();
            Register<Page3ViewModel, Page3Page>();
            Register<Page4ViewModel, Page4Page>();
            Register<Page5ViewModel, Page5Page>();
        }

        public Page5ViewModel Page5ViewModel => ServiceLocator.Current.GetInstance<Page5ViewModel>();

        public Page4ViewModel Page4ViewModel => ServiceLocator.Current.GetInstance<Page4ViewModel>();

        public Page3ViewModel Page3ViewModel => ServiceLocator.Current.GetInstance<Page3ViewModel>();

        public Page2ViewModel Page2ViewModel => ServiceLocator.Current.GetInstance<Page2ViewModel>();

        public Page1ViewModel Page1ViewModel => ServiceLocator.Current.GetInstance<Page1ViewModel>();

        public FlipViewViewModel FlipViewViewModel => ServiceLocator.Current.GetInstance<FlipViewViewModel>();

        public PivotViewModel PivotViewModel => ServiceLocator.Current.GetInstance<PivotViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
