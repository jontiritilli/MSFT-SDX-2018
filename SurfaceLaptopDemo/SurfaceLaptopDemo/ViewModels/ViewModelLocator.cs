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
            Register<FlipViewViewModel, FlipViewPage>();
            Register<AttractorLoopViewModel, MainPage>();
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
        }

        public CompareViewModel CompareViewModel => ServiceLocator.Current.GetInstance<CompareViewModel>();

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

        public AttractorLoopViewModel MainViewModel => ServiceLocator.Current.GetInstance<AttractorLoopViewModel>();

        public FlipViewViewModel PivotViewModel => ServiceLocator.Current.GetInstance<FlipViewViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
