using System;
using System.Drawing;
using System.Linq;

namespace WinformsVisualization.Visualization
{
    internal class GradientCalculator
    {
        private Color[] _colors;

        public GradientCalculator()
        {
        }

        public GradientCalculator(params Color[] colors)
        {
            _colors = colors;
        }

        public Color[] Colors
        {
            get { return _colors ?? (_colors = new Color[] {}); }
            set { _colors = value; }
        }

        public Color GetColor(float perc)
        {
            if (_colors.Length > 1)
            {
                int index = Convert.ToInt32((_colors.Length - 1) * perc - 0.5f);
                float upperIntensity = (perc % (1f / (_colors.Length - 1))) * (_colors.Length - 1);
                return Color.FromArgb(
                    255,
                    (byte) (_colors[index + 1].R * upperIntensity + _colors[index].R * (1f - upperIntensity)),
                    (byte) (_colors[index + 1].G * upperIntensity + _colors[index].G * (1f - upperIntensity)),
                    (byte) (_colors[index + 1].B * upperIntensity + _colors[index].B * (1f - upperIntensity)));
            }
            return _colors.FirstOrDefault();
        }
    }
}