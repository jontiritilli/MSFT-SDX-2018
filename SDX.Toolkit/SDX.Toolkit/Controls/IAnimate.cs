using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SDX.Toolkit.Controls
{
    public enum AnimationDirection
    {
        Left,
        Right
    }

    public interface IAnimate
    {
        bool HasAnimateChildren();

        bool HasPageEntranceAnimation();

        AnimationDirection Direction();
        List<UIElement> AnimatableChildren();
    }
    
}
