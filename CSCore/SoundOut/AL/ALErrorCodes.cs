using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.SoundOut.AL
{
    public enum ALErrorCode
    {
        /// <summary>
        /// No Error
        /// </summary>
        NoError = 0x0,

        /// <summary>
        /// Invalid Name
        /// </summary>
        InvalidName = 0xA001,

        /// <summary>
        /// Invalid Enum
        /// </summary>
        InvalidEnum = 0xA002,

        /// <summary>
        /// Invalid Value
        /// </summary>
        InvalidValue = 0xA003,

        /// <summary>
        /// Invalid Operation
        /// </summary>
        InvalidOperation = 0xA004,

        /// <summary>
        /// Out of Memory
        /// </summary>
        OutOfMemory = 0xA005
    }

    public enum ALCErrorCode
    {
        /// <summary>
        /// No Error
        /// </summary>
        NoError = 0x0,

        /// <summary>
        /// Invalid Device
        /// </summary>
        InvalidDevice = 0xA001,


        /// <summary>
        /// Invalid Context
        /// </summary>
        InvalidContext = 0xA002,

        /// <summary>
        /// Invalid Enum
        /// </summary>
        InvalidEnum = 0xA003,

        /// <summary>
        /// Invalid Value
        /// </summary>
        InvalidValue = 0xA004,

        /// <summary>
        /// Out of Memory
        /// </summary>
        OutOfMemory = 0xA005
    }
}
