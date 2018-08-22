using System;

using Windows.UI;
using Windows.UI.Xaml.Controls;

using SurfaceTestBed.ViewModels;
using SDX.Toolkit.Helpers;


namespace SurfaceTestBed.Views
{
    public sealed partial class Page5Page : Page
    {
        private Page5ViewModel ViewModel
        {
            get { return DataContext as Page5ViewModel; }
        }

        public Page5Page()
        {
            InitializeComponent();

            TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.YellowGreen);
        }
    }
}
