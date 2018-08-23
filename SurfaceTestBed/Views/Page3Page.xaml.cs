using System;

using Windows.UI;
using Windows.UI.Xaml.Controls;

using SurfaceTestBed.ViewModels;
using SDX.Toolkit.Helpers;


namespace SurfaceTestBed.Views
{
    public sealed partial class Page3Page : Page
    {
        private Page3ViewModel ViewModel
        {
            get { return DataContext as Page3ViewModel; }
        }

        public Page3Page()
        {
            InitializeComponent();

            TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.Orange);
        }
    }
}
