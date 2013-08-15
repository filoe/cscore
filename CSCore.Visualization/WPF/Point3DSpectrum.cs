using CSCore.Utils;
using CSCore.Visualization.WPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSCore.Visualization.WPF
{
    [TemplatePart(Name = "PART_visualationDisplay", Type = typeof(Image))]
    public class Point3DSpectrum : FFTVisualizationBase
    {
        public static IEnumerable<Color> DefaultColors
        {
            get { return new Color[] { Colors.Black, Colors.Blue, Colors.Cyan, Colors.Lime, Colors.Yellow, Colors.Red }; }
        }

        private Image PART_visualationDisplay;
        private PixelManipulationBitmap _pmbitmap;
        private GradientCalculator _gradientCalculator;
        private int _bands;
        private int _activeColumn = 0;

        public Point3DSpectrum()
        {
            PART_visualationDisplay = new Image();
            Content = PART_visualationDisplay;
            _gradientCalculator = new GradientCalculator(DefaultColors.ToArray());
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
            int pts = values.Length / 2;

            //create bitmap
            if (_pmbitmap == null || _bands != values.Length)
            {
                _pmbitmap = CreateBitmap(values.Length);
                _bands = values.Length;
            }

            _pmbitmap.BeginRender();

            for (int n = 0; n < pts; n++)
            {
                var color = _gradientCalculator.GetColor((float)values[n]);
                _pmbitmap.SetPixel(_activeColumn, pts - n - 1, color);
            }

            PART_visualationDisplay.Source = _pmbitmap.EndRender();

            if (++_activeColumn > _pmbitmap.Width - 1)
                _activeColumn = 0;
        }

        private PixelManipulationBitmap CreateBitmap(int bands)
        {
            return new PixelManipulationBitmap(bands / 2 * 3, bands / 2);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == SpectrumColorsProperty)
            {
                _gradientCalculator.Colors = (e.NewValue as IEnumerable<Color>).ToList();
            }
        }

        public IEnumerable<Color> SpectrumColors
        {
            get { return (IEnumerable<Color>)GetValue(SpectrumColorsProperty); }
            set { SetValue(SpectrumColorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SpectrumColors. This enables
        // animation, styling, binding, etc...
        public static readonly DependencyProperty SpectrumColorsProperty =
            DependencyProperty.Register("SpectrumColors", typeof(IEnumerable<Color>), typeof(Point3DSpectrum), new PropertyMetadata(DefaultColors));

        protected override bool ValidateTimer()
        {
            return true;
        }
    }
}