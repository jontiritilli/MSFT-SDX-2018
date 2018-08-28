using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;
using SurfaceStudioDemo.Views;

namespace SurfaceStudioDemo.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            Register<FlipViewViewModel, FlipViewPage>();
            Register<AttractorLoopViewModel, AttractorLoopPage>();
            Register<ExperienceHeroViewModel, ExperienceHeroPage>();
            Register<ExperienceCreativityViewModel, ExperienceCreativityPage>();
            Register<ExperienceCraftedViewModel, ExperienceCraftedPage>();
            Register<ExperiencePixelSenseViewModel, ExperiencePixelSensePage>();
            Register<AccessoriesDialViewModel, AccessoriesDialPage>();
            Register<AccessoriesMouseViewModel, AccessoriesMousePage>();
            Register<AccessoriesTryItViewModel, AccessoriesTryItPage>();
            Register<AccessoriesPenViewModel, AccessoriesPenPage>();
            Register<BestOfMicrosoftViewModel, BestOfMicrosoftPage>();
            Register<CompareViewModel, ComparePage>();
        }

        public CompareViewModel CompareViewModel => ServiceLocator.Current.GetInstance<CompareViewModel>();

        public BestOfMicrosoftViewModel BestOfMicrosoftViewModel => ServiceLocator.Current.GetInstance<BestOfMicrosoftViewModel>();

        public AccessoriesPenViewModel AccessoriesPenViewModel => ServiceLocator.Current.GetInstance<AccessoriesPenViewModel>();

        public AccessoriesTryItViewModel AccessoriesTryItViewModel => ServiceLocator.Current.GetInstance<AccessoriesTryItViewModel>();

        public AccessoriesMouseViewModel AccessoriesMouseViewModel => ServiceLocator.Current.GetInstance<AccessoriesMouseViewModel>();

        public AccessoriesDialViewModel AccessoriesDialViewModel => ServiceLocator.Current.GetInstance<AccessoriesDialViewModel>();

        public ExperiencePixelSenseViewModel ExperiencePixelSenseViewModel => ServiceLocator.Current.GetInstance<ExperiencePixelSenseViewModel>();

        public ExperienceCraftedViewModel ExperienceCraftedViewModel => ServiceLocator.Current.GetInstance<ExperienceCraftedViewModel>();

        public ExperienceCreativityViewModel ExperienceCreativityViewModel => ServiceLocator.Current.GetInstance<ExperienceCreativityViewModel>();

        public ExperienceHeroViewModel ExperienceHeroViewModel => ServiceLocator.Current.GetInstance<ExperienceHeroViewModel>();

        public AttractorLoopViewModel AttractorLoopViewModel => ServiceLocator.Current.GetInstance<AttractorLoopViewModel>();

        public FlipViewViewModel FlipViewViewModel => ServiceLocator.Current.GetInstance<FlipViewViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
