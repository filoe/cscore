using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;
using CSCore.Utils;

namespace CSCore.CoreAudioAPI
{
	[Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0")]
	public class MMNotificationClient : IMMNotificationClient, IDisposable
	{
		const string c = "IMMNotificationClient";

		public event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged;
		public event EventHandler<DeviceNotificationEventArgs> DeviceAdded;
		public event EventHandler<DeviceNotificationEventArgs> DeviceRemoved;
		public event EventHandler<DefaultDeviceChangedEventArgs> DefaultDeviceChanged;
		public event EventHandler<DevicePropertyChanged> DevicePropertyChanged;

		public MMNotificationClient()
		{
			using(var e = new MMDeviceEnumerator())
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
			int result = enumerator.RegisterEndpointNotificationCallback(this as IMMNotificationClient);
			CoreAudioAPIException.Try(result, "IMMDeviceEnumerator", "RegisterEndpointNotificationCallback");
		}

		int IMMNotificationClient.OnDeviceStateChanged(string id, DeviceState state)
		{
			if(DeviceStateChanged != null)
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
				DevicePropertyChanged(this, new DevicePropertyChanged(id, key));

			return (int)HResult.S_OK;
		}

		private bool _disposed;
		public void Dispose()
		{
			if(!_disposed)
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
					CoreAudioAPIException.Try(enumerator.UnregisterEndpointNotificationCallback(this as IMMNotificationClient),
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

	public class DeviceNotificationEventArgs : EventArgs
	{
		public string DeviceID { get; private set; }
		public DeviceNotificationEventArgs(string deviceID)
		{
			if (String.IsNullOrEmpty(deviceID))
				throw new ArgumentNullException("deviceID");
			DeviceID = deviceID;
		}
	}

	public class DeviceStateChangedEventArgs : DeviceNotificationEventArgs
	{
		public DeviceState DeviceState { get; private set; }

		public DeviceStateChangedEventArgs(string deviceID, DeviceState deviceState)
			: base(deviceID)
		{
			DeviceState = deviceState;
		}
	}

	public class DefaultDeviceChangedEventArgs : DeviceNotificationEventArgs
	{
		public DataFlow DataFlow { get; private set; }
		public Role Role { get; private set; }

		public DefaultDeviceChangedEventArgs(string deviceID, DataFlow dataFlow, Role role)
			: base(deviceID)
		{
			DataFlow = dataFlow;
			Role = role;
		}
	}

	public class DevicePropertyChanged : DeviceNotificationEventArgs
	{
		public PropertyKey PropertyKey { get; private set; }

		public DevicePropertyChanged(string deviceID, PropertyKey propertyKey)
			: base(deviceID)
		{
		}
	}

	/// <summary>
	/// mmdeviceapi.h line 221
	/// </summary>
	[ComImport]
	[Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	public interface IMMNotificationClient
	{
		/// <summary>
		/// The OnDeviceStateChanged method indicates that the state of an audio endpoint device has changed.
		/// </summary>
		/// <returns>HRESULT</returns>
		int OnDeviceStateChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceID, DeviceState deviceState);
		/// <summary>
		/// The OnDeviceAdded method indicates that a new audio endpoint device has been added.
		/// </summary>
		/// <returns>HRESULT</returns>
		int OnDeviceAdded([In, MarshalAs(UnmanagedType.LPWStr)] string deviceID);
		/// <summary>
		/// The OnDeviceRemoved method indicates that an audio endpoint device has been removed.
		/// </summary>
		/// <returns>HRESULT</returns>
		int OnDeviceRemoved([In, MarshalAs(UnmanagedType.LPWStr)] string deviceID);
		/// <summary>
		/// The OnDefaultDeviceChanged method notifies the client that the default audio endpoint device for a particular device role has changed.
		/// </summary>
		/// <returns>HRESULT</returns>
		int OnDefaultDeviceChanged([In] DataFlow flow, [In] Role role, [In, MarshalAs(UnmanagedType.LPWStr)] string deviceID);
		/// <summary>
		/// The OnPropertyValueChanged method indicates that the value of a property belonging to an audio endpoint device has changed.
		/// </summary>
		/// <returns>HRESULT</returns>
		int OnPropertyValueChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceID, PropertyKey key);
	}
}
