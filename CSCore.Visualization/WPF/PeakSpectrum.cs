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
    public class PeakSpectrum : FFTVisualizationBase
    {
        private Image PART_visualationDisplay;
        private RenderTargetBitmap _bmp;

        public PeakSpectrum()
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

        protected override void OnUpdate(double[] values)
        {
            const double space = 3;
            const double barwidth = 8;

            int pts = values.Length / 2;

            int width = 1000;//pts * (space + barwidth) - space;
            int height = width / 4;
            if (_bmp == null || width != _bmp.PixelWidth)
            {
                _bmp = new RenderTargetBitmap(width, height, 120, 96, PixelFormats.Pbgra32);
            }
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            Brush brush = DrawingBrush.Clone();
            brush.Freeze();

            double totalWidth = width / pts;
            double totalBarWidth = totalWidth * ((barwidth) / (barwidth + space));
            double totalSpace = totalWidth * ((space) / (barwidth + space));
            for (int i = 0; i < pts; i++)
            {
                double x = i * totalWidth;
                double y1 = height;
                double y2 = y1 - values[i] * height;
                drawingContext.DrawRectangle(brush, null, new Rect(x, y2, totalBarWidth, y1));
            }

            drawingContext.Close();
            _bmp.Clear();
            _bmp.Render(drawingVisual);
            PART_visualationDisplay.Source = _bmp;
        }

        public Brush DrawingBrush
        {
            get { return (Brush)GetValue(DrawingBrushProperty); }
            set { SetValue(DrawingBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrawingBrush. This enables animation,
        // styling, binding, etc...
        public static readonly DependencyProperty DrawingBrushProperty =
            DependencyProperty.Register("DrawingBrush", typeof(Brush), typeof(PeakSpectrum), new PropertyMetadata(Brushes.Red));
    }
}