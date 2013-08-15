using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CSCore.Visualization.WPF
{
    [TemplatePart(Name = "PART_visualationDisplay", Type = typeof(Image))]
    public class WaveForm : SampleVisualizationBase
    {
        private Image PART_visualationDisplay;
        private RenderTargetBitmap _bmp;

        public WaveForm()
        {
            PART_visualationDisplay = new Image();
            Content = PART_visualationDisplay;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var img = Template.FindName("PART_visualationDisplay", this) as Image;
            if (img != null)
            {
                PART_visualationDisplay = img;
                Content = PART_visualationDisplay;
            }
        }

        private int v;

        protected override void OnUpdate(float[] left, float[] right)
        {
            var values = left == null ? right : left;

            if (_bmp == null || v != values.Length)
            {
                _bmp = new RenderTargetBitmap(values.Length, 200, 120, 96, PixelFormats.Pbgra32);
                v = values.Length;
            }
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            drawingContext.DrawLine(AxisPen, new Point(0, _bmp.Height / 2), new Point(_bmp.Width, _bmp.Height / 2));

            if (left != null)
            {
                var pen = DrawingPenLeft.Clone();
                pen.Freeze();
                Render(drawingContext, pen, left);
            }
            if (right != null)
            {
                var pen = DrawingPenRight.Clone();
                pen.Freeze();
                Render(drawingContext, pen, right);
            }

            drawingContext.Close();
            _bmp.Clear();
            _bmp.Render(drawingVisual);
            PART_visualationDisplay.Source = _bmp;
        }

        private void Render(DrawingContext drawingContext, Pen pen, float[] values)
        {
            double xinterval = _bmp.Width / values.Length;
            double halfheight = _bmp.Height / 2;
            for (int i = 0; i < values.Length - 1; i++)
            {
                Point p1 = new Point(xinterval * i, values[i] * halfheight + halfheight);
                Point p2 = new Point(xinterval * (i + 1), values[i + 1] * halfheight + halfheight);

                drawingContext.DrawLine(pen, p1, p2);
            }
        }

        public Pen DrawingPenLeft
        {
            get { return (Pen)GetValue(DrawingPenProperty); }
            set { SetValue(DrawingPenProperty, value); }
        }

        public static readonly DependencyProperty DrawingPenProperty =
            DependencyProperty.Register("DrawingPenLeft", typeof(Pen), typeof(WaveForm), new PropertyMetadata(new Pen(Brushes.Red, 0.5)));

        public Pen DrawingPenRight
        {
            get { return (Pen)GetValue(DrawingPenRightProperty); }
            set { SetValue(DrawingPenRightProperty, value); }
        }

        public static readonly DependencyProperty DrawingPenRightProperty =
            DependencyProperty.Register("DrawingPenRight", typeof(Pen), typeof(WaveForm), new PropertyMetadata(new Pen(Brushes.Green, 0.5)));

        public Pen AxisPen
        {
            get { return (Pen)GetValue(AxisPenProperty); }
            set { SetValue(AxisPenProperty, value); }
        }

        public static readonly DependencyProperty AxisPenProperty =
            DependencyProperty.Register("AxisPen", typeof(Pen), typeof(WaveForm), new PropertyMetadata(new Pen(Brushes.Black, 0.3)));

        public bool RenderAxis
        {
            get { return (bool)GetValue(RenderAxisProperty); }
            set { SetValue(RenderAxisProperty, value); }
        }

        public static readonly DependencyProperty RenderAxisProperty =
            DependencyProperty.Register("RenderAxis", typeof(bool), typeof(WaveForm), new PropertyMetadata(true));
    }
}