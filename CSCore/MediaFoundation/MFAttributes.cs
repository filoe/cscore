// ReSharper disable InconsistentNaming
using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    //http://msdn.microsoft.com/en-gb/library/windows/desktop/ms704598%28v=vs.85%29.aspx
    /// <summary>
    /// Provides a generic way to store key/value pairs on an object. The keys are GUIDs, and the values can be any of the following data types: UINT32, UINT64, double, GUID, wide-character string, byte array, or IUnknown pointer. The standard implementation of this interface holds a thread lock while values are added, deleted, or retrieved.
    /// For a list of predefined attribute GUIDs, see Media Foundation Attributes. Each attribute GUID has an expected data type. The various "set" methods in IMFAttributes do not validate the type against the attribute GUID. It is the application's responsibility to set the correct type for the attribute.
    /// To create an empty attribute store, call MFCreateAttributes.
    /// </summary>
    [Guid("2cd2d921-c447-44a7-a13c-4adabfc247e3")]
    public class MFAttributes : ComObject
    {
        private const string InterfaceName = "IMFAttributes";

        /// <summary>
        /// Initializes a new instance of the <see cref="MFAttributes"/> class.
        /// </summary>
        /// <param name="ptr">The underlying native pointer.</param>
        public MFAttributes(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MFAttributes"/> class.
        /// </summary>
        public MFAttributes()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MFAttributes"/> class.
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
        /// Gets or sets an item specified by its index.
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
        /// Gets or sets an item specified by its key.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        public object this[Guid key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        /// <summary>
        /// Retrieves the number of attributes that are set on this object.
        /// </summary>
        public int Count
        {
            get
            {
                return GetCount();
            }
        }

        /// <summary>
        /// Retrieves the value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetItem(Guid key, IntPtr valueRef)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*)valueRef, ((void**)(*(void**)UnsafeBasePtr))[3]);
        }

        /// <summary>
        /// Retrieves the data type of the value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetItemType(Guid key, out MFAttributeType attributeType)
        {
            fixed (MFAttributeType* pat = &attributeType)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pat, ((void**)(*(void**)UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        /// Retrieves the data type of the value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public MFAttributeType GetItemType(Guid key)
        {
            MFAttributeType type;
            MediaFoundationException.Try(GetItemType(key, out type), InterfaceName, "GetItemType");
            return type;
        }

        /// <summary>
        /// Queries whether a stored attribute value equals a specified PROPVARIANT.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int CompareItem(Guid key, PropertyVariant value, out NativeBool result)
        {
            fixed (NativeBool* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &value, pr, ((void**)(*(void**)UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        /// Compares the attributes on this object with the attributes on another object.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int Compare(MFAttributes theirs, MFAttributeMatchType matchType, out NativeBool result)
        {
            fixed (NativeBool* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)((theirs == null) ? IntPtr.Zero : theirs.BasePtr), matchType, pr, ((void**)(*(void**)UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        /// Retrieves a UINT32 value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetUINT32(Guid key, out int result)
        {
            fixed (int* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**)(*(void**)UnsafeBasePtr))[7]);
            }
        }

        /// <summary>
        /// Retrieves a UINT32 value associated with a key.
        /// </summary>
        public int GetUINT32(Guid key)
        {
            int result;
            MediaFoundationException.Try(GetUINT32(key, out result), InterfaceName, "GetUINT32");
            return result;
        }

        /// <summary>
        /// Retrieves a UINT64 value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetUINT64(Guid key, out long result)
        {
            fixed (long* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**)(*(void**)UnsafeBasePtr))[8]);
            }
        }

        /// <summary>
        /// Retrieves a UINT64 value associated with a key.
        /// </summary>
        public long GetUINT64(Guid key)
        {
            long result;
            MediaFoundationException.Try(GetUINT64(key, out result), InterfaceName, "GetUINT64");
            return result;
        }

        /// <summary>
        /// Retrieves a double value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetDouble(Guid key, out double result)
        {
            fixed (double* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**)(*(void**)UnsafeBasePtr))[9]);
            }
        }

        /// <summary>
        /// Retrieves a double value associated with a key.
        /// </summary>
        public double GetDouble(Guid key)
        {
            double result;
            MediaFoundationException.Try(GetDouble(key, out result), InterfaceName, "GetDouble");
            return result;
        }

        /// <summary>
        /// Retrieves a GUID value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetGuid(Guid key, out Guid result)
        {
            fixed (Guid* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**)(*(void**)UnsafeBasePtr))[10]);
            }
        }

        /// <summary>
        /// Retrieves a GUID value associated with a key.
        /// </summary>
        public Guid GetGuid(Guid key)
        {
            Guid result;
            MediaFoundationException.Try(GetGuid(key, out result), InterfaceName, "GetGuid");
            return result;
        }

        /// <summary>
        /// Retrieves the length of a string value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetStringLength(Guid key, out int result)
        {
            fixed (int* pr = &result)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, pr, ((void**)(*(void**)UnsafeBasePtr))[11]);
            }
        }

        /// <summary>
        /// Retrieves the length of a string value associated with a key.
        /// </summary>
        public int GetStringLength(Guid key)
        {
            int result;
            MediaFoundationException.Try(GetStringLength(key, out result), InterfaceName, "GetStringLength");
            return result;
        }

        /// <summary>
        /// Retrieves a wide-character string associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetString(Guid key, IntPtr wszValue, int cchBufSize, IntPtr cchLength)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &wszValue, cchBufSize, &cchLength, ((void**)(*(void**)UnsafeBasePtr))[12]);
        }

        /// <summary>
        /// Retrieves a wide-character string associated with a key.
        /// </summary>
        public unsafe string GetString(Guid key)
        {
            int stringLength = GetStringLength(key);
            char* value = stackalloc char[stringLength + 1];
            var res = GetString(key, new IntPtr(value), stringLength + 1, IntPtr.Zero);
            MediaFoundationException.Try(res, InterfaceName, "GetString");
            return Marshal.PtrToStringUni(new IntPtr(value));
        }

        /// <summary>
        /// Retrieves a wide-character string associated with a key. This method allocates the
        /// memory for the string.
        /// </summary>
        /// <returns>HRESULT</returns>
        /// <remarks>
        /// If the key is found and the value is a string type, this parameter receives a copy of
        /// the string. The caller must free the memory for the string by calling CoTaskMemFree.
        /// </remarks>
        public unsafe int GetAllocatedString(Guid key, IntPtr wszValue, out int cchLength)
        {
            fixed (int* pcchl = (&cchLength))
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*)wszValue, (IntPtr*)pcchl, ((void**)(*(void**)UnsafeBasePtr))[13]);
            }
        }

        /// <summary>
        /// Retrieves the length of a byte array associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetBlobSize(Guid key, out int size)
        {
            fixed (int* psize = &size)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, psize, ((void**)(*(void**)UnsafeBasePtr))[14]);
            }
        }

        /// <summary>
        /// Retrieves the length of a byte array associated with a key.
        /// </summary>
        public int GetBlobSize(Guid key)
        {
            int result;
            MediaFoundationException.Try(GetBlobSize(key, out result), InterfaceName, "GetBlobSize");
            return result;
        }

        /// <summary>
        /// Retrieves a byte array associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetBlob(Guid key, IntPtr buf, int cbBufSize, IntPtr cbBlobSize)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*)buf, cbBufSize, (void*)cbBlobSize, ((void**)(*(void**)UnsafeBasePtr))[15]);
        }

        /// <summary>
        /// Retrieves a byte array associated with a key. This method allocates the memory for the
        /// array.
        /// </summary>
        /// <returns>HRESULT</returns>
        /// <remarks>
        /// If the key is found and the value is a byte array, this parameter receives a copy of the
        /// array. The caller must free the memory for the array by calling CoTaskMemFree.
        /// </remarks>
        public unsafe int GetAllocatedBlob(Guid key, out byte[] ip, out int pcbSize)
        {
            fixed (void* ppcbsize = &pcbSize)
            {
                IntPtr ptr = IntPtr.Zero;
                int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &ptr, ppcbsize, ((void**)(*(void**)UnsafeBasePtr))[16]);
                ip = new byte[pcbSize];
                Marshal.Copy(ptr, ip, 0, ip.Length);
                return result;
            }
        }

        /// <summary>
        /// Retrieves an interface pointer associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetUnknown(Guid key, Guid riid, out IntPtr unknown)
        {
            fixed (IntPtr* ptr = &unknown)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &riid, ptr, ((void**)(*(void**)UnsafeBasePtr))[17]);
            }
        }

        /// <summary>
        /// Associates an attribute value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetItemNative(Guid key, PropertyVariant value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &value, ((void**)(*(void**)UnsafeBasePtr))[18]);
        }

        /// <summary>
        /// Associates an attribute value with a key.
        /// </summary>
        public void SetItem(Guid key, PropertyVariant value)
        {
            MediaFoundationException.Try(SetItemNative(key, value), InterfaceName, "SetItem");
        }

        /// <summary>
        /// Removes a key/value pair from the object's attribute list.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int DeleteItem(Guid key)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, ((void**)(*(void**)UnsafeBasePtr))[19]);
        }

        /// <summary>
        /// Removes all key/value pairs from the object's attribute list.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int DeleteAllItems()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[20]);
        }

        /// <summary>
        /// Associates a UINT32 value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetUINT32(Guid key, int value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, value, ((void**)(*(void**)UnsafeBasePtr))[21]);
        }

        /// <summary>
        /// Associates a UINT64 value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetUINT64(Guid key, long value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, value, ((void**)(*(void**)UnsafeBasePtr))[22]);
        }

        /// <summary>
        /// Associates a double value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetDouble(Guid key, double value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, value, ((void**)(*(void**)UnsafeBasePtr))[23]);
        }

        /// <summary>
        /// Associates a GUID value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetGuid(Guid key, Guid value)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, &value, ((void**)(*(void**)UnsafeBasePtr))[24]);
        }

        /// <summary>
        /// Associates a wide-character string with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetString(Guid key, string value)
        {
            IntPtr intPtr = Marshal.StringToHGlobalUni(value);
            int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*)intPtr, ((void**)(*(void**)UnsafeBasePtr))[25]);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }

        /// <summary>
        /// Associates a byte array with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetBlob(Guid key, IntPtr buf, int cbBufSize)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, (void*)buf, cbBufSize, ((void**)(*(void**)UnsafeBasePtr))[26]);
        }

        /// <summary>
        /// Associates an IUnknown pointer with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetUnknown(Guid key, IntPtr unknown)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &key, unknown, ((void**)(*(void**)UnsafeBasePtr))[27]);
        }

        /// <summary>
        /// Locks the attribute store so that no other thread can access it.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int LockStore()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[28]);
        }

        /// <summary>
        /// Unlocks the attribute store.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int UnlockStore()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[29]);
        }

        /// <summary>
        /// Retrieves the number of attributes that are set on this object.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetCountNative(out int itemCount)
        {
            itemCount = -1;
            fixed (void* ptr = &itemCount)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[30]);
            }
        }

        /// <summary>
        /// Retrieves the number of attributes that are set on this object.
        /// </summary>
        public int GetCount()
        {
            int count;
            MediaFoundationException.Try(GetCountNative(out count), InterfaceName, "GetCount");
            return count;
        }

        /// <summary>
        /// Retrieves an attribute at the specified index.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetItemByIndexNative(int index, out Guid key, IntPtr value)
        {
            key = default(Guid);
            fixed (void* ptr = &key)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, index, new IntPtr(ptr), (void*)value, ((void**)(*(void**)UnsafeBasePtr))[31]);
            }
        }

        /// <summary>
        /// Retrieves an attribute at the specified index.
        /// </summary>
        public unsafe PropertyVariant GetItemByIndex(int index, out Guid key)
        {
            PropertyVariant value = default(PropertyVariant);
            MediaFoundationException.Try(GetItemByIndexNative(index, out key, new IntPtr(&value)), InterfaceName, "GetItemByIndex");
            return value;
        }

        /// <summary>
        /// Copies all of the attributes from this object into another attribute store.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int CopyAllItems(MFAttributes destination)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)((destination == null) ? IntPtr.Zero : destination.BasePtr), ((void**)(*(void**)UnsafeBasePtr))[32]);
        }

        /// <summary>
        /// Gets the item which got associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <returns>The item which got associated with the specified <paramref name="key"/>.</returns>
        /// <exception cref="NotSupportedException">The value type of the associated item is not supported.</exception>
        public object Get(Guid key)
        {
            var itemType = GetItemType(key);
            var mfAttributeType = itemType;
            if (mfAttributeType <= MFAttributeType.UInt64)
            {
                if (mfAttributeType == MFAttributeType.Double)
                {
                    return Get<double>(key);
                }
                if (mfAttributeType == MFAttributeType.IUnknown)
                {
                    return Get<ComObject>(key);
                }
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
                {
                    return Get<string>(key);
                }
                if (mfAttributeType == MFAttributeType.Guid)
                {
                    return Get<Guid>(key);
                }
                if (mfAttributeType == MFAttributeType.Blob)
                {
                    return Get<byte[]>(key);
                }
            }
            throw new NotSupportedException("The valuetype is not supported");
        }

        /// <summary>
        /// Gets the item which got associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <typeparam name="TValueType">Type of the returned item.</typeparam>
        /// <returns>The item which got associated with the specified <paramref name="key"/>.</returns>
        /// <exception cref="NotSupportedException">The specified <typeparamref name="TValueType"/> is not supported.</exception>
        public unsafe TValueType Get<TValueType>(Guid key)
        {
            if (typeof(TValueType) == typeof(int) || typeof(TValueType) == typeof(bool) || typeof(TValueType) == typeof(byte) || typeof(TValueType) == typeof(uint) || typeof(TValueType) == typeof(short) || typeof(TValueType) == typeof(ushort) || typeof(TValueType) == typeof(byte) || typeof(TValueType) == typeof(sbyte))
            {
                return (TValueType)Convert.ChangeType(GetUINT32(key), typeof(TValueType));
            }
            if (typeof(TValueType).IsEnum)
            {
                return (TValueType)Enum.ToObject(typeof(TValueType), GetUINT32(key));
            }
            if (typeof(TValueType) == typeof(IntPtr))
            {
                return (TValueType)((object)new IntPtr(GetUINT64(key)));
            }
            if (typeof(TValueType) == typeof(long) || typeof(TValueType) == typeof(ulong))
            {
                return (TValueType)Convert.ChangeType(GetUINT64(key), typeof(TValueType));
            }
            if (typeof(TValueType) == typeof(Guid))
            {
                return (TValueType)((object)GetGuid(key));
            }
            if (typeof(TValueType) == typeof(string))
            {
                return (TValueType)((object)GetString(key));
            }
            if (typeof(TValueType) == typeof(double) || typeof(TValueType) == typeof(float))
            {
                return (TValueType)Convert.ChangeType(GetDouble(key), typeof(TValueType));
            }
            if (typeof(TValueType) == typeof(byte[]))
            {
                int blobSize = GetBlobSize(key);
                byte[] array = new byte[blobSize];
                fixed (void* ptr = &array[0])
                {
                    GetBlob(key, (IntPtr)ptr, array.Length, IntPtr.Zero);
                }
                return (TValueType)((object)array);
            }
            if (typeof(TValueType).IsValueType)
            {
                TValueType result = default(TValueType);
                var h = GCHandle.Alloc(result, GCHandleType.Pinned);
                GetBlob(key, h.AddrOfPinnedObject(), Marshal.SizeOf(result), IntPtr.Zero);

                h.Free();

                return result;
            }
            if (typeof(TValueType) == typeof(ComObject))
            {
                IntPtr pointer;
                GetUnknown(key, typeof(IUnknown).GetGuid(), out pointer);
                return (TValueType)((object)new ComObject(pointer));
            }
            if (typeof(TValueType).IsSubclassOf(typeof(ComObject)))
            {
                IntPtr iunknownPtr;
                GetUnknown(key, typeof(TValueType).GetGuid(), out iunknownPtr);
                return new ComObject(iunknownPtr).QueryInterface1<TValueType>();
            }
            throw new NotSupportedException("The specified valuetype is not supported.");
        }

        /// <summary>
        /// Sets the value of a property specified by its <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the property.</param>
        /// <param name="value">The value to set.</param>
        /// <typeparam name="TValueType">The type of the property.</typeparam>
        /// <exception cref="NotSupportedException">The specified <typeparamref name="TValueType"/> is not supported.</exception>
        public unsafe void Set<TValueType>(Guid key, TValueType value)
        {
            if (typeof(TValueType) == typeof(int) || typeof(TValueType) == typeof(bool) || typeof(TValueType) == typeof(byte) || typeof(TValueType) == typeof(uint) || typeof(TValueType) == typeof(short) || typeof(TValueType) == typeof(ushort) || typeof(TValueType) == typeof(byte) || typeof(TValueType) == typeof(sbyte) || typeof(TValueType).IsEnum)
            {
                SetUINT32(key, Convert.ToInt32(value));
            }
            else if (typeof(TValueType) == typeof(long) || typeof(TValueType) == typeof(ulong))
            {
                SetUINT64(key, Convert.ToInt64(value));
            }
            else if (typeof(TValueType) == typeof(IntPtr))
            {
                SetUINT64(key, ((IntPtr)((object)value)).ToInt64());
            }
            else if (typeof(TValueType) == typeof(Guid))
            {
                SetGuid(key, (Guid)((object)value));
            }
            else if (typeof(TValueType) == typeof(string))
            {
                SetString(key, value.ToString());
            }
            else if (typeof(TValueType) == typeof(double) || typeof(TValueType) == typeof(float))
            {
                SetDouble(key, Convert.ToDouble(value));
            }
            else if (typeof(TValueType) == typeof(byte[]))
            {
                byte[] array = (byte[])((object)value);
                fixed (void* ptr = &array[0])
                {
                    SetBlob(key, (IntPtr)(ptr), array.Length);
                }
            }
            else if (typeof(TValueType).IsValueType)
            {
                var h = GCHandle.Alloc(value, GCHandleType.Pinned);
                SetBlob(key, h.AddrOfPinnedObject(), Marshal.SizeOf(value)); //todo: test
                h.Free();
            }
            else if (typeof(TValueType) == typeof(ComObject) || typeof(TValueType).IsSubclassOf(typeof(ComObject)))
            {
                Set(key, (ComObject)((object)value));
            }
            else
            {
                throw new ArgumentException("The type of the value is not supported");
            }
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
        /// Sets the value of a property specified by the key of the <paramref name="keyValuePair"/> object.
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