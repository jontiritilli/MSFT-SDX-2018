using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Helpers;

// this class is sourced from https://blogs.u2u.be/diederik/post/Drawing-a-Circular-Gradient-in-Windows-Store-and-Windows-Phone-apps

namespace SDX.Toolkit.Controls
{

    public class RingSlice : Path
    {
        #region Private Members

        private bool _isUpdating;

        #endregion

        #region Constructor

        public RingSlice()
        {
            this.SizeChanged += OnSizeChanged;

            // inherited dependency property
            new PropertyChangeEventSource<double>(
                this, "StrokeThickness", BindingMode.OneWay).ValueChanged +=
                OnStrokeThicknessChanged;
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(RingSlice), new PropertyMetadata(0d, OnStartAngleChanged));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(RingSlice), new PropertyMetadata(0d, OnEndAngleChanged));

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(RingSlice), new PropertyMetadata(200d, OnRadiusChanged));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(RingSlice), new PropertyMetadata(180d, OnInnerRadiusChanged));

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point?), typeof(RingSlice), new PropertyMetadata(null, OnCenterChanged));

        public Point? Center
        {
            get { return (Point?)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        #endregion

        #region Event Handlers

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdatePath();
        }

        private void OnStrokeThicknessChanged(object sender, double e)
        {
            UpdatePath();
        }

        //private static void OnPathOpacityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (sender is RingSlice ring)
        //    {
        //        ring.Opacity = (double)e.NewValue;
        //    }
        //}
        
        private static void OnStartAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
            var oldStartAngle = (double)e.OldValue;
            var newStartAngle = (double)e.NewValue;
            target.OnStartAngleChanged(oldStartAngle, newStartAngle);
        }

        private void OnStartAngleChanged(double oldStartAngle, double newStartAngle)
        {
            UpdatePath();
        }

        private static void OnEndAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
            var oldEndAngle = (double)e.OldValue;
            var newEndAngle = (double)e.NewValue;
            target.OnEndAngleChanged(oldEndAngle, newEndAngle);
        }

        private void OnEndAngleChanged(double oldEndAngle, double newEndAngle)
        {
            UpdatePath();
        }

        private static void OnRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
            var oldRadius = (double)e.OldValue;
            var newRadius = (double)e.NewValue;
            target.OnRadiusChanged(oldRadius, newRadius);
        }

        private void OnRadiusChanged(double oldRadius, double newRadius)
        {
            this.Width = this.Height = 2 * Radius;
            UpdatePath();
        }

        private static void OnInnerRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
            var oldInnerRadius = (double)e.OldValue;
            var newInnerRadius = (double)e.NewValue;
            target.OnInnerRadiusChanged(oldInnerRadius, newInnerRadius);
        }

        private void OnInnerRadiusChanged(double oldInnerRadius, double newInnerRadius)
        {
            if (newInnerRadius < 0)
            {
                throw new ArgumentException("InnerRadius can't be a negative value.", "InnerRadius");
            }

            UpdatePath();
        }

        private static void OnCenterChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)d;
            Point? oldCenter = (Point?)e.OldValue;
            Point? newCenter = target.Center;
            target.OnCenterChanged(oldCenter, newCenter);
        }

        private void OnCenterChanged(
            Point? oldCenter, Point? newCenter)
        {
            UpdatePath();
        }

        #endregion

        #region Public Methods

        public void BeginUpdate()
        {
            _isUpdating = true;
        }

        public void EndUpdate()
        {
            _isUpdating = false;
            UpdatePath();
        }

        #endregion

        #region Render Methods

        private void UpdatePath()
        {
            var innerRadius = this.InnerRadius + this.StrokeThickness / 2;
            var outerRadius = this.Radius - this.StrokeThickness / 2;

            if (_isUpdating ||
                this.ActualWidth == 0 ||
                innerRadius <= 0 ||
                outerRadius < innerRadius)
            {
                return;
            }

            var pathGeometry = new PathGeometry() { FillRule = FillRule.Nonzero };
            var pathFigure = new PathFigure();
            pathFigure.IsClosed = true;
            
            var center =
                this.Center ??
                new Point(
                    outerRadius + this.StrokeThickness / 2,
                    outerRadius + this.StrokeThickness / 2);

            // Starting Point
            pathFigure.StartPoint =
                new Point(
                    center.X + Math.Sin(StartAngle * Math.PI / 180) * innerRadius,
                    center.Y - Math.Cos(StartAngle * Math.PI / 180) * innerRadius);

            // Inner Arc
            ArcSegment innerArcSegment = new ArcSegment()
            {
                IsLargeArc = (EndAngle - StartAngle) >= 180.0,
                Point = new Point(center.X + Math.Sin(EndAngle * Math.PI / 180) * innerRadius,
                                    center.Y - Math.Cos(EndAngle * Math.PI / 180) * innerRadius),
                Size = new Size(innerRadius, innerRadius),
                SweepDirection = SweepDirection.Clockwise
            };

            // Endpoint Arc
            double radius = (outerRadius - innerRadius) / 2;
            ArcSegment endpointArcSegment = new ArcSegment()
            {
                IsLargeArc = false,
                Point = new Point()
                {
                    X = center.X + Math.Sin(EndAngle * Math.PI / 180) * outerRadius,
                    Y = center.Y - Math.Cos(EndAngle * Math.PI / 180) * outerRadius
                },
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Counterclockwise
            };

            // Outer Arc
            ArcSegment outerArcSegment = new ArcSegment();
            outerArcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
            outerArcSegment.Point =
                new Point(
                        center.X + Math.Sin(StartAngle * Math.PI / 180) * outerRadius,
                        center.Y - Math.Cos(StartAngle * Math.PI / 180) * outerRadius);
            outerArcSegment.Size = new Size(outerRadius, outerRadius);
            outerArcSegment.SweepDirection = SweepDirection.Counterclockwise;

            // Arc back to starting point
            radius = (outerRadius - innerRadius) / 2;
            ArcSegment startpointArcSegment = new ArcSegment()
            {
                IsLargeArc = false,
                Point = new Point()
                {
                    X = center.X + Math.Sin(StartAngle * Math.PI / 180) * innerRadius,
                    Y = center.Y - Math.Cos(StartAngle * Math.PI / 180) * innerRadius
                },
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Counterclockwise
            };

            pathFigure.Segments.Add(innerArcSegment);
            pathFigure.Segments.Add(endpointArcSegment);
            pathFigure.Segments.Add(outerArcSegment);
            pathFigure.Segments.Add(startpointArcSegment);
            pathGeometry.Figures.Add(pathFigure);
            this.InvalidateArrange();
            this.Data = pathGeometry;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // call base
            Size baseSize = base.MeasureOverride(availableSize);

            double width = 0;
            double height = 0;
            Size desiredSize;

            // calculate our actual width
            double widthActual = 2 * this.Radius;

            // calculate our actual height
            double heightActual = 2 * this.Radius;

            // default to our preferred values
            width = widthActual;
            height = heightActual;

            // if we're wider than available size, use available width
            if (widthActual > availableSize.Width)
            {
                // take the available width
                width = height = availableSize.Width;

                // change the radius
                double pathThickness = this.Radius - this.InnerRadius;
                this.Radius = width / 2;
                this.InnerRadius = this.Radius - pathThickness;
            }

            // return our preferred/adjusted size
            desiredSize.Width = width;
            desiredSize.Height = height;

            return desiredSize;
        }
        #endregion
    }
}

