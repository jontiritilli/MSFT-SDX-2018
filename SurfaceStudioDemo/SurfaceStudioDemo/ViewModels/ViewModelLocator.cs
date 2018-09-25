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
            Register<ExperiencePixelSensePopupViewModel, ExperiencePixelSensePopupPage>();
            Register<AccessoriesMouseViewModel, AccessoriesMousePage>();
            Register<AccessoriesDialViewModel, AccessoriesDialPage>();
            Register<AccessoriesPenViewModel, AccessoriesPenPage>();
            Register<AccessoriesPenPopupViewModel, AccessoriesPenPopupPage>();
            Register<BestOfMicrosoftViewModel, BestOfMicrosoftPage>();
            Register<CompareViewModel, ComparePage>();
            Register<ComparePopupProViewModel, ComparePagePopupPro>();
            Register<ComparePopupBookViewModel, ComparePagePopupBook>();
            Register<ComparePopupStudioViewModel, ComparePagePopupStudio>();
            Register<ComparePopupLaptopViewModel, ComparePagePopupLaptop>();
            Register<ComparePopupGoViewModel, ComparePagePopupGo>();
        }

        public CompareViewModel CompareViewModel => ServiceLocator.Current.GetInstance<CompareViewModel>();

        public ComparePopupProViewModel ComparePopupProViewModel => ServiceLocator.Current.GetInstance<ComparePopupProViewModel>();

        public ComparePopupBookViewModel ComparePopupBookViewModel => ServiceLocator.Current.GetInstance<ComparePopupBookViewModel>();

        public ComparePopupStudioViewModel ComparePopupStudioViewModel => ServiceLocator.Current.GetInstance<ComparePopupStudioViewModel>();

        public ComparePopupLaptopViewModel ComparePopupLaptopViewModel => ServiceLocator.Current.GetInstance<ComparePopupLaptopViewModel>();

        public ComparePopupGoViewModel ComparePopupGoViewModel => ServiceLocator.Current.GetInstance<ComparePopupGoViewModel>();

        public BestOfMicrosoftViewModel BestOfMicrosoftViewModel => ServiceLocator.Current.GetInstance<BestOfMicrosoftViewModel>();

        public AccessoriesPenViewModel AccessoriesPenViewModel => ServiceLocator.Current.GetInstance<AccessoriesPenViewModel>();

        public AccessoriesPenPopupViewModel AccessoriesPenPopupViewModel => ServiceLocator.Current.GetInstance<AccessoriesPenPopupViewModel>();

        public AccessoriesDialViewModel AccessoriesDialViewModel => ServiceLocator.Current.GetInstance<AccessoriesDialViewModel>();

        public AccessoriesMouseViewModel AccessoriesMouseViewModel => ServiceLocator.Current.GetInstance<AccessoriesMouseViewModel>();

        public ExperiencePixelSenseViewModel ExperiencePixelSenseViewModel => ServiceLocator.Current.GetInstance<ExperiencePixelSenseViewModel>();

        public ExperiencePixelSensePopupViewModel ExperiencePixelSensePopupViewModel => ServiceLocator.Current.GetInstance<ExperiencePixelSensePopupViewModel>();

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
