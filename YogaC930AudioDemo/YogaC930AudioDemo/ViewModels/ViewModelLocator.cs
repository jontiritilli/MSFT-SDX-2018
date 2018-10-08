using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using YogaC930AudioDemo.Services;
using YogaC930AudioDemo.Views;

namespace YogaC930AudioDemo.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            Register<AttractorLoopViewModel, AttractorLoopPage>();
            Register<SurroundSoundViewModel, SurroundSoundPage>();
            Register<HardwarePopupViewModel, HardwarePopupPage>();
            Register<ImmersivePopupViewModel, ImmersivePopupPage>();
            Register<PlayerPopupViewModel, PlayerPopupPage>();
            Register<AccessoriesViewModel, AccessoriesPage>();
            Register<SpecsViewModel, SpecsPage>();
            Register<SchemeActivationSampleViewModel, SchemeActivationSamplePage>();
        }

        public SchemeActivationSampleViewModel SchemeActivationSampleViewModel => ServiceLocator.Current.GetInstance<SchemeActivationSampleViewModel>();

        public SpecsViewModel SpecsViewModel => ServiceLocator.Current.GetInstance<SpecsViewModel>();

        public AccessoriesViewModel AccessoriesViewModel => ServiceLocator.Current.GetInstance<AccessoriesViewModel>();

        public PlayerPopupViewModel PlayerPopupViewModel => ServiceLocator.Current.GetInstance<PlayerPopupViewModel>();

        public ImmersivePopupViewModel ImmersivePopupViewModel => ServiceLocator.Current.GetInstance<ImmersivePopupViewModel>();

        public HardwarePopupViewModel HardwareViewViewModel => ServiceLocator.Current.GetInstance<HardwarePopupViewModel>();

        public SurroundSoundViewModel SurroundSoundViewModel => ServiceLocator.Current.GetInstance<SurroundSoundViewModel>();

        public AttractorLoopViewModel AttractorLoopViewModel => ServiceLocator.Current.GetInstance<AttractorLoopViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
