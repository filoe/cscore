namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="MMNotificationClient.DefaultDeviceChanged"/> event.
    /// </summary>
    public class DefaultDeviceChangedEventArgs : DeviceNotificationEventArgs
    {
        /// <summary>
        /// Gets the data-flow direction of the endpoint device.
        /// </summary>
        public DataFlow DataFlow { get; private set; }

        /// <summary>
        /// Gets the device role of the audio endpoint device.
        /// </summary>
        public Role Role { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDeviceChangedEventArgs"/> class.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <param name="dataFlow">The data-flow direction of the endpoint device.</param>
        /// <param name="role">The device role of the audio endpoint device.</param>
        public DefaultDeviceChangedEventArgs(string deviceId, DataFlow dataFlow, Role role)
            : base(deviceId)
        {
            DataFlow = dataFlow;
            Role = role;
        }
    }
}