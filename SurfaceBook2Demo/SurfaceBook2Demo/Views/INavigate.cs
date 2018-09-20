using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SurfaceBook2Demo.Views
{
    public enum INavigateMoveDirection
    {
        Unknown,
        Backward,
        Forward
    }

    interface INavigate
    {
        void NavigateToPage(INavigateMoveDirection moveDirection);

        void NavigateFromPage();
    }
}
