using System;

using Windows.UI;
using Windows.UI.Xaml.Controls;

using SurfaceTestBed.ViewModels;
using SDX.Toolkit.Helpers;


namespace SurfaceTestBed.Views
{
    public sealed partial class Page4Page : Page
    {
        private Page4ViewModel ViewModel
        {
            get { return DataContext as Page4ViewModel; }
        }

        public Page4Page()
        {
            InitializeComponent();

            TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.DarkSeaGreen);
        }
    }
}
