﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceProDemo.Views
{
    public enum INavigateMoveDirection
    {
        Unknown,
        Backward,
        Forward
    }

    public interface INavigate
    {
        void NavigateToPage(INavigateMoveDirection moveDirection);

        void NavigateFromPage();
    }
}
