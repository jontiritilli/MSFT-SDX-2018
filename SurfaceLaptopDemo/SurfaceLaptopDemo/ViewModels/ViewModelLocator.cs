using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SurfaceLaptopDemo.Views;


namespace SurfaceLaptopDemo.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());

            Register<AttractorLoopViewModel, AttractorLoopPage>();
            Register<FlipViewViewModel, FlipViewPage>();
            Register<ExperienceHeroViewModel, ExperienceHeroPage>();
            Register<ExperienceColorsViewModel, ExperienceColorsPage>();
            Register<ExperienceInnovationViewModel, ExperienceInnovationPage>();
            Register<ExperiencePerformanceViewModel, ExperiencePerformancePage>();
            Register<ExperienceSpeakersViewModel, ExperienceSpeakersPage>();
            Register<ExperienceSleekViewModel, ExperienceSleekPage>();
            Register<AccessoriesTouchViewModel, AccessoriesTouchPage>();
            Register<AccessoriesMouseViewModel, AccessoriesMousePage>();
            Register<AccessoriesPenViewModel, AccessoriesPenPage>();
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

        public AccessoriesMouseViewModel AccessoriesMouseViewModel => ServiceLocator.Current.GetInstance<AccessoriesMouseViewModel>();

        public AccessoriesTouchViewModel AccessoriesTouchViewModel => ServiceLocator.Current.GetInstance<AccessoriesTouchViewModel>();

        public ExperienceSleekViewModel ExperienceSleekViewModel => ServiceLocator.Current.GetInstance<ExperienceSleekViewModel>();

        public ExperienceSpeakersViewModel ExperienceSpeakersViewModel => ServiceLocator.Current.GetInstance<ExperienceSpeakersViewModel>();

        public ExperiencePerformanceViewModel ExperiencePerformanceViewModel => ServiceLocator.Current.GetInstance<ExperiencePerformanceViewModel>();

        public ExperienceInnovationViewModel ExperienceInnovationViewModel => ServiceLocator.Current.GetInstance<ExperienceInnovationViewModel>();

        public ExperienceColorsViewModel ExperienceColorsViewModel => ServiceLocator.Current.GetInstance<ExperienceColorsViewModel>();

        public ExperienceHeroViewModel ExperienceHeroViewModel => ServiceLocator.Current.GetInstance<ExperienceHeroViewModel>();

        public FlipViewViewModel FlipViewViewModel => ServiceLocator.Current.GetInstance<FlipViewViewModel>();

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
