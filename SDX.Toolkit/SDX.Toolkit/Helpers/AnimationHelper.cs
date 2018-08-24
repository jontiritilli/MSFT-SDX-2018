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
