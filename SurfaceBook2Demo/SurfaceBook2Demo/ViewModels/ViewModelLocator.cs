using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;
using SurfaceBook2Demo.Views;


namespace SurfaceBook2Demo.ViewModels
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
            Register<ExperienceIntroViewModel, ExperienceIntroPage>();
            Register<ExperienceDayViewModel, ExperienceDayPage>();
            Register<ExperienceDayWorkViewModel, ExperienceDayWorkPage>();
            Register<ExperienceDayWorkPopupViewModel, ExperienceDayWorkPopupPage>();
            Register<ExperienceDayCreateViewModel, ExperienceDayCreatePage>();
            Register<ExperienceDayRelaxViewModel, ExperienceDayRelaxPage>();
            Register<ExperienceDayPlayViewModel, ExperienceDayPlayPage>();
            Register<AccessoriesPenViewModel, AccessoriesPenPage>();
            Register<AccessoriesDialViewModel, AccessoriesDialPage>();
            Register<AccessoriesMouseViewModel, AccessoriesMousePage>();
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

        public AccessoriesMouseViewModel AccessoriesMouseViewModel => ServiceLocator.Current.GetInstance<AccessoriesMouseViewModel>();

        public AccessoriesDialViewModel AccessoriesDialViewModel => ServiceLocator.Current.GetInstance<AccessoriesDialViewModel>();

        public AccessoriesPenViewModel AccessoriesPenViewModel => ServiceLocator.Current.GetInstance<AccessoriesPenViewModel>();

        public ExperienceDayPlayViewModel ExperienceDayPlayViewModel => ServiceLocator.Current.GetInstance<ExperienceDayPlayViewModel>();

        public ExperienceDayRelaxViewModel ExperienceDayRelaxViewModel => ServiceLocator.Current.GetInstance<ExperienceDayRelaxViewModel>();

        public ExperienceDayCreateViewModel ExperienceDayCreateViewModel => ServiceLocator.Current.GetInstance<ExperienceDayCreateViewModel>();

        public ExperienceDayWorkViewModel ExperienceDayWorkViewModel => ServiceLocator.Current.GetInstance<ExperienceDayWorkViewModel>();

        public ExperienceDayWorkPopupViewModel ExperienceDayWorkPopupViewModel => ServiceLocator.Current.GetInstance<ExperienceDayWorkPopupViewModel>();

        public ExperienceDayViewModel ExperienceDayViewModel => ServiceLocator.Current.GetInstance<ExperienceDayViewModel>();

        public ExperienceIntroViewModel ExperienceIntroViewModel => ServiceLocator.Current.GetInstance<ExperienceIntroViewModel>();

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
