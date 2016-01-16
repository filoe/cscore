using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CSCore;
using CSCoreWaveform.Annotations;

namespace CSCoreWaveform
{
    /// <summary>
    /// Interaction logic for Waveform.xaml
    /// </summary>
    public partial class Waveform : UserControl, INotifyPropertyChanged
    {
        private LineGeometry _positionGeometry;

        public Waveform()
        {
            InitializeComponent();
        }

        public IList<float> ChannelData
        {
            get { return (IList<float>)GetValue(ChannelDataProperty); }
            set
            {
                SetValue(ChannelDataProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ChannelDataFloats.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChannelDataProperty =
            DependencyProperty.Register("ChannelData", typeof(IList<float>), typeof(Waveform), new PropertyMetadata(null, async (d, e) =>
                {
                    await ((Waveform) d).UpdateWaveformAsync();
                }));




        public double PositionInPerc
        {
            get { return (double)GetValue(PositionInPercProperty); }
            set
            {
                SetValue(PositionInPercProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for PositionInPerc.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionInPercProperty =
            DependencyProperty.Register("PositionInPerc", typeof(double), typeof(Waveform), new PropertyMetadata(0.0, (d, e) =>
                {
                    ((Waveform)d).UpdatePosition((double)e.NewValue);
                }));

        private async Task UpdateWaveformAsync()
        {
            if (!Dispatcher.CheckAccess())
            {
                await Dispatcher.InvokeAsync(async () => await UpdateWaveformAsync());
                return;
            }

            List<Point> points = new List<Point>();
            double centerHeight = PART_Canvas.RenderSize.Height / 2d;
            double x = 0;
            var channelData = ChannelData;
            double minValue = 0;
            double maxValue = 1.5;
            double dbScale = maxValue - minValue;

            points.Add(new Point(0, centerHeight));
            points.Add(new Point(0, centerHeight));

            int iOffset = 0;
            for (int i = 0; i < channelData.Count; i++)
            {
                if (i == channelData.Count / 2)
                {
                    iOffset = channelData.Count / 2;
                    points.Add(new Point(x, centerHeight));
                    points.Add(new Point(0, centerHeight));
                }

                x = (PART_Canvas.RenderSize.Width / (channelData.Count / 2)) * (i - iOffset);
                Debug.Assert(i - iOffset >= 0);
                double height = ((channelData[i] - minValue) / dbScale) * centerHeight;
                height += centerHeight;
                points.Add(new Point(x, height));
            }

            points.Add(new Point(x, centerHeight));
            points.Add(new Point(0, centerHeight));

            PolyLineSegment lineSegment = new PolyLineSegment(points, false);

            PathFigure figure = new PathFigure();
            figure.Segments.Add(lineSegment);

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            //var path = new Path
            //{
            //    Data = geometry,
            //    Fill = Brushes.Green,
            //    Stroke = Brushes.Transparent,
            //    StrokeThickness = PART_Canvas.RenderSize.Width / (channelData.Count / 2)
            //};

            PART_Path.Data = geometry;
            PART_Path.StrokeThickness = PART_Canvas.RenderSize.Width / (channelData.Count / 2);

            var centerLinePath = new Path()
            {
                Data = new LineGeometry(new Point(points.First().X, centerHeight), new Point(x, centerHeight)),
                Fill = Brushes.Transparent,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            _positionGeometry = new LineGeometry();
            var positionPath = new Path()
            {
                Data = _positionGeometry,
                Fill = Brushes.Transparent,
                Stroke = Brushes.Red,
                StrokeThickness = 1
            };

            geometry.FillRule = FillRule.Nonzero;

            ClearWaveform();
            PART_Canvas.CacheMode = new BitmapCache(1.0);
            PART_Canvas.Children.Add(PART_Path);
            PART_Canvas.Children.Add(centerLinePath);
            PART_Canvas.Children.Add(positionPath);
        }

        public void UpdatePosition(double perc)
        {
            if (Dispatcher.CheckAccess() && _positionGeometry != null)
            {
                double x = perc * PART_Canvas.RenderSize.Width;
                var centerLineGeometry = (LineGeometry) ((Path) PART_Canvas.Children[1]).Data;
                x = centerLineGeometry.StartPoint.X + (perc * (centerLineGeometry.EndPoint - centerLineGeometry.StartPoint).X);

                _positionGeometry.StartPoint = new Point(x, 0);
                _positionGeometry.EndPoint = new Point(x, PART_Canvas.RenderSize.Height);
            }
            else
            {
                Dispatcher.InvokeAsync(() => UpdatePosition(perc));
            }
        }

        public void ClearWaveform()
        {
            PART_Canvas.Children.Clear();
        }

        protected override async void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            var cache = PART_Canvas.CacheMode as BitmapCache;
            if (cache != null)
            {
                cache.RenderAtScale = 1.0;
                await UpdateWaveformAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PART_Canvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var x = e.GetPosition(PART_Canvas).X;
            var perc = x / PART_Canvas.RenderSize.Width;
            if(PositionChanged != null)
                PositionChanged(this, new PositionChangedEventArgs(perc));
        }

        public event EventHandler<PositionChangedEventArgs> PositionChanged;
    }

    public class PositionChangedEventArgs : EventArgs
    {
        public PositionChangedEventArgs(double percentage)
        {
            Percentage = percentage;
        }

        public double Percentage { get; private set; }
    }
}
