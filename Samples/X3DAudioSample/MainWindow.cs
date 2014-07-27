using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CSCore.Codecs;
using CSCore.Utils;

namespace X3DAudioSample
{
    public partial class MainWindow : Form
    {
        public const double DurationPerPass = 5000; //in ms
        public const double Radius = 5000;
        private readonly AudioPlayer _audioPlayer = new AudioPlayer();
        private double _angle;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnPlayFile_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = CodecFactory.SupportedFilesFilterEn;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                updateTimer.Stop();
                _audioPlayer.OpenFile(openFileDialog.FileName);
                updateTimer.Start();
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            var center = new Vector3(0);

            _audioPlayer.EmitterPosition = CalculatePosition(center, _angle, Radius);
            _angle += (360 / DurationPerPass) * updateTimer.Interval;

            pictureBox.Invalidate();
        }

        private Rectangle GetRectangle(RectangleF bounds, Point centerPoint, double sizeDenominator)
        {
            double sizeX = (bounds.Width / sizeDenominator) / 2;
            double sizeY = (bounds.Height / sizeDenominator) / 2;

            var x = (int) (centerPoint.X - sizeX);
            var y = (int) (centerPoint.Y - sizeY);
            var width = (int) (2 * sizeX);
            var height = (int) (2 * sizeY);

            return new Rectangle(x, y, width, height);
        }

        private Vector3 CalculatePosition(Vector3 center, double angle, double radius)
        {
            double radians = (Math.PI / 180) * angle;
            var vector = new Vector3
            {
                X = (float) (center.X + radius * Math.Cos(radians)),
                Z = (float) (center.Z + radius * Math.Sin(radians)),
                Y = center.Y
            };

            return vector;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(SystemColors.Control);

            RectangleF rect = g.VisibleClipBounds;
            var centerPoint = new Point((int) (rect.Left + rect.Width / 2),
                (int) (rect.Top + rect.Height / 2));

            Rectangle centerRectangle = GetRectangle(rect, centerPoint, 20);
            g.FillEllipse(new SolidBrush(Color.Red), centerRectangle);

            double pixelPerUnitX = (rect.Width - (rect.Width / 30)) / 2 / Radius;
            double pixelPerUnitY = (rect.Height - (rect.Height/ 30)) / 2 / Radius;

            centerPoint = new Point((int) (centerPoint.X + pixelPerUnitX * _audioPlayer.EmitterPosition.X),
                (int) (centerPoint.Y + pixelPerUnitY * _audioPlayer.EmitterPosition.Z));
                //Z because X3DAudio uses a left-handed Cartesian coordinate system
            Rectangle pointRectangle = GetRectangle(rect, centerPoint, 30);
            g.FillEllipse(new SolidBrush(Color.Green), pointRectangle);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _audioPlayer.Stop();
        }
    }
}