using System;

using GalaSoft.MvvmLight;

namespace SurfaceTestBed.ViewModels
{
    public class Page1ViewModel : ViewModelBase
    {

        public string Headline = "Surface Test Bed - Page One";
        public string Lede = "You can add your controls to this app to test them, and to test interaction.";
        public string HeroPhrase = "I Need A Hero!";
        public int HeroWordCount = 1;

        public Page1ViewModel()
        {
        }
    }
}
