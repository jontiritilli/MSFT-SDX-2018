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
            Register<AudioViewModel, AudioPage>();
            Register<SpeakerDesignPopupViewModel, SpeakerDesignPopupPage>();
            Register<HingeDesignPopupViewModel, HingeDesignPopupPage>();
            Register<PlayerPopupViewModel, PlayerPopupPage>();
            Register<FeaturesViewModel, FeaturesPage>();
            Register<SpeedsAndFeedsViewModel, SpeedsAndFeedsPage>();
            Register<SchemeActivationSampleViewModel, SchemeActivationSamplePage>();
        }

        public SchemeActivationSampleViewModel SchemeActivationSampleViewModel => ServiceLocator.Current.GetInstance<SchemeActivationSampleViewModel>();

        public SpeedsAndFeedsViewModel SpecsViewModel => ServiceLocator.Current.GetInstance<SpeedsAndFeedsViewModel>();

        public FeaturesViewModel AccessoriesViewModel => ServiceLocator.Current.GetInstance<FeaturesViewModel>();

        public PlayerPopupViewModel PlayerPopupViewModel => ServiceLocator.Current.GetInstance<PlayerPopupViewModel>();

        public HingeDesignPopupViewModel ImmersivePopupViewModel => ServiceLocator.Current.GetInstance<HingeDesignPopupViewModel>();

        public SpeakerDesignPopupViewModel HardwareViewViewModel => ServiceLocator.Current.GetInstance<SpeakerDesignPopupViewModel>();

        public AudioViewModel SurroundSoundViewModel => ServiceLocator.Current.GetInstance<AudioViewModel>();

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
