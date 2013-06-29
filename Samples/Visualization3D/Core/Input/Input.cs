using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.DirectInput;
using System.Windows.Forms;

namespace Visualization3D.Core.Input
{
    public class InputManager : IDisposable
    {
        DirectInput _directInput;
        Mouse _mouse;
        Keyboard _keyboard;

        KeyboardState _keyboardState;
        MouseState _mouseState;
        MouseState _prevmouseState;

        bool _enableMouse = true;
        public bool EnableMouse
        {
            get { return _enableMouse; }
            set
            {
                _enableMouse = value;
                if (!value)
                    ResetMouse();
            }
        }

        public void Load()
        {
            DirectInput directInput = new DirectInput();
            Mouse mouse = new Mouse(directInput);
            mouse.Acquire();
            Keyboard keyBoard = new Keyboard(directInput);
            keyBoard.Acquire();

            _directInput = directInput;
            _mouse = mouse;
            _keyboard = keyBoard;
        }

        public void Update(float time)
        {
            _keyboardState = _keyboard.GetCurrentState();

            if (EnableMouse)
            {
                _prevmouseState = _mouseState;
                _mouseState = _mouse.GetCurrentState();

                var position = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
                if (_prevmouseState != null)
                {
                    MouseDeltaX = _mouseState.X - _prevmouseState.X;
                    MouseDeltaY = _mouseState.Y - _prevmouseState.Y;
                }
                Cursor.Position = position;
                _mouseState = _mouse.GetCurrentState();

                //Cursor.Hide();
            }
            else
            {
                ResetMouse();
            }
        }

        public bool IsKeyDown(Key key)
        {
            if (_keyboardState != null)
                return _keyboardState.PressedKeys.Contains(key);
            return false;
        }

        public bool IsKeyUp(Key key)
        {
            return !IsKeyDown(key);
        }

        public int MouseDeltaX { get; protected set; }
        public int MouseDeltaY { get; protected set; }

        public void ResetMouse()
        {
            _mouseState = _prevmouseState = null;
            MouseDeltaX = 0;
            MouseDeltaY = 0;

            //Cursor.Show();
        }

        public void Dispose()
        {
            Dispose(true);
			GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
			if(disposing)
			{
				//dispose managed
                ResetMouse();
			}

            if(!_directInput.IsDisposed)
                _directInput.Dispose();
            if (!_keyboard.IsDisposed)
                _keyboard.Dispose();
            if (!_mouse.IsDisposed)
                _mouse.Dispose();
        }

        ~InputManager()
        {
            Dispose(false);
        }
    }
}
