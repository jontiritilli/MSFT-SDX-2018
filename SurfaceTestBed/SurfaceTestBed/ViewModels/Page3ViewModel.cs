using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using System.Collections.Generic;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Controls;

using GalaSoft.MvvmLight;

namespace SurfaceTestBed.ViewModels
{
    public class Page3ViewModel : ViewModelBase
    {
        public string Headline = "Surface Test Bed - Page Three";
        public string Lede = "You can add your controls to this app to test them, and to test interaction.";
        public string WelcomeText = "<Add your controls to this space.>";
        public string HeadlineJot = "Write some shit down";
        public string BulletJot = "Jot Down Notes, you'll never read them again";

        public string HeadlineWrite = "As they say, it's got mad lean";
        public string BulletWrite = "Now With Tilt! Lean with it, rock with it";
        public string HeadlinePressure = "Gasp at the power";
        public string BulletPressure = "Pressure sensitivity is more than 9000!!";
        public string HeadlinePalm = "FaceFalm";
        public string BulletPalm = "Put your palm right on the screen, it won't mess up your drawing";
    }
}
