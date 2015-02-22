using System;
using System.Runtime.Serialization;

namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// FLAC Exception.
    /// </summary>
    [Serializable]
    public class FlacException : Exception
    {
        /// <summary>
        /// Gets the layer of the flac stream the exception got thrown.
        /// </summary>
        /// <remarks>Used for debugging purposes.</remarks>
        public FlacLayer Layer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacException"/> class.
        /// </summary>
        /// <param name="message">A message which describes the error.</param>
        /// <param name="layer">The layer of the flac stream the exception got thrown.</param>
        public FlacException(string message, FlacLayer layer)
            : base(message)
        {
            Layer = layer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacException"/> class.
        /// </summary>
        /// <param name="innerException">The InnerException which caused the error.</param>
        /// <param name="layer">The layer.The layer of the flac stream the exception got thrown.</param>
        public FlacException(Exception innerException, FlacLayer layer)
            : base("See InnerException for more details.", innerException)
        {
            Layer = layer;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlacException" /> class from serialization data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> object that holds the serialized object data.</param>
        /// <param name="context">
        ///     The StreamingContext object that supplies the contextual information about the source or
        ///     destination.
        /// </param>
        protected FlacException(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            Layer = (FlacLayer) info.GetValue("Layer", typeof (FlacLayer));
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Layer", Layer);
        }
    }
}