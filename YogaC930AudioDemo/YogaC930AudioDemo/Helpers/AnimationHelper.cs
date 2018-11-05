using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

using YogaC930AudioDemo.Converters;
using YogaC930AudioDemo.Models;


namespace YogaC930AudioDemo.Helpers
{
    public enum TranslateAxes
    {
        Horizontal,
        Vertical,
    }

    public enum TranslateDirections
    {
        FromLeft,
        FromRight,
        FromTop,
        FromBottom,
    }

    public enum AnimationTypes
    {
        Background,
        Standard,
        Hero,
        Radiate,
        None,
    }

    public static class AnimationHelper
    {
        public static double DEFAULT_DURATION = 200;
        public static double DEFAULT_TRANSLATE_OFFSET = 30;
        public static double DEFAULT_TRANSLATE_HERO_OFFSET = 300;

        public static void PerformPageEntranceAnimation(Page page)
        {
            if (null != page)
            {
                // stagger delay counter
                double staggerDelay = 0;

                // create default easing
                CubicEase easeIn = new CubicEase() { EasingMode = EasingMode.EaseIn };
                CubicEase easeOut = new CubicEase() { EasingMode = EasingMode.EaseOut };

                // storyboards
                List<Storyboard> storyboards = new List<Storyboard>();

                // get all the objects to animate
                List<UIElement> animatables = GetAllAnimatableChildren(page.Content);

                // loop through them
                foreach (UIElement animatable in animatables)
                {
                    // get the animation tag properties
                    AnimationTypes animationType = AnimationTag.GetAnimationType(animatable);
                    TranslateAxes translateAxis = AnimationTag.GetTranslateAxis(animatable);
                    TranslateDirections translateDirection = AnimationTag.GetTranslateDirection(animatable);

                    // create the animations
                    switch (animationType)
                    {
                        case AnimationTypes.Background:
                            storyboards.Add(CreateEasingAnimation(animatable, "Opacity", 0.0, 0.0, 1.0,
                                                                    DEFAULT_DURATION, staggerDelay,
                                                                    false, false, new RepeatBehavior(1d)));
                            break;

                        case AnimationTypes.Standard:
                            storyboards.Add(CreateEasingAnimation(animatable, "Opacity", 0.0, 0.0, 1.0,
                                                                    DEFAULT_DURATION, staggerDelay,
                                                                    false, false, new RepeatBehavior(1d)));

                            double startValue1 = ((TranslateDirections.FromTop == translateDirection) ||
                                                 (TranslateDirections.FromLeft == translateDirection))
                                                 ? -1 * DEFAULT_TRANSLATE_OFFSET : DEFAULT_TRANSLATE_OFFSET;

                            storyboards.Add(CreateTranslateAnimation(animatable, translateAxis, startValue1, startValue1, 0,
                                                                        easeIn, easeOut, DEFAULT_DURATION, staggerDelay,
                                                                        false, false, new RepeatBehavior(1d)));
                            break;

                        case AnimationTypes.Hero:
                            storyboards.Add(CreateEasingAnimation(animatable, "Opacity", 0.0, 0.0, 1.0,
                                                                    DEFAULT_DURATION, staggerDelay,
                                                                    false, false, new RepeatBehavior(1d)));

                            double startValue2 = ((TranslateDirections.FromTop == translateDirection) ||
                                                 (TranslateDirections.FromLeft == translateDirection))
                                                 ? -1 * DEFAULT_TRANSLATE_HERO_OFFSET : DEFAULT_TRANSLATE_HERO_OFFSET;

                            storyboards.Add(CreateTranslateAnimation(animatable, translateAxis, startValue2, startValue2, 0,
                                                                        easeIn, easeOut, DEFAULT_DURATION, staggerDelay,
                                                                        false, false, new RepeatBehavior(1d)));
                            break;

                        case AnimationTypes.Radiate:
                            storyboards.Add(CreateEasingAnimation(animatable, "Opacity", 0.0, 0.0, 1.0,
                                                                    DEFAULT_DURATION * 2, staggerDelay,
                                                                    false, false, new RepeatBehavior(1d)));
                            staggerDelay += DEFAULT_DURATION * 2;

                            //storyboards.Add(CreateEasingAnimation(animatable, "Opacity", 0.5, 0.5, 1.0,
                            //            DEFAULT_DURATION * 2, staggerDelay,
                            //            false, false, new RepeatBehavior(1d)));

                            //staggerDelay += DEFAULT_DURATION;

                            break;

                        case AnimationTypes.None:
                        default:
                            break;
                    }

                    // increment stagger delay
                    staggerDelay += 2 * (DEFAULT_DURATION / 3);
                }

                // loop through the animations and start them
                foreach (Storyboard storyboard in storyboards)
                {
                    storyboard.Begin();
                }
            }
        }

        public static void PerformPageExitAnimation(Page page)
        {
            
        }

        private static List<UIElement> GetAllAnimatableChildren(UIElement element)
        {
            List<UIElement> children = new List<UIElement>();

            if (null != element)
            {
                // if the element is animatable, add it
                if (AnimationTypes.None != AnimationTag.GetAnimationType(element))
                {
                    children.Add(element);
                }

                // what type is the element?
                if (element is Panel panelElement)
                {
                    // element is a panel which has .Children

                    // loop through its children
                    foreach (UIElement child in panelElement.Children)
                    {
                        // yes, recursively get its children
                        children.AddRange(GetAllAnimatableChildren(child));
                    }
                }
                else if (element is Border borderElement)
                {
                    // element is a border which may have 1 child

                    // is the border animatable?
                    if (AnimationTypes.None != AnimationTag.GetAnimationType(borderElement))
                    {
                        // yes, add it
                        children.Add(borderElement);
                    }

                    // does the border have a child?
                    if (null != borderElement.Child)
                    {
                        // yes, recursively get its children
                        children.AddRange(GetAllAnimatableChildren(borderElement.Child));
                    }
                }
                //else
                //{
                //    // is the element animatable?
                //    if (AnimationTypes.None != AnimationTag.GetAnimationType(element))
                //    {
                //        // yes, add it
                //        children.Add(element);
                //    }
                //}
            }

            return children;
        }

        public static void PerformAnimation(DependencyObject dependencyObject, string propertyName,
                                            double defaultValue, double startingValue, double endingValue,
                                            double duration, double staggerDelay = 0)
        {
            Storyboard storyboard = null;

            if (null != dependencyObject)
            {
                storyboard = CreateEasingAnimation(dependencyObject, propertyName, defaultValue, startingValue, endingValue, duration, staggerDelay, false, false, new RepeatBehavior(1d));

                storyboard.Begin();
            }
        }

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

        public static void PerformTranslateIn(DependencyObject dependencyObject, TranslateAxes translateAxis,
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

        public static void PerformTranslateIn(DependencyObject dependencyObject, TranslateAxes translateAxis,
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

        public static Storyboard CreateTranslateAnimation(DependencyObject dependencyObject, TranslateAxes axis,
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
                translateTransform.X = (TranslateAxes.Horizontal == axis) ? defaultValue : 0;
                translateTransform.Y = (TranslateAxes.Vertical == axis) ? defaultValue : 0;

                // create default easing
                CubicEase easeIn = new CubicEase()
                {
                    EasingMode = EasingMode.EaseIn
                };

                CubicEase easeOut = new CubicEase()
                {
                    EasingMode = EasingMode.EaseOut
                };

                string propertyName = (TranslateAxes.Horizontal == axis) ? "X" : "Y";

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
