using System;

using Windows.Storage;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using YogaC930AudioDemo.Services;


namespace YogaC930AudioDemo.ViewModels
{
    public class SpeedsAndFeedsViewModel : ViewModelBase
    {
        #region Public Properties

        public string Headline;
        public string Body;
        public string Footnote;
        public string TopColor;
        public string BottomColor;

        public string WindowsBodyFirst;
        public string WindowsBodyBold;
        public string WindowsBodyLast;

        public string IntelBodyFirst;
        public string IntelBodyBold;
        public string IntelBodyLast;

        public string DolbyBodyFirst;
        public string DolbyBodyBold;
        public string DolbyBodyLast;

        public string FourKBodyFirst;
        public string FourKBodyBold;
        public string FourKBodyLast;

        public string HoursBodyFirst;
        public string HoursBodyBold;
        public string HoursBodyLast;

        public string FiftySevenBodyFirst;
        public string FiftySevenBodyBold;
        public string FiftySevenBodyLast;


        #endregion


        #region Construction

        public SpeedsAndFeedsViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadSpeedsAndFeedsViewModel(this);
            }
        }

        #endregion
    }
}
