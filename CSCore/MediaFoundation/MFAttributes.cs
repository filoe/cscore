// ReSharper disable InconsistentNaming

using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    //http://msdn.microsoft.com/en-gb/library/windows/desktop/ms704598%28v=vs.85%29.aspx
    /// <summary>
    ///     Provides a generic way to store key/value pairs on an object.
    /// </summary>
    [Guid("2cd2d921-c447-44a7-a13c-4adabfc247e3")]
    public class MFAttributes : ComObject
    {
        private const string InterfaceName = "IMFAttributes";

        /// <summary>
        ///     Initializes a new instance of the <see cref="MFAttributes" /> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public MFAttributes(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MFAttributes" /> class.
        /// </summary>
        public MFAttributes()
            : this(0)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MFAttributes" /> class with a initial size.
        /// </summary>
        /// <param name="initialSize">The initial size in bytes.</param>
        public unsafe MFAttributes(int initialSize)
        {
            IntPtr zero = IntPtr.Zero;
            int result = NativeMethods.MFCreateAttributes_(new IntPtr(&zero), initialSize);
            MediaFoundationException.Try(result, "Interop", "MFCreateAttributes");
            UnsafeBasePtr = zero.ToPointer();
        }

        /// <summary>
        ///     Gets or sets an item specified by its index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        public PropertyVariant this[int index]
        {
            get
            {
                Guid key;
                return GetItemByIndex(index, out key);
            }
            set
            {
                Guid key;
                GetItemByIndex(index, out key);
                SetItem(key, value);
            }
        }

        /// <summary>
        ///     Gets or sets an item specified by its key.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        public object this[Guid key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        /// <summary>
        ///     Gets the number of attributes that are set on this object.
        /// </summary>
        public int Count
        {
            get { return GetCount(); }
        }

        /// <summary>
        ///     Retrieves the value associated with a key.
        /// </summary>
        /// <param name="key">A <see cref="Guid"/> that identifies which value to retrieve.</param>
        /// <param name="valueRef">A pointer to a <see cref="PropertyVariant" /> that receives the value.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb970450(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int GetItemNative(Guid key, IntPtr valueRef)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*) valueRef,
                ((void**) (*(void**) UnsafeBasePtr))[3]);
        }

        /// <summary>
        ///     Retrieves the value associated with a key.
        /// </summary>
        /// <param name="key">A <see cref="Guid"/> that identifies which value to retrieve.</param>
        /// <returns>A <see cref="PropertyVariant" /> that receives the value.</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb970450(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe PropertyVariant GetItem(Guid key)
        {
            var propertyVariant = new PropertyVariant();
            MediaFoundationException.Try(GetItemNative(key, (IntPtr) (&propertyVariant)), InterfaceName, "GetItem");
            return propertyVariant;
        }

        /// <summary>
        ///     Retrieves the data type of the value associated with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to query.</param>
        /// <param name="attributeType">The type of the item, associated with the specified <paramref name="key" />.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb970369(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int GetItemTypeNative(Guid key, out MFAttributeType attributeType)
        {
            fixed (MFAttributeType* pat = &attributeType)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pat, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        ///     Retrieves the data type of the value associated with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to query.</param>
        /// <returns>The type of the item, associated with the specified <paramref name="key" />.</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb970369(v=vs.85).aspx" />.
        /// </remarks>
        public MFAttributeType GetItemType(Guid key)
        {
            MFAttributeType type;
            MediaFoundationException.Try(GetItemTypeNative(key, out type), InterfaceName, "GetItemType");
            return type;
        }

        /// <summary>
        ///     Queries whether a stored attribute value equals a specified <see cref="PropertyVariant" />.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to query.</param>
        /// <param name="value"><see cref="PropertyVariant" /> that contains the value to compare.</param>
        /// <param name="result">
        ///     Receives a boolean value indicating whether the attribute matches the value given in
        ///     <paramref name="value" />.
        /// </param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/de-de/library/windows/desktop/bb970566(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int CompareItemNative(Guid key, PropertyVariant value, out NativeBool result)
        {
            fixed (NativeBool* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &value, pr,
                    ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        ///     Queries whether a stored attribute value equals a specified <see cref="PropertyVariant" />.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to query.</param>
        /// <param name="value"><see cref="PropertyVariant" /> that contains the value to compare.</param>
        /// <returns>A boolean value indicating whether the attribute matches the value given in <paramref name="value" />.</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/de-de/library/windows/desktop/bb970566(v=vs.85).aspx" />.
        /// </remarks>
        public bool CompareItem(Guid key, PropertyVariant value)
        {
            NativeBool result;
            MediaFoundationException.Try(CompareItemNative(key, value, out result), InterfaceName, "CompareItem");
            return result;
        }

        /// <summary>
        ///     Compares the attributes on this object with the attributes on another object.
        /// </summary>
        /// <param name="theirs">The <see cref="MFAttributes" /> interface of the object to compare with this object.</param>
        /// <param name="matchType">A value, specifying the type of comparison to make.</param>
        /// <param name="result">
        ///     Receives a Boolean value. The value is <see cref="NativeBool.True" /> if the two sets of
        ///     attributes match in the way specified by the <paramref name="matchType" /> parameter. Otherwise, the value is
        ///     <see cref="NativeBool.False" />.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int CompareNative(MFAttributes theirs, MFAttributeMatchType matchType, out NativeBool result)
        {
            fixed (NativeBool* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr,
                    (void*) ((theirs == null) ? IntPtr.Zero : theirs.BasePtr), matchType, pr,
                    ((void**) (*(void**) UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        ///     Compares the attributes on this object with the attributes on another object.
        /// </summary>
        /// <param name="theirs">The <see cref="MFAttributes" /> interface of the object to compare with this object.</param>
        /// <param name="matchType">A value, specifying the type of comparison to make.</param>
        /// <returns>
        ///     Returns <c>true</c> if the two sets of attributes match in the way specified by the
        ///     <paramref name="matchType" /> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Compare(MFAttributes theirs, MFAttributeMatchType matchType)
        {
            NativeBool result;
            MediaFoundationException.Try(CompareNative(theirs, matchType, out result), InterfaceName, "Compare");
            return result;
        }

        /// <summary>
        ///     Retrieves a UINT32 value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.UInt32" />.
        /// </param>
        /// <param name="result">
        ///     Receives a UINT32 value. If the key is found and the data type is <see cref="MFAttributeType.UInt32" />, the method
        ///     copies the
        ///     value into this parameter.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetUINT32Native(Guid key, out int result)
        {
            fixed (int* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**) (*(void**) UnsafeBasePtr))[7]);
            }
        }

        /// <summary>
        ///     Retrieves a UINT32 value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.UInt32" />.
        /// </param>
        /// <returns>
        ///     If the key is found and the data type is <see cref="MFAttributeType.UInt32" />, the method returns the
        ///     associated value.
        /// </returns>
        public int GetUINT32(Guid key)
        {
            int result;
            MediaFoundationException.Try(GetUINT32Native(key, out result), InterfaceName, "GetUINT32");
            return result;
        }

        /// <summary>
        ///     Retrieves a UINT64 value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.UInt64" />.
        /// </param>
        /// <param name="result">
        ///     Receives a UINT64 value. If the key is found and the data type is <see cref="MFAttributeType.UInt64" />, the method
        ///     copies the
        ///     value into this parameter.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetUINT64Native(Guid key, out long result)
        {
            fixed (long* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**) (*(void**) UnsafeBasePtr))[8]);
            }
        }

        /// <summary>
        ///     Retrieves a UINT64 value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.UInt64" />.
        /// </param>
        /// <returns>
        ///     If the key is found and the data type is <see cref="MFAttributeType.UInt64" />, the method returns the
        ///     associated value.
        /// </returns>
        public long GetUINT64(Guid key)
        {
            long result;
            MediaFoundationException.Try(GetUINT64Native(key, out result), InterfaceName, "GetUINT64");
            return result;
        }

        /// <summary>
        ///     Retrieves a Double value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.Double" />.
        /// </param>
        /// <param name="result">
        ///     Receives a Double value. If the key is found and the data type is <see cref="MFAttributeType.Double" />, the method
        ///     copies the
        ///     value into this parameter.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetDoubleNative(Guid key, out double result)
        {
            fixed (double* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**) (*(void**) UnsafeBasePtr))[9]);
            }
        }

        /// <summary>
        ///     Retrieves a Double value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.Double" />.
        /// </param>
        /// <returns>
        ///     If the key is found and the data type is <see cref="MFAttributeType.Double" />, the method returns the
        ///     associated value.
        /// </returns>
        public double GetDouble(Guid key)
        {
            double result;
            MediaFoundationException.Try(GetDoubleNative(key, out result), InterfaceName, "GetDouble");
            return result;
        }

        /// <summary>
        ///     Retrieves a Guid value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.Guid" />.
        /// </param>
        /// <param name="result">
        ///     Receives a Guid value. If the key is found and the data type is <see cref="MFAttributeType.Guid" />, the method
        ///     copies the
        ///     value into this parameter.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetGuidNative(Guid key, out Guid result)
        {
            fixed (Guid* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**) (*(void**) UnsafeBasePtr))[10]);
            }
        }

        /// <summary>
        ///     Retrieves a Guid value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.Guid" />.
        /// </param>
        /// <returns>
        ///     If the key is found and the data type is <see cref="MFAttributeType.Guid" />, the method returns the
        ///     associated value.
        /// </returns>
        public Guid GetGuid(Guid key)
        {
            Guid result;
            MediaFoundationException.Try(GetGuidNative(key, out result), InterfaceName, "GetGuid");
            return result;
        }

        /// <summary>
        ///     Retrieves the length of a string value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.String" />.
        /// </param>
        /// <param name="result">
        ///     If the key is found and the data type is <see cref="MFAttributeType.String" />, 
        ///     this parameter receives the number of characters in the string, not including the terminating NULL character.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetStringLengthNative(Guid key, out int result)
        {
            fixed (int* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**) (*(void**) UnsafeBasePtr))[11]);
            }
        }

        /// <summary>
        ///     Retrieves the length of a string value associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.String" />.
        /// </param>
        /// <returns>If the key is found and the data type is <see cref="MFAttributeType.String" />, 
        ///     this method returns the number of characters in the string, not including the terminating NULL character.</returns>
        public int GetStringLength(Guid key)
        {
            int result;
            MediaFoundationException.Try(GetStringLengthNative(key, out result), InterfaceName, "GetStringLength");
            return result;
        }

        /// <summary>
        ///     Retrieves a wide-character string associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.String" />.
        /// </param>
        /// <param name="wszValue">
        /// Pointer to a wide-character array allocated by the caller. 
        /// The array must be large enough to hold the string, including the terminating NULL character. 
        /// If the key is found and the value is a string type, the method copies the string into this buffer.
        /// To find the length of the string, call <see cref="GetStringLength"/>.
        /// </param>
        /// <param name="cchBufSize">The size of the pwszValue array, in characters. This value includes the terminating NULL character.</param>
        /// <param name="cchLength">Receives the number of characters in the string, excluding the terminating NULL character. This parameter can be NULL.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetStringNative(Guid key, IntPtr wszValue, int cchBufSize, IntPtr cchLength)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*)wszValue, cchBufSize, &cchLength,
                ((void**) (*(void**) UnsafeBasePtr))[12]);
        }

        /// <summary>
        ///     Retrieves a wide-character string associated with a key.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.String" />.
        /// </param>
        /// <returns>
        ///     If the key is found and the data type is <see cref="MFAttributeType.String" />, the method returns the
        ///     associated value.
        /// </returns>
        public unsafe string GetString(Guid key)
        {
            int stringLength = GetStringLength(key);
            char* value = stackalloc char[stringLength + 1];
            int res = GetStringNative(key, new IntPtr(value), stringLength + 1, IntPtr.Zero);
            MediaFoundationException.Try(res, InterfaceName, "GetString");
            return Marshal.PtrToStringUni(new IntPtr(value));
        }

        /// <summary>
        ///     Retrieves a wide-character string associated with a key. This method allocates the
        ///     memory for the string.
        /// </summary>
        /// <param name="key">
        ///     <see cref="Guid"/> that identifies which value to retrieve. The attribute type must be
        ///     <see cref="MFAttributeType.String" />.
        /// </param>
        /// <param name="wszValue">
        /// If the key is found and the value is a string type, this parameter receives a copy of the string. The caller must free the memory for the string by calling <see cref="Marshal.FreeCoTaskMem"/>.
        /// </param>
        /// <param name="cchLength">
        /// Receives the number of characters in the string, excluding the terminating NULL character.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        /// Don't use the <see cref="GetAllocatedStringNative"/> method. Use the <see cref="GetString(System.Guid)"/> method instead.
        /// </remarks>
        [Obsolete("Use the GetString method instead.")]
        public unsafe int GetAllocatedStringNative(Guid key, IntPtr wszValue, out int cchLength)
        {
            fixed (int* pcchl = (&cchLength))
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*) wszValue, (IntPtr*) pcchl,
                    ((void**) (*(void**) UnsafeBasePtr))[13]);
            }
        }

        /// <summary>
        ///     Retrieves the length of a byte array associated with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to retrieve. The attribute type must be <see cref="MFAttributeType.Blob"/>.</param>
        /// <param name="size">If the key is found and the value is a byte array, this parameter receives the size of the array, in bytes.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetBlobSizeNative(Guid key, out int size)
        {
            fixed (int* psize = &size)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, psize, ((void**) (*(void**) UnsafeBasePtr))[14]);
            }
        }

        /// <summary>
        ///     Retrieves the length of a byte array associated with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to retrieve. The attribute type must be <see cref="MFAttributeType.Blob"/>.</param>
        /// <returns>If the key is found and the value is a byte array, this method returns the size of the array, in bytes.</returns>
        public int GetBlobSize(Guid key)
        {
            int result;
            MediaFoundationException.Try(GetBlobSizeNative(key, out result), InterfaceName, "GetBlobSize");
            return result;
        }

        /// <summary>
        ///     Retrieves a byte array associated with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to retrieve. The attribute type must be <see cref="MFAttributeType.Blob"/>.</param>
        /// <param name="bufferPtr">Pointer to a buffer allocated by the caller. If the key is found and the value is a byte array, the method copies the array into this buffer. To find the required size of the buffer, call <see cref="GetBlobSize"/>.</param>
        /// <param name="cbBufSize">The size of the <paramref name="bufferPtr"/> buffer, in bytes.</param>
        /// <param name="cbBlobSize">Receives the size of the byte array. This parameter can be <see cref="IntPtr.Zero"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetBlobNative(Guid key, IntPtr bufferPtr, int cbBufSize, IntPtr cbBlobSize)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*) bufferPtr, cbBufSize, (void*) cbBlobSize,
                ((void**) (*(void**) UnsafeBasePtr))[15]);
        }

        /// <summary>
        ///     Retrieves a byte array associated with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to retrieve. The attribute type must be <see cref="MFAttributeType.Blob"/>.</param>
        /// <returns>The byte array associated with the <paramref name="key"/>.</returns>
        public unsafe byte[] GetBlob(Guid key)
        {
            int blobSize = GetBlobSize(key);
            var array = new byte[blobSize];
            fixed (void* ptr = &array[0])
            {
                int result = GetBlobNative(key, (IntPtr)ptr, array.Length, IntPtr.Zero);
                MediaFoundationException.Try(result, InterfaceName, "GetBlob");
            }
            return array;
        }

        
        /// <summary>
        /// Retrieves an object associated with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to retrieve. The attribute type must be <see cref="MFAttributeType.Blob"/>.</param>
        /// <param name="type">The type of the object (type of the returned object -> see return value).</param>
        /// <returns>The object associated with the <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        /// <remarks>Internally this method retrieves a byte-array with gets converted to a instance of the specified <paramref name="type"/>.</remarks>
        public object GetBlob(Guid key, Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            int size = Marshal.SizeOf(type);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                GetBlobNative(key, ptr, size, IntPtr.Zero);
                return Marshal.PtrToStructure(ptr, type);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        ///     Retrieves a byte array associated with a key. 
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to retrieve. The attribute type must be <see cref="MFAttributeType.Blob"/>.</param>
        /// <param name="buffer">If the key is found and the value is a byte array, this parameter receives a copy of the array.</param>        
        /// <param name="pcbSize">Receives the size of the array, in bytes.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        /// Obsolete, use the <see cref="GetBlob(System.Guid)"/> method instead.
        /// </remarks>
        [Obsolete("Use the GetBlob-method instead.")]
        public unsafe int GetAllocatedBlobNative(Guid key, out byte[] buffer, out int pcbSize)
        {
            fixed (void* ppcbsize = &pcbSize)
            {
                IntPtr ptr = IntPtr.Zero;
                int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &ptr, ppcbsize,
                    ((void**) (*(void**) UnsafeBasePtr))[16]);
                try
                {
                    buffer = new byte[pcbSize];
                    Marshal.Copy(ptr, buffer, 0, buffer.Length);
                }
                finally
                {
                    Marshal.FreeCoTaskMem(ptr);
                }
                return result;
            }
        }

        /// <summary>
        ///     Retrieves an interface pointer associated with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies which value to retrieve. The attribute type must be <see cref="MFAttributeType.IUnknown"/>.</param>
        /// <param name="riid">Interface identifier (IID) of the interface to retrieve.</param>
        /// <param name="unknown">Receives a pointer to the requested interface. The caller must release the interface.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetUnknownNative(Guid key, Guid riid, out IntPtr unknown)
        {
            fixed (IntPtr* ptr = &unknown)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &riid, ptr,
                    ((void**) (*(void**) UnsafeBasePtr))[17]);
            }
        }

        /// <summary>
        ///     Associates an attribute value with a key.
        /// </summary>
        /// <param name="key">A <see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">A <see cref="PropertyVariant"/> that contains the attribute value. The method copies the value. The <see cref="PropertyVariant"/> type must be one of the types listed in the <see cref="MFAttributeType"/> enumeration.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetItemNative(Guid key, PropertyVariant value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &value, ((void**) (*(void**) UnsafeBasePtr))[18]);
        }

        /// <summary>
        ///     Associates an attribute value with a key.
        /// </summary>
        /// <param name="key">A <see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">A <see cref="PropertyVariant"/> that contains the attribute value. The method copies the value. The <see cref="PropertyVariant"/> type must be one of the types listed in the <see cref="MFAttributeType"/> enumeration.</param>
        public void SetItem(Guid key, PropertyVariant value)
        {
            MediaFoundationException.Try(SetItemNative(key, value), InterfaceName, "SetItem");
        }

        /// <summary>
        ///     Removes a key/value pair from the object's attribute list.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to delete.</param>
        /// <returns>HRESULT</returns>
        public unsafe int DeleteItemNative(Guid key)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, ((void**) (*(void**) UnsafeBasePtr))[19]);
        }

        /// <summary>
        ///     Removes a key/value pair from the object's attribute list.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to delete.</param>
        public void DeleteItem(Guid key)
        {
            MediaFoundationException.Try(DeleteItemNative(key), InterfaceName, "DeleteItem");
        }

        /// <summary>
        ///     Removes all key/value pairs from the object's attribute list.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int DeleteAllItemsNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[20]);
        }

        /// <summary>
        ///     Removes all key/value pairs from the object's attribute list.
        /// </summary>
        public void DeleteAllItems()
        {
            MediaFoundationException.Try(DeleteAllItemsNative(), InterfaceName, "DeleteAllItems");
        }

        /// <summary>
        ///     Associates a UINT32 value with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetUINT32Native(Guid key, int value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, value, ((void**) (*(void**) UnsafeBasePtr))[21]);
        }

        /// <summary>
        ///     Associates a UINT32 value with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        public void SetUINT32(Guid key, int value)
        {
            MediaFoundationException.Try(SetUINT32Native(key, value), InterfaceName, "SetUINT32");   
        }

        /// <summary>
        ///     Associates a UINT64 value with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetUINT64Native(Guid key, long value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, value, ((void**) (*(void**) UnsafeBasePtr))[22]);
        }

        /// <summary>
        ///     Associates a UINT64 value with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        public void SetUINT64(Guid key, long value)
        {
            MediaFoundationException.Try(SetUINT64Native(key, value), InterfaceName, "SetUINT64");
        }

        /// <summary>
        ///     Associates a Double value with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetDoubleNative(Guid key, double value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, value, ((void**) (*(void**) UnsafeBasePtr))[23]);
        }

        /// <summary>
        ///     Associates a Double value with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        public void SetDouble(Guid key, double value)
        {
            MediaFoundationException.Try(SetDoubleNative(key, value), InterfaceName, "SetDouble");
        }

        /// <summary>
        ///     Associates a <see cref="Guid"/> value with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetGuidNative(Guid key, Guid value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &value, ((void**) (*(void**) UnsafeBasePtr))[24]);
        }

        /// <summary>
        ///     Associates a <see cref="Guid"/> value with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        public void SetGuid(Guid key, Guid value)
        {
            MediaFoundationException.Try(SetGuidNative(key, value), InterfaceName, "SetGuid");
        }

        /// <summary>
        ///     Associates a wide-character string with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>Internally this method stores a copy of the string specified by the <paramref name="value"/> parameter.</remarks>
        public unsafe int SetStringNative(Guid key, string value)
        {
            IntPtr intPtr = Marshal.StringToHGlobalUni(value);
            int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*) intPtr,
                ((void**) (*(void**) UnsafeBasePtr))[25]);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }

        /// <summary>
        ///     Associates a wide-character string with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="value">New value for this key.</param>
        /// <remarks>Internally this method stores a copy of the string specified by the <paramref name="value"/> parameter.</remarks>
        public void SetString(Guid key, string value)
        {
            MediaFoundationException.Try(SetStringNative(key, value), InterfaceName, "SetString");
        }

        /// <summary>
        ///     Associates a byte array with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="buf">Pointer to a byte array to associate with this key. The method stores a copy of the array.</param>
        /// <param name="cbBufSize">Size of the array, in bytes.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetBlobNative(Guid key, IntPtr buf, int cbBufSize)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*) buf, cbBufSize,
                ((void**) (*(void**) UnsafeBasePtr))[26]);
        }
        /// <summary>
        ///     Associates a byte array with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="buffer">The byte array to associate with the <paramref name="key"/></param>
        public unsafe void SetBlob(Guid key, byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            fixed (void* p = &buffer[0])
            {
                int result = SetBlobNative(key, (IntPtr) p, buffer.Length);
                MediaFoundationException.Try(result, InterfaceName, "SetBlob");
            }
        }

        /// <summary>
        ///     Associates an IUnknown pointer with a key.
        /// </summary>
        /// <param name="key"><see cref="Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value.</param>
        /// <param name="unknown">IUnknown pointer to be associated with this key.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetUnknownNative(Guid key, IntPtr unknown)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, unknown, ((void**) (*(void**) UnsafeBasePtr))[27]);
        }

        /// <summary>
        ///     Locks the attribute store so that no other thread can access it. If the attribute store is already locked by another thread, this method blocks until the other thread unlocks the object. After calling this method, call <see cref="UnlockStore"/> to unlock the object.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int LockStoreNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[28]);
        }

        /// <summary>
        ///     Locks the attribute store so that no other thread can access it. If the attribute store is already locked by another thread, this method blocks until the other thread unlocks the object. After calling this method, call <see cref="UnlockStore"/> to unlock the object.
        /// </summary>
        public void LockStore()
        {
            MediaFoundationException.Try(LockStoreNative(), InterfaceName, "LockStore");
        }

        /// <summary>
        ///     Unlocks the attribute store after a call to the <see cref="LockStore"/> method. While the object is unlocked, multiple threads can access the object's attributes.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int UnlockStoreNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[29]);
        }

        /// <summary>
        ///     Unlocks the attribute store after a call to the <see cref="LockStore"/> method. While the object is unlocked, multiple threads can access the object's attributes.
        /// </summary>
        public void UnlockStore()
        {
            MediaFoundationException.Try(UnlockStoreNative(), InterfaceName, "UnlockStore");    
        }

        /// <summary>
        ///     Retrieves the number of attributes that are set on this object.
        /// </summary>
        /// <param name="itemCount">Receives the number of attributes.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetCountNative(out int itemCount)
        {
            itemCount = -1;
            fixed (void* ptr = &itemCount)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**) (*(void**) UnsafeBasePtr))[30]);
            }
        }

        /// <summary>
        ///     Retrieves the number of attributes that are set on this object.
        /// </summary>
        /// <returns>Returns the number of attributes.</returns>
        public int GetCount()
        {
            int count;
            MediaFoundationException.Try(GetCountNative(out count), InterfaceName, "GetCount");
            return count;
        }

        /// <summary>
        ///     Retrieves an attribute at the specified index.
        /// </summary>
        /// <param name="index">Index of the attribute to retrieve. To get the number of attributes, call <see cref="GetCount"/>.</param>
        /// <param name="key">Receives the <see cref="Guid"/> that identifies this attribute.</param>
        /// <param name="value">Pointer to a <see cref="PropertyVariant"/> that receives the value. This parameter can be <see cref="IntPtr.Zero"/>. If it is not <see cref="IntPtr.Zero"/>, the method fills the <see cref="PropertyVariant"/> with a copy of the attribute value. Call <see cref="PropertyVariant.Dispose"/> to free the memory allocated by this method.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetItemByIndexNative(int index, out Guid key, IntPtr value)
        {
            key = default(Guid);
            fixed (void* ptr = &key)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, index, new IntPtr(ptr), (void*) value,
                    ((void**) (*(void**) UnsafeBasePtr))[31]);
            }
        }

        /// <summary>
        ///     Retrieves an attribute at the specified index.
        /// </summary>
        /// <param name="index">Index of the attribute to retrieve. To get the number of attributes, call <see cref="GetCount"/>.</param>
        /// <param name="key">Receives the <see cref="Guid"/> that identifies this attribute.</param>
        /// <returns>Returns the value of the attribute specified by the <paramref name="index"/>.</returns>
        public unsafe PropertyVariant GetItemByIndex(int index, out Guid key)
        {
            PropertyVariant value = default(PropertyVariant);
            try
            {
                MediaFoundationException.Try(GetItemByIndexNative(index, out key, new IntPtr(&value)), InterfaceName,
                    "GetItemByIndex");
                var copy = value;
                return copy;
            }
            finally
            {
                //value.Dispose();
            }
        }

        /// <summary>
        ///     Copies all of the attributes from this object into another attribute store.
        /// </summary>
        /// <param name="destination">The attribute store that recevies the copy.</param>
        /// <returns>HRESULT</returns>
        public unsafe int CopyAllItemsNative(MFAttributes destination)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr,
                (void*) ((destination == null) ? IntPtr.Zero : destination.BasePtr),
                ((void**) (*(void**) UnsafeBasePtr))[32]);
        }

        /// <summary>
        ///     Copies all of the attributes from this object into another attribute store.
        /// </summary>
        /// <param name="destination">The attribute store that recevies the copy.</param>
        public void CopyAllItems(MFAttributes destination)
        {
            MediaFoundationException.Try(CopyAllItemsNative(destination), InterfaceName, "CopyAllItems");
        }

        /// <summary>
        /// Determines whether the attribute store contains an attribute with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the attribute.</param>
        /// <returns><c>True</c> if the attribute exists; otherwise, <c>false</c></returns>
        /// <exception cref="MediaFoundationException">An unexpected error occurred.</exception>
        public bool Exists(Guid key)
        {
            int result = GetItemNative(key, IntPtr.Zero);
            if (result == (int) HResult.S_OK)
                return true;
            if (result == (int) HResult.MF_E_ATTRIBUTENOTFOUND)
                return false;
            throw new MediaFoundationException(result, InterfaceName, "GetItem");
        }

        /// <summary>
        ///     Gets the item which got associated with the specified <paramref name="key" />.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <returns>The item which got associated with the specified <paramref name="key" />.</returns>
        /// <exception cref="NotSupportedException">The value type of the associated item is not supported.</exception>
        public object Get(Guid key)
        {
            MFAttributeType itemType = GetItemType(key);
            MFAttributeType mfAttributeType = itemType;
            if (mfAttributeType <= MFAttributeType.UInt64)
            {
                if (mfAttributeType == MFAttributeType.Double)
                    return Get<double>(key);
                if (mfAttributeType == MFAttributeType.IUnknown)
                    return Get<ComObject>(key);
                switch (mfAttributeType)
                {
                    case MFAttributeType.UInt32:
                        return Get<int>(key);

                    case MFAttributeType.UInt64:
                        return Get<long>(key);
                }
            }
            else
            {
                if (mfAttributeType == MFAttributeType.String)
                    return Get<string>(key);
                if (mfAttributeType == MFAttributeType.Guid)
                    return Get<Guid>(key);
                if (mfAttributeType == MFAttributeType.Blob)
                    return Get<byte[]>(key);
            }
            throw new NotSupportedException("The valuetype is not supported.");
        }

        /// <summary>
        ///     Gets the item which got associated with the specified <paramref name="key" />.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <typeparam name="TValueType">Type of the returned item.</typeparam>
        /// <returns>The item which got associated with the specified <paramref name="key" />.</returns>
        /// <exception cref="NotSupportedException">The specified <typeparamref name="TValueType" /> is not supported.</exception>
        public TValueType Get<TValueType>(Guid key)
        {
            if (typeof (TValueType) == typeof (int) || typeof (TValueType) == typeof (bool) ||
                typeof (TValueType) == typeof (byte) || typeof (TValueType) == typeof (uint) ||
                typeof (TValueType) == typeof (short) || typeof (TValueType) == typeof (ushort) ||
                typeof (TValueType) == typeof (byte) || typeof (TValueType) == typeof (sbyte))
                return (TValueType) Convert.ChangeType(GetUINT32(key), typeof (TValueType));
            if (typeof (TValueType).IsEnum)
                return (TValueType) Enum.ToObject(typeof (TValueType), GetUINT32(key));
            if (typeof (TValueType) == typeof (IntPtr))
                return (TValueType) ((object) new IntPtr(GetUINT64(key)));
            if (typeof (TValueType) == typeof (long) || typeof (TValueType) == typeof (ulong))
                return (TValueType) Convert.ChangeType(GetUINT64(key), typeof (TValueType));
            if (typeof (TValueType) == typeof (Guid))
                return (TValueType) ((object) GetGuid(key));
            if (typeof (TValueType) == typeof (string))
                return (TValueType) ((object) GetString(key));
            if (typeof (TValueType) == typeof (double) || typeof (TValueType) == typeof (float))
                return (TValueType) Convert.ChangeType(GetDouble(key), typeof (TValueType));
            if (typeof (TValueType) == typeof (byte[]))
            {
                return (TValueType)(object)GetBlob(key);
            }
            if (typeof (TValueType).IsValueType)
            {
                return (TValueType) GetBlob(key, typeof (TValueType));
            }
            //currently not supported
            /*if (typeof (TValueType) == typeof (ComObject))
            {
                IntPtr pointer;
                GetUnknown(key, typeof (IUnknown).GetGuid(), out pointer);
                return (TValueType) ((object) new ComObject(pointer));
            }
            if (typeof (TValueType).IsSubclassOf(typeof (ComObject)))
            {
                IntPtr iunknownPtr;
                GetUnknown(key, typeof(TValueType).<see cref="Guid"/>, out iunknownPtr);
                return new ComObject(iunknownPtr).QueryInterface1<TValueType>();
            }*/
            throw new NotSupportedException("The specified valuetype is not supported.");
        }

        /// <summary>
        ///     Sets the value of a property specified by its <paramref name="key" />.
        /// </summary>
        /// <param name="key">The key of the property.</param>
        /// <param name="value">The value to set.</param>
        /// <typeparam name="TValueType">The type of the property.</typeparam>
        /// <exception cref="NotSupportedException">The specified <typeparamref name="TValueType" /> is not supported.</exception>
        public void Set<TValueType>(Guid key, TValueType value)
        {
            if (typeof (TValueType) == typeof (int) || typeof (TValueType) == typeof (bool) ||
                typeof (TValueType) == typeof (byte) || typeof (TValueType) == typeof (uint) ||
                typeof (TValueType) == typeof (short) || typeof (TValueType) == typeof (ushort) ||
                typeof (TValueType) == typeof (byte) || typeof (TValueType) == typeof (sbyte) ||
                typeof (TValueType).IsEnum)
                SetUINT32(key, Convert.ToInt32(value));
            else if (typeof (TValueType) == typeof (long) || typeof (TValueType) == typeof (ulong))
                SetUINT64(key, Convert.ToInt64(value));
            else if (typeof (TValueType) == typeof (IntPtr))
                SetUINT64(key, ((IntPtr) ((object) value)).ToInt64());
            else if (typeof (TValueType) == typeof (Guid))
                SetGuid(key, (Guid) ((object) value));
            else if (typeof (TValueType) == typeof (string))
                SetString(key, value.ToString());
            else if (typeof (TValueType) == typeof (double) || typeof (TValueType) == typeof (float))
                SetDouble(key, Convert.ToDouble(value));
            else if (typeof (TValueType) == typeof (byte[]))
            {
                SetBlob(key, (byte[])(object)value);
                //var array = (byte[]) ((object) value);
                //fixed (void* ptr = &array[0])
                //{
                //    SetBlob(key, (IntPtr) (ptr), array.Length);
                //}
            }
            else if (typeof (TValueType).IsValueType)
            {
                int size = Marshal.SizeOf(value);
                IntPtr ptr = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.StructureToPtr(value, ptr, true);
                    MediaFoundationException.Try(SetBlobNative(key, ptr, size), InterfaceName, "SetBlob");
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            /*else if (typeof (TValueType) == typeof (ComObject) || typeof (TValueType).IsSubclassOf(typeof (ComObject)))
            {
                var ptr = ((ComObject) (object) value).BasePtr;
                SetUnknown(key, new IntPtr(&ptr));
            }*/
            else
                throw new ArgumentException("The type of the value is not supported");
        }

        internal bool TryGet<T>(Guid key, out T value)
        {
            try
            {
                value = Get<T>(key);
                return true;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }

        /// <summary>
        ///     Sets the value of a property specified by the key of the <paramref name="keyValuePair" /> object.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="keyValuePair">Specifies the key of the property and the new value to set.</param>
        public void Set<T>(MFAttribute<T> keyValuePair)
        {
            if (keyValuePair == null)
                throw new ArgumentNullException("keyValuePair");
            Set(keyValuePair.Key, keyValuePair.Value);
        }
    }
}

// ReSharper restore InconsistentNaming