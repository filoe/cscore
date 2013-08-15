using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSCore.Visualization.WPF.Utils
{
    public class PixelManipulationBitmap
    {
        private WriteableBitmap _bitmap;

        private Int32Rect _updateRegion;
        private object _lockObj;

        public int Height
        {
            get { return _bitmap.PixelHeight; }
        }

        public int Width
        {
            get { return _bitmap.PixelWidth; }
        }

        public PixelManipulationBitmap(int width, int height)
            : this(width, height, PixelFormats.Rgb24)
        {
        }

        public PixelManipulationBitmap(int width, int height, PixelFormat format)
            : this(width, height, 96, 96, format)
        {
        }

        public PixelManipulationBitmap(int width, int height, int dpiX, int dpiY, PixelFormat format)
            : this(width, height, dpiX, dpiY, format, null)
        {
        }

        public PixelManipulationBitmap(int width, int height, int dpiX, int dpiY, PixelFormat format, BitmapPalette palette)
        {
            _bitmap = new WriteableBitmap(width, height, dpiX, dpiY, format, palette);
            _lockObj = new object();
        }

        public void BeginRender()
        {
            lock (_lockObj)
            {
                _bitmap.Lock();
                _updateRegion = new Int32Rect();
            }
        }

        public BitmapSource EndRender()
        {
            lock (_lockObj)
            {
                _bitmap.AddDirtyRect(_updateRegion);
                _bitmap.Unlock();
                return _bitmap;
            }
        }

        public BitmapSource GetBitmap()
        {
            return _bitmap;
        }

        public unsafe void SetPixel(int x, int y, Color color)
        {
            SetPixel(x, y, color.R, color.G, color.B);
        }

        public unsafe void SetPixel(int x, int y, params byte[] buffer)
        {
            if (!(x < _bitmap.PixelWidth && x >= 0))
                throw new ArgumentOutOfRangeException("x");
            if (!(y < _bitmap.PixelHeight && y >= 0))
                throw new ArgumentOutOfRangeException("y");
            if (buffer == null) throw new ArgumentNullException("buffer");

            _updateRegion.X = Math.Min(_updateRegion.X, x);
            _updateRegion.Y = Math.Min(_updateRegion.Y, y);
            int width = x - _updateRegion.X;
            int height = y - _updateRegion.Y;

            _updateRegion.Width = Math.Max(_updateRegion.Width, width);
            _updateRegion.Height = Math.Max(_updateRegion.Height, height);

            int offset = y * _bitmap.BackBufferStride + x * (_bitmap.Format.BitsPerPixel / 8);
            byte* ptr = ((byte*)_bitmap.BackBuffer.ToPointer()) + offset;
            for (int i = 0; i < buffer.Length; i++)
            {
                *(ptr++) = buffer[i];
            }
        }

        public unsafe void Clear(Color color)
        {
            for (int i = 0; i < _bitmap.PixelWidth; i++)
            {
                for (int j = 0; j < _bitmap.PixelHeight; j++)
                {
                    SetPixel(i, j, color);
                }
            }
        }
    }
}