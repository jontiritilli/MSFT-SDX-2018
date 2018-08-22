using System;

using Windows.UI;
using Windows.UI.Xaml.Controls;

using SurfaceTestBed.ViewModels;
using SDX.Toolkit.Helpers;


namespace SurfaceTestBed.Views
{
    public sealed partial class Page1Page : Page
    {
        private Page1ViewModel ViewModel
        {
            get { return DataContext as Page1ViewModel; }
        }

        public Page1Page()
        {
            InitializeComponent();

            //TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.AliceBlue);
        }

    }
}
