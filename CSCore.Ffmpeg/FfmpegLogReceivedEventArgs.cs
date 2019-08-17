using System;
using System.Runtime.InteropServices;
using CSCore.Ffmpeg.Interops;

namespace CSCore.Ffmpeg
{
    /// <summary>
    /// Provides data for the <see cref="FfmpegUtils.FfmpegLogReceived"/> event.
    /// </summary>
    public class FfmpegLogReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the level of the message.
        /// </summary>
        /// <value>
        /// The level of the message.
        /// </value>
        public LogLevel Level { get; private set; }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName { get; private set; }

        /// <summary>
        /// Gets the item name of the class.
        /// </summary>
        /// <value>
        /// The item name of the class.
        /// </value>
        public string ItemName { get; private set; }

        /// <summary>
        /// Gets or sets the name of the parent log context class.
        /// </summary>
        /// <value>
        /// The name of the parent log context class. Might me empty.
        /// </value>
        public string ParentLogContextClassName { get; set; }

        /// <summary>
        /// Gets or sets the item name of the parent log context class.
        /// </summary>
        /// <value>
        /// The item name of the parent log context class. Might me empty.
        /// </value>
        public string ParentLogContextItemName { get; set; }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ItemNameFunc(IntPtr avClass);

        internal unsafe FfmpegLogReceivedEventArgs(AVClass? avClass, AVClass? parentLogContext, LogLevel level, string line, void* ptr, void* ptr1)
        {
            ItemNameFunc itemNameFunc;
            IntPtr strPtr;

            Message = line;
            Level = level;

            if (avClass != null)
            {
                AVClass avc = avClass.Value;

                ClassName = Marshal.PtrToStringAnsi((IntPtr)avc.class_name);
                if (avc.item_name != IntPtr.Zero)
                {
                    itemNameFunc = (ItemNameFunc) Marshal.GetDelegateForFunctionPointer(avc.item_name, typeof(ItemNameFunc));
                    strPtr = itemNameFunc((IntPtr)ptr);
                    if (strPtr != IntPtr.Zero)
                        ItemName = Marshal.PtrToStringAnsi(strPtr);
                }
            }
            if (parentLogContext != null)
            {
                AVClass pavc = parentLogContext.Value;

                ParentLogContextClassName = Marshal.PtrToStringAnsi((IntPtr)pavc.class_name);
                if (pavc.item_name != IntPtr.Zero)
                {
                    itemNameFunc = (ItemNameFunc) Marshal.GetDelegateForFunctionPointer(pavc.item_name, typeof(ItemNameFunc));
                    strPtr = itemNameFunc((IntPtr) ptr1);
                    if (strPtr != IntPtr.Zero)
                        ParentLogContextItemName = Marshal.PtrToStringAnsi(strPtr);
                }
            }
        }
    }
}