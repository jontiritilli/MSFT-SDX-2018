using System;

using Windows.UI;
using Windows.UI.Xaml.Controls;

using SurfaceTestBed.ViewModels;
using SDX.Toolkit.Helpers;


namespace SurfaceTestBed.Views
{
    public sealed partial class Page2Page : Page
    {
        private Page2ViewModel ViewModel
        {
            get { return DataContext as Page2ViewModel; }
        }

        public Page2Page()
        {
            InitializeComponent();

            TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.Purple);
        }
    }
}
