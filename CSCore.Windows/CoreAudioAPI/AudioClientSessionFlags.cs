namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     The <see cref="AudioClientSessionFlags" /> constants indicate characteristics of an audio session associated with
    ///     the stream. A client can specify these options during the initialization of the stream by through the
    ///     <c>StreamFlags</c> parameter of the <see cref="AudioClient.Initialize" /> method.
    /// </summary>
    public enum AudioClientSessionFlags
    {
        /// <summary>
        ///     The session expires when there are no associated streams and owning session control objects holding references.
        /// </summary>
        SessionFlagsExpireWhenUnowned = 0x10000000,

        /// <summary>
        ///     The volume control is hidden in the volume mixer user interface when the audio session is created. If the session
        ///     associated with the stream already exists before <see cref="AudioClient.Initialize" /> opens the stream, the volume
        ///     control is displayed in the volume mixer.
        /// </summary>
        SessionFlagsDisplayHide = 0x20000000,

        /// <summary>
        ///     The volume control is hidden in the volume mixer user interface after the session expires.
        /// </summary>
        SessionFlagsDisplayHideWhenExpired = 0x40000000
    }
}