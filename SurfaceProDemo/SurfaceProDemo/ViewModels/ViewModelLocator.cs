using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;
using SurfaceProDemo.Views;

namespace SurfaceProDemo.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<FlipViewViewModel>();
            Register<AttractorLoopViewModel, AttractorLoopPage>();
            Register<ChoosePathViewModel, ChoosePathPage>();
            Register<ExperienceHeroViewModel, ExperienceHeroPage>();
            Register<ExperienceIntroViewModel, ExperienceIntroPage>();
            Register<ExperienceTransformViewModel, ExperienceTransformPage>();
            Register<ExperiencePerformanceViewModel, ExperiencePerformancePage>();
            Register<ExperienceQuietViewModel, ExperienceQuietPage>();
            Register<AccessoriesPenViewModel, AccessoriesPenPage>();
            Register<AccessoriesKeyboardViewModel, AccessoriesKeyboardPage>();
            Register<AccessoriesMouseViewModel, AccessoriesMousePage>();
            Register<BestOfMicrosoftViewModel, BestOfMicrosoftPage>();
            Register<CompareViewModel, ComparePage>();
            Register<UriSchemeExampleViewModel, UriSchemeExamplePage>();
        }

        public UriSchemeExampleViewModel UriSchemeExampleViewModel => ServiceLocator.Current.GetInstance<UriSchemeExampleViewModel>();

        public CompareViewModel CompareViewModel => ServiceLocator.Current.GetInstance<CompareViewModel>();

        public BestOfMicrosoftViewModel BestOfMicrosoftViewModel => ServiceLocator.Current.GetInstance<BestOfMicrosoftViewModel>();

        public AccessoriesMouseViewModel AccessoriesMouseViewModel => ServiceLocator.Current.GetInstance<AccessoriesMouseViewModel>();

        public AccessoriesKeyboardViewModel AccessoriesKeyboardViewModel => ServiceLocator.Current.GetInstance<AccessoriesKeyboardViewModel>();

        public AccessoriesPenViewModel AccessoriesPenViewModel => ServiceLocator.Current.GetInstance<AccessoriesPenViewModel>();

        public ExperienceQuietViewModel ExperienceQuietViewModel => ServiceLocator.Current.GetInstance<ExperienceQuietViewModel>();

        public ExperiencePerformanceViewModel ExperiencePerformanceViewModel => ServiceLocator.Current.GetInstance<ExperiencePerformanceViewModel>();

        public ExperienceTransformViewModel ExperienceTransformViewModel => ServiceLocator.Current.GetInstance<ExperienceTransformViewModel>();

        public ExperienceIntroViewModel ExperienceIntroViewModel => ServiceLocator.Current.GetInstance<ExperienceIntroViewModel>();

        public ExperienceHeroViewModel ExperienceHeroViewModel => ServiceLocator.Current.GetInstance<ExperienceHeroViewModel>();

        public ChoosePathViewModel ChoosePathViewModel => ServiceLocator.Current.GetInstance<ChoosePathViewModel>();

        public AttractorLoopViewModel AttractorLoopViewModel => ServiceLocator.Current.GetInstance<AttractorLoopViewModel>();

        public FlipViewViewModel ShellViewModel => ServiceLocator.Current.GetInstance<FlipViewViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
