using System;

using GalaSoft.MvvmLight;

namespace SurfaceTestBed.ViewModels
{
    public class Page2ViewModel : ViewModelBase
    {
        public string Headline = "Surface Test Bed - Page Two";
        public string Lede = "You can add your controls to this app to test them, and to test interaction.";
        public string WelcomeText = "<Add your controls to this space.>";

        public Page2ViewModel()
        {
        }
    }
}
