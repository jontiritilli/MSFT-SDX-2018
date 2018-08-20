using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SDX.Toolkit.Helpers
{
    public static class TestHelper
    {
        public static void AddGridCellBorders(Grid grid, int rowCount, int colCount, Color color)
        {
            // may not be loaded when we get called
            //int rowCount = grid.RowDefinitions.Count;
            //int colCount = grid.ColumnDefinitions.Count;

            // test
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    Border border = new Border() { BorderBrush = new SolidColorBrush(color), BorderThickness = new Thickness(1) };
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                    grid.Children.Add(border);
                }
            }
        }
    }
}
