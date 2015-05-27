using System;
using System.Drawing;
using CSCore.DSP;

namespace WinformsVisualization.Visualization
{
    public class VoicePrint3DSpectrum : SpectrumBase
    {
        private readonly GradientCalculator _colorCalculator;
        private bool _isInitialized;

        public VoicePrint3DSpectrum(FftSize fftSize)
        {
            _colorCalculator = new GradientCalculator();
            Colors = new[] {Color.Black, Color.Blue, Color.Cyan, Color.Lime, Color.Yellow, Color.Red};

            FftSize = fftSize;
        }

        public Color[] Colors
        {
            get { return _colorCalculator.Colors; }
            set
            {
                if (value == null || value.Length <= 0)
                    throw new ArgumentException("value");

                _colorCalculator.Colors = value;
            }
        }

        public int PointCount
        {
            get { return SpectrumResolution; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                SpectrumResolution = value;

                UpdateFrequencyMapping();
            }
        }

        public bool CreateVoicePrint3D(Graphics graphics, RectangleF clipRectangle, float xPos, Color background,
            float lineThickness = 1f)
        {
            if (!_isInitialized)
            {
                UpdateFrequencyMapping();
                _isInitialized = true;
            }

            var fftBuffer = new float[(int) FftSize];

            if (SpectrumProvider.GetFftData(fftBuffer, this))
            {
                SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(1.0, fftBuffer);
                using (var pen = new Pen(background, lineThickness))
                {
                    float currentYOffset = clipRectangle.Y + clipRectangle.Height;

                    for (int i = 0; i < spectrumPoints.Length; i++)
                    {
                        SpectrumPointData p = spectrumPoints[i];

                        float xCoord = clipRectangle.X + xPos;
                        float pointHeight = clipRectangle.Height / spectrumPoints.Length;

                        pen.Color = _colorCalculator.GetColor((float) p.Value);
                        //pen.Color = Color.FromArgb(255, pen.Color.R, pen.Color.G, pen.Color.B);

                        var p1 = new PointF(xCoord, currentYOffset);
                        var p2 = new PointF(xCoord, currentYOffset - pointHeight);

                        graphics.DrawLine(pen, p1, p2);

                        currentYOffset -= pointHeight;
                    }
                }
                return true;
            }
            return false;
        }
    }
}