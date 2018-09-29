using SDX.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;


namespace SDX.Toolkit.Helpers
{
    public enum TranslateDirection
    {
        Left,
        Right
    }

    public static class AnimationHelper
    {
        public static void PerformFadeIn(DependencyObject dependencyObject, double duration, double staggerDelay = 0)
        {
            Storyboard storyboard = null;

            if (null != dependencyObject)
            {
                storyboard = CreateEasingAnimation(dependencyObject, "Opacity", 0.0, 0.0, 1.0, duration, staggerDelay, false, false, new RepeatBehavior(1d));

                storyboard.Begin();
            }
        }

        public static void PerformFadeOut(DependencyObject dependencyObject, double duration, double staggerDelay = 0)
        {
            Storyboard storyboard = null;

            if (null != dependencyObject)
            {
                storyboard = CreateEasingAnimation(dependencyObject, "Opacity", 1.0, 1.0, 0.0, duration, staggerDelay, false, false, new RepeatBehavior(1d));

                storyboard.Begin();
            }
        }

        public static void PerformTranslateIn(DependencyObject dependencyObject, TranslateDirection translateDirection, double distance, double duration, double staggerDelay = 0)
        {
            Storyboard storyboard = null;

            double xStartingPosition = distance;
            if (translateDirection == TranslateDirection.Left)
            {
                xStartingPosition = distance;
            }
            else
            {
                xStartingPosition = distance * -1;
            }

            if (null != dependencyObject)
            {
                storyboard = CreateTranslateAnimation(dependencyObject, "X", 0.0, xStartingPosition, 0.0, duration, staggerDelay, false, false, new RepeatBehavior(1d));

                storyboard.Begin();
            }
        }

        // keep incase we dont need to traverse the visual tree recursively (itll output last first so be aware)
        //static private IEnumerable<DependencyObject> FindInputElements(DependencyObject parent)
        //{
        //    if (parent == null)
        //        yield break;

        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        //    {
        //        DependencyObject o = VisualTreeHelper.GetChild(parent, i);

        //        foreach (DependencyObject obj in FindInputElements(o))

        //            yield return (UIElement)obj;

        //    }

        //    yield return parent;
        //}

        public static void PerformPageEntranceAnimation(Page page)
        {
            //traverse the visual tree of a page and perform the fade in and translate in on each frameworkitem            
            // general idea from EntranceThemeTransition and rebuilt as a behavior here
            // https://jeremiahmorrill.wordpress.com/2014/04/02/entrancethemetransitionbehavior-behavior-for-wpf/
            List<Storyboard> StoryBoardCollection = new List<Storyboard>();
            List<UIElement> AnimatableChildren = new List<UIElement>();
            double StaggerDelay = 0.0;
            double TotalStagger = (((Windows.UI.Xaml.Controls.Panel)page.Content).Children.Count * 100d) + 500d;
            IAnimate animateChild;
            double distanceToTranslate;
            // Traverses the first content area on the page in linear order to show everything
            foreach (UIElement child in ((Windows.UI.Xaml.Controls.Panel)page.Content).Children)
            {
                             
                if (null != child && child != page && !(child is Grid) && child is IAnimate)// dont do the page either
                {
                    animateChild = (IAnimate)child;
                    ParseAnimatableChildren(child, ref AnimatableChildren);
                }
            }
            TotalStagger = AnimatableChildren.Count * 100d + 500d;
                foreach (UIElement OrderedChild in AnimatableChildren)
                {
                    Storyboard storyboard = null;
                    IAnimate AnimatedOrderedChild = (IAnimate)OrderedChild;
                    distanceToTranslate = 100 * (AnimatedOrderedChild.Direction() != AnimationDirection.Left ? 1 : -1);
                    storyboard = CreateEasingAnimation(OrderedChild, "Opacity", 0.0, 0.0, 1.0, TotalStagger, StaggerDelay, false, false, new RepeatBehavior(1d));
                    StoryBoardCollection.Add(storyboard);
                    if (AnimatedOrderedChild.HasPageEntranceTranslation())
                    {
                        storyboard = CreateTranslateAnimation(OrderedChild, "X", distanceToTranslate, distanceToTranslate, 0.0, TotalStagger, StaggerDelay, false, false, new RepeatBehavior(1d));
                        StoryBoardCollection.Add(storyboard);
                    }

                    StaggerDelay += 100;
                }
            

            foreach (Storyboard SB in StoryBoardCollection)
            {
                SB.Begin();
            }
        }
        public static void ParseAnimatableChildren(UIElement child,ref List<UIElement> AnimatableChildren)
        {
            if (child is IAnimate)
            {
                IAnimate AnimateChild = (IAnimate)child;
                if (AnimateChild.HasPageEntranceAnimation())// dont do the page either
                {
                    if (AnimateChild.HasAnimateChildren())
                    {
                        List<UIElement> AnimatableGrandChildren = AnimateChild.AnimatableChildren();
                        foreach (UIElement grandChild in AnimatableGrandChildren)
                        {
                            if (null != grandChild)// headers can have null ledes so check pls
                            {
                                ParseAnimatableChildren(grandChild, ref AnimatableChildren);
                            }
                        }
                    }
                    else
                    {
                        AnimatableChildren.Add(child);
                    }
                }
            }
            else
            {
                // canvas and other stuff that can contain children
            }
            
        }

        public static void PerformPageExitAnimation(Page page)
        {
            IAnimate animateChild;
            List<UIElement> AnimatableChildren = new List<UIElement>();
            FrameworkElement FE;
            foreach (UIElement child in ((Windows.UI.Xaml.Controls.Panel)page.Content).Children)
            {
                if (null != child && child != page && !(child is Grid) && child is IAnimate)
                {
                    animateChild = (IAnimate)child;
                    ParseAnimatableChildren(child, ref AnimatableChildren);
                }
            }

            foreach (UIElement HidableChild in AnimatableChildren)
            {
                FE = (FrameworkElement)HidableChild;
                FE.Opacity = 0;
            }

        }

        public static bool IsVisible(UIElement uiElement)
        {
            bool isVisible = false;

            if (null != uiElement)
            {
                isVisible = (uiElement.Opacity > 0.0);
            }

            return isVisible;
        }

        public static Storyboard CreateStandardAnimation(DependencyObject dependencyObject, string propertyName,
                                                            double defaultValue, double startValue, double endValue,
                                                            double duration, double staggerDelay,
                                                            bool autoReverse, bool repeatForever, RepeatBehavior repeatBehavior)
        {
            // total duration
            double totalDuration = duration + staggerDelay;

            // create the storyboard
            Storyboard storyboard = new Storyboard()
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                AutoReverse = autoReverse,
                RepeatBehavior = (repeatForever) ? RepeatBehavior.Forever : repeatBehavior
            };

            // add frame collection to the storyboard
            storyboard.Children.Add(
                CreateStandardKeyFrames(dependencyObject, propertyName, defaultValue, startValue, endValue, duration, staggerDelay));

            // set the target of the storyboard
            Storyboard.SetTarget(storyboard, dependencyObject);
            Storyboard.SetTargetProperty(storyboard, propertyName);

            return storyboard;
        }

        public static Storyboard CreateEasingAnimation(DependencyObject dependencyObject, string propertyName,
                                                    double defaultValue, double startValue, double endValue,
                                                    EasingFunctionBase easingFunctionIn, EasingFunctionBase easingFunctionOut,
                                                    double duration, double staggerDelay,
                                                    bool autoReverse, bool repeatForever, RepeatBehavior repeatBehavior)
        {
            // total duration
            double totalDuration = duration + staggerDelay;

            // create the storyboard
            Storyboard storyboard = new Storyboard()
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                AutoReverse = autoReverse,
                RepeatBehavior = (repeatForever) ? RepeatBehavior.Forever : repeatBehavior
            };

            // add frame collection to the storyboard
            storyboard.Children.Add(
                CreateEasingKeyFrames(dependencyObject, propertyName, defaultValue, startValue, endValue, easingFunctionIn, easingFunctionOut, duration, staggerDelay));

            // set the target of the storyboard
            Storyboard.SetTarget(storyboard, dependencyObject);
            Storyboard.SetTargetProperty(storyboard, propertyName);

            return storyboard;
        }

        public static Storyboard CreateEasingAnimation(DependencyObject dependencyObject, string propertyName,
                                                            double defaultValue, double startValue, double endValue,
                                                            double duration, double staggerDelay,
                                                            bool autoReverse, bool repeatForever, RepeatBehavior repeatBehavior)
        {
            // total duration
            double totalDuration = duration + staggerDelay;

            // create the storyboard
            Storyboard storyboard = new Storyboard()
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                AutoReverse = autoReverse,
                RepeatBehavior = (repeatForever) ? RepeatBehavior.Forever : repeatBehavior
            };

            // create default easing
            CubicEase easeIn = new CubicEase()
            {
                EasingMode = EasingMode.EaseIn
            };

            CubicEase easeOut = new CubicEase()
            {
                EasingMode = EasingMode.EaseOut
            };

            // add frame collection to the storyboard
            storyboard.Children.Add(
                CreateEasingKeyFrames(dependencyObject, propertyName, defaultValue, startValue, endValue, easeIn, easeOut, duration, staggerDelay));

            // set the target of the storyboard
            Storyboard.SetTarget(storyboard, dependencyObject);
            Storyboard.SetTargetProperty(storyboard, propertyName);

            return storyboard;
        }

        public static Storyboard CreateEasingAnimationWithNotify(DependencyObject dependencyObject, EventHandler<object> handler,
                                                            string propertyName, double defaultValue,
                                                            double startValue, double endValue,
                                                            EasingFunctionBase easingFunctionIn, EasingFunctionBase easingFunctionOut,
                                                            double duration, double staggerDelay,
                                                            bool autoReverse, bool repeatForever, RepeatBehavior repeatBehavior)
        {
            Storyboard storyboard = AnimationHelper.CreateEasingAnimation(dependencyObject, propertyName, defaultValue, startValue, endValue, easingFunctionIn, easingFunctionOut, duration, staggerDelay, autoReverse, repeatForever, repeatBehavior);

            if (null != handler)
            {
                storyboard.Completed += handler;
            }

            return storyboard;
        }

        public static Storyboard CreateInOutAnimation(DependencyObject dependencyObject, string propertyName,
                                                            double defaultValue, double startValue, double endValue,
                                                            double durationIn, double durationOut,
                                                            double staggerDelayIn, double restEnd, double staggerDelayOut,
                                                            bool autoReverse, bool repeatForever, RepeatBehavior repeatBehavior)
        {
            // total duration
            double totalDuration = staggerDelayIn + durationIn + restEnd + staggerDelayOut + durationOut;

            // create the storyboard
            Storyboard storyboard = new Storyboard()
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                AutoReverse = autoReverse,
                RepeatBehavior = (repeatForever) ? RepeatBehavior.Forever : repeatBehavior
            };

            // add frame collection to the storyboard
            storyboard.Children.Add(
                CreateInOutKeyFrames(dependencyObject, propertyName, defaultValue, startValue, endValue, durationIn, durationOut, staggerDelayIn, restEnd, staggerDelayOut));

            // set the target of the storyboard
            Storyboard.SetTarget(storyboard, dependencyObject);
            Storyboard.SetTargetProperty(storyboard, propertyName);

            return storyboard;
        }

        public static Storyboard CreateTranslateAnimation(DependencyObject dependencyObject, string propertyName,
                                                            double defaultValue, double startValue, double endValue,
                                                            double duration, double staggerDelay,
                                                            bool autoReverse, bool repeatForever, RepeatBehavior repeatBehavior)
        {
            // total duration
            double totalDuration = duration + staggerDelay;

            // create the storyboard
            Storyboard storyboard = new Storyboard()
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                AutoReverse = autoReverse,
                RepeatBehavior = (repeatForever) ? RepeatBehavior.Forever : repeatBehavior
            };

            var tg = new TransformGroup();
            var translation = new TranslateTransform()
            {
                X = startValue,
                Y = 0
            };

            tg.Children.Add(translation);

            UIElement UI = (UIElement)dependencyObject;
            UI.RenderTransform = tg;
            // create default easing
            CubicEase easeIn = new CubicEase()
            {
                EasingMode = EasingMode.EaseIn
            };

            CubicEase easeOut = new CubicEase()
            {
                EasingMode = EasingMode.EaseOut
            };

            DoubleAnimationUsingKeyFrames daKeyFrames = CreateEasingKeyFrames(translation, propertyName, defaultValue, startValue, endValue, easeIn, easeOut, duration, staggerDelay);
            storyboard.Children.Add(daKeyFrames);
            Storyboard.SetTarget(daKeyFrames, translation);
            Storyboard.SetTargetProperty(daKeyFrames, propertyName);

            return storyboard;
        }

        public static DoubleAnimationUsingKeyFrames CreateEasingKeyFrames(DependencyObject dependencyObject, string propertyName,
                                                        double defaultValue, double startValue, double endValue,
                                                        EasingFunctionBase easingFunctionIn, EasingFunctionBase easingFunctionOut,
                                                        double duration, double staggerDelay)
        {
            // total duration
            double totalDuration = duration + staggerDelay;

            // create the key frames holder
            DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                EnableDependentAnimation = true
            };

            // set the target
            Storyboard.SetTarget(frames, dependencyObject);
            Storyboard.SetTargetProperty(frames, propertyName);

            // easing
            if (null == easingFunctionIn)
            {
                easingFunctionIn = new CubicEase() { EasingMode = EasingMode.EaseIn };
            }
            if (null == easingFunctionOut)
            {
                easingFunctionOut = new CubicEase() { EasingMode = EasingMode.EaseOut };
            }

            // create frame 0; if we have a stagger delay, then create this frame with the default value
            EasingDoubleKeyFrame _frame0 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0d)),
                Value = (staggerDelay > 0) ? defaultValue : startValue,
                EasingFunction = easingFunctionIn
            };
            frames.KeyFrames.Add(_frame0);

            // if we have a stagger delay
            if (staggerDelay > 0)
            {
                // create stagger frame
                EasingDoubleKeyFrame _frameStagger = new EasingDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(staggerDelay)),
                    Value = startValue,
                    EasingFunction = easingFunctionIn
                };
                frames.KeyFrames.Add(_frameStagger);
            }

            // create frame 1
            EasingDoubleKeyFrame _frame1 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(totalDuration)),
                Value = endValue,
                EasingFunction = easingFunctionOut
            };
            frames.KeyFrames.Add(_frame1);

            // set the target
            Storyboard.SetTarget(frames, dependencyObject);
            Storyboard.SetTargetProperty(frames, propertyName);

            return frames;
        }

        public static DoubleAnimationUsingKeyFrames CreateStandardKeyFrames(DependencyObject dependencyObject, string propertyName,
                                                double defaultValue, double startValue, double endValue,
                                                double duration, double staggerDelay)
        {
            // total duration
            double totalDuration = duration + staggerDelay;

            // create the key frames holder
            DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames()
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                EnableDependentAnimation = true
            };

            // set the target
            Storyboard.SetTarget(frames, dependencyObject);
            Storyboard.SetTargetProperty(frames, propertyName);

            // create frame 0; if we have a stagger delay, then create this frame with the default value
            LinearDoubleKeyFrame _frame0 = new LinearDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0d)),
                Value = (staggerDelay > 0) ? defaultValue : startValue
            };
            frames.KeyFrames.Add(_frame0);

            // if we have a stagger delay
            if (staggerDelay > 0)
            {
                // create stagger frame
                LinearDoubleKeyFrame _frameStagger = new LinearDoubleKeyFrame()
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(staggerDelay)),
                    Value = startValue
                };
                frames.KeyFrames.Add(_frameStagger);
            }

            // create frame 1
            LinearDoubleKeyFrame _frame1 = new LinearDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(totalDuration)),
                Value = endValue
            };
            frames.KeyFrames.Add(_frame1);

            // set the target
            Storyboard.SetTarget(frames, dependencyObject);
            Storyboard.SetTargetProperty(frames, propertyName);

            return frames;
        }

        public static DoubleAnimationUsingKeyFrames CreateInOutKeyFrames(DependencyObject dependencyObject, string propertyName,
                                                double defaultValue, double startValue, double endValue,
                                                double durationIn, double durationOut,
                                                double staggerDelayIn, double restEnd, double staggerDelayOut)
        {
            // total duration
            double totalDuration = staggerDelayIn + durationIn + restEnd + staggerDelayOut + durationOut;

            // create the key frames holder
            DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                EnableDependentAnimation = true
            };

            // set the target
            Storyboard.SetTarget(frames, dependencyObject);
            Storyboard.SetTargetProperty(frames, propertyName);

            // need cubic easing
            CubicEase cubicEaseIn = new CubicEase()
            {
                EasingMode = EasingMode.EaseIn
            };
            CubicEase cubicEaseOut = new CubicEase()
            {
                EasingMode = EasingMode.EaseOut
            };

            // track the timeline time (milliseconds)
            double frameTime = 0;

            // create frame 0; if we have a stagger delay, then create this frame with the default value
            EasingDoubleKeyFrame _frameStart = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                Value = (staggerDelayIn > 0) ? defaultValue : startValue,
                EasingFunction = cubicEaseIn
            };
            frames.KeyFrames.Add(_frameStart);

            frameTime += staggerDelayIn;

            // if we have a stagger delay
            if (staggerDelayIn > 0)
            {
                // create stagger frame
                EasingDoubleKeyFrame _frameStaggerIn = new EasingDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                    Value = startValue,
                    EasingFunction = cubicEaseIn
                };
                frames.KeyFrames.Add(_frameStaggerIn);
            }

            frameTime += durationIn;

            // create frame in
            EasingDoubleKeyFrame _frameIn = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                Value = endValue,
                EasingFunction = cubicEaseIn
            };
            frames.KeyFrames.Add(_frameIn);

            frameTime += restEnd;

            // create frame rest end
            if (restEnd > 0)
            {
                EasingDoubleKeyFrame _frameRestEnd = new EasingDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                    Value = endValue,
                    EasingFunction = cubicEaseOut
                };
                frames.KeyFrames.Add(_frameRestEnd);
            }

            frameTime += durationOut;

            // create frame our
            EasingDoubleKeyFrame _frameOut = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                Value = startValue,
                EasingFunction = cubicEaseOut
            };
            frames.KeyFrames.Add(_frameOut);

            frameTime += staggerDelayOut;

            // if we have a stagger delay
            if (staggerDelayOut > 0)
            {
                // create stagger frame
                EasingDoubleKeyFrame _frameStaggerOut = new EasingDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                    Value = startValue,
                    EasingFunction = cubicEaseOut
                };
                frames.KeyFrames.Add(_frameStaggerOut);
            }

            return frames;
        }
    }
}

// This function derives from: 
// https://dgkanatsios.com/2013/02/11/animating-a-spritesheet-in-a-windows-store-app-using-xaml-and-c-3/
// https://github.com/dgkanatsios/UniversalHelpers/blob/master/UniversalHelpersLibrary/StoryboardHelpers.cs
///// <summary>
///// Initiates a spritesheet animation
///// </summary>
///// <param name="shape">Shape to animate on (will create an ImageBrush)</param>
///// <param name="spriteSheetColumns">Spritesheet columns</param>
///// <param name="spriteSheetRows">Spritesheet rows</param>
///// <param name="image">The spritesheet image</param>
///// <param name="width">Width of the sprite on the spritesheet</param>
///// <param name="height">Height of the sprite on the spritesheet</param>
///// <param name="keyframeTime">Time that each keyframe should have</param>
///// <returns>Storyboard created</returns>
//public static Storyboard BeginSpriteSheetStoryboard(Shape shape, int spriteSheetColumns, int spriteSheetRows, BitmapImage image,
//    double width, double height, int keyframeTime)
//{
//    ImageBrush ib = new ImageBrush() { Stretch = Stretch.None, AlignmentX = AlignmentX.Left, AlignmentY = AlignmentY.Top };
//    ib.Transform = new CompositeTransform();
//    ib.ImageSource = image;

//    shape.Fill = ib;

//    Storyboard sb = new Storyboard();
//    sb.RepeatBehavior = RepeatBehavior.Forever;

//    ObjectAnimationUsingKeyFrames frm = new ObjectAnimationUsingKeyFrames();
//    ObjectAnimationUsingKeyFrames frm2 = new ObjectAnimationUsingKeyFrames();
//    frm.BeginTime = new TimeSpan(0, 0, 0);
//    frm2.BeginTime = new TimeSpan(0, 0, 0);


//    int time = 0;
//    for (int j = 0; j < spriteSheetRows; j++)
//    {
//        for (int i = 0; i < spriteSheetColumns; i++)
//        {
//            DiscreteObjectKeyFrame dokf = new DiscreteObjectKeyFrame();
//            dokf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(time));
//            dokf.Value = -(i * width);
//            frm.KeyFrames.Add(dokf);


//            DiscreteObjectKeyFrame dokf2 = new DiscreteObjectKeyFrame();
//            dokf2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(time));
//            dokf2.Value = -(j * height);
//            frm2.KeyFrames.Add(dokf2);
//            time += keyframeTime;
//        }
//    }
//    Storyboard.SetTarget(frm, shape.Fill);
//    Storyboard.SetTarget(frm2, shape.Fill);
//    Storyboard.SetTargetProperty(frm, "(ImageBrush.Transform).(CompositeTransform.TranslateX)");
//    Storyboard.SetTargetProperty(frm2, "(ImageBrush.Transform).(CompositeTransform.TranslateY)");
//    sb.Children.Add(frm);
//    sb.Children.Add(frm2);
//    sb.Begin();
//    return sb;
//}