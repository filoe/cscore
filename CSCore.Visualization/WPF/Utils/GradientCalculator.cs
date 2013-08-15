using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CSCore.Visualization.WPF
{
    public class GradientCalculator
    {
        private List<Color> _colors;

        public List<Color> Colors
        {
            get { return _colors ?? (_colors = new List<Color>()); }
            set { _colors = value; }
        }

        public GradientCalculator(params Color[] colors)
        {
            _colors = new List<Color>(colors);
        }

        public Color GetColor(float perc)
        {
            if (_colors.Count > 1)
            {
                int index = Convert.ToInt32((_colors.Count - 1) * perc - 0.5f);
                float upperIntensity = (perc % (1f / (_colors.Count - 1))) * (_colors.Count - 1);
                return Color.FromArgb(
                    0,
                    (byte)(_colors[index + 1].R * upperIntensity + _colors[index].R * (1f - upperIntensity)),
                    (byte)(_colors[index + 1].G * upperIntensity + _colors[index].G * (1f - upperIntensity)),
                    (byte)(_colors[index + 1].B * upperIntensity + _colors[index].B * (1f - upperIntensity)));
            }
            else
            {
                return _colors.FirstOrDefault(); ;
            }
        }
    }
}