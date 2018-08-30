using System;

using Windows.UI;
using Windows.UI.Xaml.Controls;

using SurfaceTestBed.ViewModels;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceTestBed.Views
{
    public sealed partial class Page3Page : Page
    {
        private Page3ViewModel ViewModel
        {
            get { return DataContext as Page3ViewModel; }
        }

        ListItem[] items = new ListItem[4];

        public Page3Page()
        {
            InitializeComponent();

            TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.Orange);


            items[0] = ListItem.CreateListItem(0, ListItemIcon.Jot, 50d, ViewModel.HeadlineJot, ViewModel.BulletJot, null);

            items[1] = ListItem.CreateListItem(1, ListItemIcon.Write, 50d, ViewModel.HeadlineWrite, ViewModel.BulletWrite, null);

            items[2] = ListItem.CreateListItem(2, ListItemIcon.Pressure, 50d, ViewModel.HeadlinePressure, ViewModel.BulletPressure, null);

            items[3] = ListItem.CreateListItem(3, ListItemIcon.Palm, 50d, ViewModel.HeadlinePalm, ViewModel.BulletPalm, null);
        }
    }
}
