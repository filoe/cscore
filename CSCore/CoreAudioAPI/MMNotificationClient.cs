using CSCore.Utils;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="MMNotificationClient"/> object provides notifications when an audio endpoint device is added or removed, when the state or properties of an endpoint device change, or when there is a change in the default role assigned to an endpoint device.
    /// </summary>
    [Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0")]
    public sealed class MMNotificationClient : IMMNotificationClient, IDisposable
    {
        private readonly MMDeviceEnumerator _deviceEnumerator;
        private const string InterfaceName = "IMMNotificationClient";

        /// <summary>
        /// Occurs when the state of an audio endpoint device has changed.
        /// </summary>
        public event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged;

        /// <summary>
        /// Occurs when a new audio endpoint device has been added.
        /// </summary>
        public event EventHandler<DeviceNotificationEventArgs> DeviceAdded;

        /// <summary>
        /// Occurs when an audio endpoint device has been removed.
        /// </summary>
        public event EventHandler<DeviceNotificationEventArgs> DeviceRemoved;

        /// <summary>
        /// Occurs when the default audio endpoint device for a particular device role has changed.
        /// </summary>
        public event EventHandler<DefaultDeviceChangedEventArgs> DefaultDeviceChanged;

        /// <summary>
        /// Occurs when the value of a property belonging to an audio endpoint device has changed.
        /// </summary>
        public event EventHandler<DevicePropertyChangedEventArgs> DevicePropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MMNotificationClient"/> class.
        /// </summary>
        public MMNotificationClient()
        {
            _deviceEnumerator = new MMDeviceEnumerator();
            Initialize(_deviceEnumerator);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MMNotificationClient"/> class based on an existing <see cref="MMDeviceEnumerator"/>.
        /// </summary>
        /// <param name="enumerator"></param>
        public MMNotificationClient(MMDeviceEnumerator enumerator)
        {
            if (enumerator == null)
                throw new ArgumentNullException("enumerator");

            _deviceEnumerator = enumerator;
            Initialize(enumerator);
        }

        private void Initialize(MMDeviceEnumerator enumerator)
        {
            int result = enumerator.RegisterEndpointNotificationCallbackNative(this);
            CoreAudioAPIException.Try(result, "IMMDeviceEnumerator", "RegisterEndpointNotificationCallback");
        }

        /// <summary>
        /// The OnDeviceStateChanged method indicates that the state of an audio endpoint device has
        /// changed.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <param name="deviceState">Specifies the new state of the endpoint device.</param>
        /// <returns>HRESULT</returns>
        void IMMNotificationClient.OnDeviceStateChanged(string deviceId, DeviceState deviceState)
        {
            if (DeviceStateChanged != null)
                DeviceStateChanged(this, new DeviceStateChangedEventArgs(deviceId, deviceState));

            //return (int) HResult.S_OK;
        }

        /// <summary>
        /// The OnDeviceAdded method indicates that a new audio endpoint device has been added.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <returns>HRESULT</returns>
        void IMMNotificationClient.OnDeviceAdded(string deviceId)
        {
            if (DeviceAdded != null)
                DeviceAdded(this, new DeviceNotificationEventArgs(deviceId));

            //return (int) HResult.S_OK;
        }

        /// <summary>
        /// The OnDeviceRemoved method indicates that an audio endpoint device has been removed.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <returns>HRESULT</returns>
        void IMMNotificationClient.OnDeviceRemoved(string deviceId)
        {
            if (DeviceRemoved != null)
                DeviceRemoved(this, new DeviceNotificationEventArgs(deviceId));

            //return (int) HResult.S_OK;
        }

        /// <summary>
        /// The OnDefaultDeviceChanged method notifies the client that the default audio endpoint
        /// device for a particular device role has changed.
        /// </summary>
        /// <param name="dataFlow">The data-flow direction of the endpoint device.</param>
        /// <param name="role">The device role of the audio endpoint device.</param>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <returns>HRESULT</returns>
        void IMMNotificationClient.OnDefaultDeviceChanged(DataFlow dataFlow, Role role, string deviceId)
        {
            if (DefaultDeviceChanged != null)
                DefaultDeviceChanged(this, new DefaultDeviceChangedEventArgs(deviceId, dataFlow, role));

            //return (int) HResult.S_OK;
        }

        /// <summary>
        /// The OnPropertyValueChanged method indicates that the value of a property belonging to an
        /// audio endpoint device has changed.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <param name="key">The <see cref="Win32.PropertyKey"/> that specifies the changed property.</param>        
        /// <returns>HRESULT</returns>
        void IMMNotificationClient.OnPropertyValueChanged(string deviceId, PropertyKey key)
        {
            if (DevicePropertyChanged != null)
                DevicePropertyChanged(this, new DevicePropertyChangedEventArgs(deviceId, key));

            //return (int) HResult.S_OK;
        }

        private bool _disposed;

        /// <summary>
        /// Disposes und unregisters the <see cref="MMNotificationClient"/>.
        /// </summary>
        /// <remarks>In order to unregister the <see cref="MMNotificationClient"/>, this method calls the <see cref="MMDeviceEnumerator.UnregisterEndpointNotificationCallback"/> method.</remarks>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                try
                {
                    CoreAudioAPIException.Try(_deviceEnumerator.UnregisterEndpointNotificationCallbackNative(this),
                        InterfaceName, "UnregisterEndpointNotificationCallback");
                }
                finally
                {
                    _deviceEnumerator.Dispose();
                }
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MMNotificationClient"/> class.
        /// </summary>
        ~MMNotificationClient()
        {
            Dispose();
        }
    }
}