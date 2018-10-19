using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;


namespace YogaC930AudioDemo.Helpers
{
    public enum TranslateAxis
    {
        Horizontal,
        Vertical
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

        public static void PerformTranslateIn(DependencyObject dependencyObject, TranslateAxis translateAxis,
                                        double defaultValue, double startingValue, double endingValue,
                                        double duration, double staggerDelay = 0)
        {
            // create default easing
            CubicEase easeIn = new CubicEase()
            {
                EasingMode = EasingMode.EaseIn
            };

            CubicEase easeOut = new CubicEase()
            {
                EasingMode = EasingMode.EaseOut
            };

            PerformTranslateIn(dependencyObject, translateAxis, defaultValue, startingValue, endingValue, easeIn, easeOut, duration, staggerDelay);
        }

        public static void PerformTranslateIn(DependencyObject dependencyObject, TranslateAxis translateAxis,
                                                double defaultValue, double startingValue, double endingValue,
                                                EasingFunctionBase easingFunctionIn, EasingFunctionBase easingFunctionOut,
                                                double duration, double staggerDelay = 0)
        {
            Storyboard storyboard = null;

            if (null != dependencyObject)
            {
                storyboard = CreateTranslateAnimation(dependencyObject, translateAxis,
                                                        defaultValue, startingValue, endingValue,
                                                        easingFunctionIn, easingFunctionOut,
                                                        duration, staggerDelay,
                                                        false, false, new RepeatBehavior(1d));

                storyboard.Begin();
            }
        }

        public static Storyboard CreateTranslateAnimation(DependencyObject dependencyObject, TranslateAxis axis, 
                                            double defaultValue, double startValue, double endValue,
                                            EasingFunctionBase easingFunctionIn, EasingFunctionBase easingFunctionOut,
                                            double duration, double staggerDelay,
                                            bool autoReverse, bool repeatForever, RepeatBehavior repeatBehavior)
        {
            if (null == dependencyObject) { return null; }

            // total duration
            double totalDuration = duration + staggerDelay;

            // create the storyboard
            Storyboard storyboard = new Storyboard()
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                AutoReverse = autoReverse,
                RepeatBehavior = (repeatForever) ? RepeatBehavior.Forever : repeatBehavior
            };

            // need a translate transform to animate
            TranslateTransform translateTransform = FindOrCreateTranslateTransform(dependencyObject);

            if (null != translateTransform)
            {
                // set the default value for the transform
                translateTransform.X = (TranslateAxis.Horizontal == axis) ? defaultValue : 0;
                translateTransform.Y = (TranslateAxis.Vertical == axis) ? defaultValue : 0;

                // create default easing
                CubicEase easeIn = new CubicEase()
                {
                    EasingMode = EasingMode.EaseIn
                };

                CubicEase easeOut = new CubicEase()
                {
                    EasingMode = EasingMode.EaseOut
                };

                string propertyName = (TranslateAxis.Horizontal == axis) ? "X" : "Y";

                DoubleAnimationUsingKeyFrames daKeyFrames = CreateEasingKeyFrames(translateTransform, propertyName,
                                                                    defaultValue, startValue, endValue,
                                                                    easeIn, easeOut, duration, staggerDelay);

                storyboard.Children.Add(daKeyFrames);
                Storyboard.SetTarget(daKeyFrames, translateTransform);
                Storyboard.SetTargetProperty(daKeyFrames, propertyName);
            }

            return storyboard;
        }

        public static Storyboard CreateEasingAnimation(DependencyObject dependencyObject, string propertyName,
                                                            double defaultValue, double startValue, double endValue,
                                                            double duration, double staggerDelay,
                                                            bool autoReverse, bool repeatForever, RepeatBehavior repeatBehavior)
        {
            // create default easing
            CubicEase easeIn = new CubicEase()
            {
                EasingMode = EasingMode.EaseIn
            };

            CubicEase easeOut = new CubicEase()
            {
                EasingMode = EasingMode.EaseOut
            };

            return CreateEasingAnimation(dependencyObject, propertyName, defaultValue, startValue, endValue, easeIn, easeOut, duration, staggerDelay, autoReverse, repeatForever, repeatBehavior);
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

        public static TranslateTransform FindOrCreateTranslateTransform(DependencyObject dependencyObject)
        {
            TranslateTransform translateTransform = null;

            // convert the object to a UIElement
            if (dependencyObject is UIElement ui)
            {
                // make sure it's valid
                if (null != ui)
                {
                    // grab the transform
                    Transform transform = ui.RenderTransform;

                    // is it null?
                    if (null == transform)
                    {
                        // yes it's null; there's no transform there

                        // create one
                        translateTransform = new TranslateTransform();

                        // assign it to the ui element
                        ui.RenderTransform = translateTransform;
                    }
                    else
                    {
                        // no, we have something

                        // is it the simple case, a translate transform?
                        if (transform is TranslateTransform xlateTransform)
                        {
                            // it's a TranslateTransform, so use it
                            translateTransform = xlateTransform;
                        }
                        // is it a group?
                        else if (transform is TransformGroup group)
                        {
                            bool found = false;

                            // search the group for a translate transform
                            foreach (Transform t in group.Children)
                            {
                                // if we've found a translate transform
                                if (t is TranslateTransform tt)
                                {
                                    // save it and exit
                                    translateTransform = tt;
                                    found = true;
                                    break;
                                }
                            }

                            // if we didn't find it, create it
                            if (!found)
                            {
                                // create it
                                translateTransform = new TranslateTransform();

                                // add it to the group
                                group.Children.Add(translateTransform);
                            }
                        }
                        else
                        {
                            // it's another kind of transform, so create a group
                            TransformGroup transformGroup = new TransformGroup();

                            // add the existing transform to the group
                            transformGroup.Children.Add(transform);

                            // create a new translate transform
                            translateTransform = new TranslateTransform();

                            // add it to the group
                            transformGroup.Children.Add(translateTransform);

                            // set the uielement rendertransform to the group
                            ui.RenderTransform = transformGroup;
                        }
                    }
                }
            }

            return translateTransform;
        }
    }
}
