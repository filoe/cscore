using SharpDX;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visualization3D.Core.Input
{
    public class Camera
    {
        private const float mouseSensitivity = 0.005f;
        private const float movementSpeed = 20f;

        private float _pitch, _yaw;
        private Vector3 _position;

        private Matrix _view;

        private InputManager _input;

        public Matrix View { get { return _view; } }

        public bool EnableMouse { get; set; }

        public bool Enabled { get; set; }

        public Camera(InputManager input)
        {
            _input = input;
            EnableMouse = true;
            Enabled = true;
        }

        public void Update(float time)
        {
            if (Enabled)
            {
                if (EnableMouse)
                    UpdateMouse(time);
                UpdateKeyboard(time);

                _view = Matrix.Translation(_position);
                //if (EnableMouse)
                //{
                _view = Matrix.RotationX(_pitch) * _view;
                _view = Matrix.RotationY(-_yaw) * _view;
                //}
            }
        }

        private void UpdateMouse(float time)
        {
            Yaw(_input.MouseDeltaX * mouseSensitivity);
            Pitch(-_input.MouseDeltaY * mouseSensitivity);
        }

        private void UpdateKeyboard(float time)
        {
            if (_input.IsKeyDown(Key.W))
                MoveForward(time * movementSpeed);
            if (_input.IsKeyDown(Key.S))
                MoveBackward(time * movementSpeed);
            if (_input.IsKeyDown(Key.A))
                MoveLeft(time * movementSpeed);
            if (_input.IsKeyDown(Key.D))
                MoveRight(time * movementSpeed);
            if (_input.IsKeyDown(Key.Q))
                _position.Y -= time * movementSpeed;
            if (_input.IsKeyDown(Key.E))
                _position.Y += time * movementSpeed;
        }

        public void MoveForward(float value)
        {
            float x = value * (float)Math.Sin(DegreeToRadian(_yaw));
            float z = value * (float)Math.Cos(DegreeToRadian(_yaw));
            Translate(+x, 0, -z);
        }

        public void MoveBackward(float value)
        {
            float x = value * (float)Math.Sin(DegreeToRadian(_yaw));
            float z = value * (float)Math.Cos(DegreeToRadian(_yaw));
            Translate(-x, 0, +z);
        }

        public void MoveLeft(float value)
        {
            float x = value * (float)Math.Sin(DegreeToRadian(_yaw - 90));
            float z = value * (float)Math.Cos(DegreeToRadian(_yaw - 90));
            Translate(-x, 0, z);
        }

        public void MoveRight(float value)
        {
            float x = value * (float)Math.Sin(DegreeToRadian(_yaw + 90));
            float z = value * (float)Math.Cos(DegreeToRadian(_yaw + 90));
            Translate(-x, 0, z);
        }

        public void Yaw(float angle)
        {
            //x
            _yaw += angle;
        }

        public void Pitch(float angle)
        {
            //y
            _pitch += angle;
        }

        public void Translate(float x, float y, float z)
        {
            _position.X += x;
            _position.Y += y;
            _position.Z += z;
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}