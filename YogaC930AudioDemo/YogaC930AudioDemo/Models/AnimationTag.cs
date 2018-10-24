using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;

using YogaC930AudioDemo.Helpers;


namespace YogaC930AudioDemo.Models
{
    public class AnimationTag : DependencyObject
    {
        public TranslateAxes TranslateAxis = TranslateAxes.Horizontal;
        public AnimationTypes AnimationType = AnimationTypes.Standard;

        // AnimationType
        public static readonly DependencyProperty AnimationTypeProperty =
                                            DependencyProperty.RegisterAttached("AnimationType",
                                            typeof(AnimationTypes), typeof(AnimationTag),
                                            new PropertyMetadata(AnimationTypes.None));

        public static void SetAnimationType(UIElement element, AnimationTypes value)
        {
            element.SetValue(AnimationTypeProperty, value);
        }

        public static AnimationTypes GetAnimationType(UIElement element)
        {
            return (AnimationTypes)element.GetValue(AnimationTypeProperty);
        }

        // TranslateAxis
        public static readonly DependencyProperty TranslateAxisProperty =
                                                    DependencyProperty.RegisterAttached("TranslateAxis",
                                                    typeof(TranslateAxes), typeof(AnimationTag),
                                                    new PropertyMetadata(TranslateAxes.Horizontal));

        public static void SetTranslateAxis(UIElement element, TranslateAxes value)
        {
            element.SetValue(TranslateAxisProperty, value);
        }

        public static TranslateAxes GetTranslateAxis(UIElement element)
        {
            return (TranslateAxes)element.GetValue(TranslateAxisProperty);
        }

        // TranslateDirection
        public static readonly DependencyProperty TranslateDirectionProperty =
                                                    DependencyProperty.RegisterAttached("TranslateDirection",
                                                    typeof(TranslateDirections), typeof(AnimationTag),
                                                    new PropertyMetadata(TranslateDirections.FromLeft));

        public static void SetTranslateDirection(UIElement element, TranslateDirections value)
        {
            element.SetValue(TranslateDirectionProperty, value);
        }

        public static TranslateDirections GetTranslateDirection(UIElement element)
        {
            return (TranslateDirections)element.GetValue(TranslateDirectionProperty);
        }


    }
}
