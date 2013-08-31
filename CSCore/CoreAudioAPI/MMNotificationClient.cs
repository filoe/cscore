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
    [Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0")]
    public class MMNotificationClient : IMMNotificationClient, IDisposable
    {
        private const string c = "IMMNotificationClient";

        public event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged;

        public event EventHandler<DeviceNotificationEventArgs> DeviceAdded;

        public event EventHandler<DeviceNotificationEventArgs> DeviceRemoved;

        public event EventHandler<DefaultDeviceChangedEventArgs> DefaultDeviceChanged;

        public event EventHandler<DevicePropertyChangedEventArgs> DevicePropertyChanged;

        public MMNotificationClient()
        {
            using (var e = new MMDeviceEnumerator())
            {
                Initialize(e);
            }
        }

        public MMNotificationClient(MMDeviceEnumerator enumerator)
        {
            if (enumerator == null)
                throw new ArgumentNullException("enumerator");

            Initialize(enumerator);
        }

        private void Initialize(MMDeviceEnumerator enumerator)
        {
            int result = enumerator.RegisterEndpointNotificationCallbackNative(this as IMMNotificationClient);
            CoreAudioAPIException.Try(result, "IMMDeviceEnumerator", "RegisterEndpointNotificationCallback");
        }

        int IMMNotificationClient.OnDeviceStateChanged(string id, DeviceState state)
        {
            if (DeviceStateChanged != null)
                DeviceStateChanged(this, new DeviceStateChangedEventArgs(id, state));

            return (int)HResult.S_OK;
        }

        int IMMNotificationClient.OnDeviceAdded(string id)
        {
            if (DeviceAdded != null)
                DeviceAdded(this, new DeviceNotificationEventArgs(id));

            return (int)HResult.S_OK;
        }

        int IMMNotificationClient.OnDeviceRemoved(string id)
        {
            if (DeviceRemoved != null)
                DeviceRemoved(this, new DeviceNotificationEventArgs(id));

            return (int)HResult.S_OK;
        }

        int IMMNotificationClient.OnDefaultDeviceChanged(DataFlow flow, Role role, string id)
        {
            if (DefaultDeviceChanged != null)
                DefaultDeviceChanged(this, new DefaultDeviceChangedEventArgs(id, flow, role));

            return (int)HResult.S_OK;
        }

        int IMMNotificationClient.OnPropertyValueChanged(string id, PropertyKey key)
        {
            if (DevicePropertyChanged != null)
                DevicePropertyChanged(this, new DevicePropertyChangedEventArgs(id, key));

            return (int)HResult.S_OK;
        }

        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.Assert(disposing == true, "MMNotificationClient not disposed.");
            try
            {
                using (MMDeviceEnumerator enumerator = new MMDeviceEnumerator())
                {
                    CoreAudioAPIException.Try(enumerator.UnregisterEndpointNotificationCallbackNative(this as IMMNotificationClient),
                        c, "UnregisterEndpointNotificationCallback");
                }
            }
            catch (CoreAudioAPIException)
            {
                throw; //0x80070490
            }
        }

        ~MMNotificationClient()
        {
            Dispose(false);
        }
    }
}