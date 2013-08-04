using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Direct3D;
using SharpDX.Windows;
using System.Diagnostics;
using System.Windows.Forms;
using CSCore.Codecs;
using SharpDX.DirectInput;

using Visualization3D.Core;
using Visualization3D.Core.Graphics;
using Visualization3D.Core.Input;

namespace Visualization3D
{
    public class VisualizationRoot : RenderForm, IDisposable
    {
        const int bands = 128 * 4;

        InputManager _input;
        DeviceManager _deviceManager;
        Matrix view, proj;

        VisualisationItemManager<CubeItem> _cubeManager;
        Camera _camera;
        Clock _clock;
        AudioPlayer<CubeItem> _audioPlayer;
        bool _close = false;

        List<Key> _pressedKeys;

        bool _autoRotate = false;
        float _rotation = 0f;

        public VisualizationRoot()
        {
            _pressedKeys = new List<Key>();
            Shown += (s, e) => Focus();

            Text = String.Empty;
        }

        public void Run()
        {
            InitializeDirectX();

            LoadComponent();

            RenderLoop.Run(this, () =>
            {
                float elapsed = _clock.Update();

                Update(elapsed);
                Render(elapsed);

                if (_close)
                    Close();
            });

            Dispose();
        }

        protected void LoadComponent()
        {
            proj = Matrix.PerspectiveFovLH((float)Math.PI / 4f, ClientSize.Width / (float)ClientSize.Height, 0.1f, 1000f);

            _cubeManager = new VisualisationItemManager<CubeItem>(new Context() { DeviceManager = _deviceManager, Input = _input }, bands / 2);
            _cubeManager.Load();

            _clock = new Clock();
            _clock.Start();

            _camera = new Camera(_input);
            _camera.Translate(0, 0, 200);

            CustomLight light = new CustomLight(_deviceManager);
            light.Load();

            _audioPlayer = new AudioPlayer<CubeItem>(_cubeManager, bands);
        }

        public void Update(float time)
        {
            if (_input.IsKeyDown(Key.Escape) && Focused)
                _close = true;
            if (_input.IsKeyDown(Key.O) && Focused)
                OpenFile();
            if (CheckKeyPressed(Key.F1) && Focused)
                _camera.EnableMouse = !_camera.EnableMouse;
            if (CheckKeyPressed(Key.F2) && Focused)
                _autoRotate = !_autoRotate;

            _input.EnableMouse = Focused & _camera.EnableMouse;
            _camera.Enabled = Focused;

            _input.Update(time);
            _camera.Update(time);

            if (_autoRotate && _camera.EnableMouse == false)
                view = Matrix.RotationY((_rotation += (time * 0.2f)));
            else
            {
                _rotation = 0f;
                view = Matrix.Identity;
            }

            view *= _camera.View;
            _cubeManager.Update(time);
        }

        public void Render(float time)
        {
            var device = _deviceManager.Device;
            var manager = _cubeManager;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();

            manager.Render(view * proj, time);

            device.EndScene();
            device.Present();
        }

        private void OpenFile()
        {
            _input.EnableMouse = false;

            OpenFileDialog ofn = new OpenFileDialog();
            ofn.Filter = CodecFactory.SupportedFilesFilterEN;
            if (ofn.ShowDialog() == DialogResult.OK)
            {
                _audioPlayer.StartStream(new CSCore.Streams.LoopStream(CodecFactory.Instance.GetCodec(ofn.FileName)));
            }

            _input.EnableMouse = true;
        }

        private void InitializeDirectX()
        {
            _input = new InputManager();
            _input.Load();
            _input.EnableMouse = true;

            _deviceManager = new DeviceManager(new Direct3D());
            _deviceManager.InitializeDX(0, Handle, ClientSize.Width, ClientSize.Height);
        }

        private bool CheckKeyPressed(Key key)
        {
            if (_pressedKeys.Contains(key) && _input.IsKeyUp(key))
            {
                _pressedKeys.Remove(key);
            }
            else
            {
                if (_input.IsKeyDown(key) && !_pressedKeys.Contains(key))
                {
                    _pressedKeys.Add(key);
                    return true;
                }
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _audioPlayer.Dispose();
            _input.Dispose();

            _cubeManager.Dispose();
            _deviceManager.Dispose();
        }
    }
}
