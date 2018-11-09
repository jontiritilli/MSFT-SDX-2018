using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using SurfaceHeadphoneDemo.Services;
using SurfaceHeadphoneDemo.Views;


namespace SurfaceHeadphoneDemo.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            Register<AttractorLoopViewModel, AttractorLoopPage>();
            Register<FirmwareUpdateViewModel, FirmwareUpdatePage>();
            Register<FlipViewViewModel, FlipViewPage>();
            Register<ChoosePathViewModel, ChoosePathPage>();
            Register<AudioTryItViewModel, AudioTryItPage>();
            Register<AudioListenViewModel, AudioListenPage>();
            Register<AudioListenPopupViewModel, AudioListenPopupPage>();
            Register<DesignViewModel, DesignPage>();
            Register<TechViewModel, TechPage>();
            Register<ProductivityViewModel, ProductivityPage>();
            Register<SpecsViewModel, SpecsPage>();
            Register<InTheBoxViewModel, InTheBoxPage>();
            Register<HowToViewModel, HowToPage>();
        }

        public InTheBoxViewModel InTheBoxViewModel => ServiceLocator.Current.GetInstance<InTheBoxViewModel>();

        public HowToViewModel HowToViewModel => ServiceLocator.Current.GetInstance<HowToViewModel>();

        public SpecsViewModel SpecsViewModel => ServiceLocator.Current.GetInstance<SpecsViewModel>();

        public ProductivityViewModel ProductivityViewModel => ServiceLocator.Current.GetInstance<ProductivityViewModel>();

        public TechViewModel TechViewModel => ServiceLocator.Current.GetInstance<TechViewModel>();

        public DesignViewModel DesignViewModel => ServiceLocator.Current.GetInstance<DesignViewModel>();

        public AudioListenViewModel AudioListenViewModel => ServiceLocator.Current.GetInstance<AudioListenViewModel>();

        public AudioListenPopupViewModel AudioListenPopupViewModel => ServiceLocator.Current.GetInstance<AudioListenPopupViewModel>();

        public AudioTryItViewModel AudioTryItViewModel => ServiceLocator.Current.GetInstance<AudioTryItViewModel>();

        public ChoosePathViewModel ChoosePathViewModel => ServiceLocator.Current.GetInstance<ChoosePathViewModel>();

        public AttractorLoopViewModel AttractorLoopViewModel => ServiceLocator.Current.GetInstance<AttractorLoopViewModel>();

        public FlipViewViewModel FlipViewViewModel => ServiceLocator.Current.GetInstance<FlipViewViewModel>();

        public FirmwareUpdateViewModel FirmwareUpdateViewModel => ServiceLocator.Current.GetInstance<FirmwareUpdateViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
