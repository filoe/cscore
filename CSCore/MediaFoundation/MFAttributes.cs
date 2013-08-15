using CSCore.CoreAudioAPI;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

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
        [SuppressUnmanagedCodeSecurity]
        [DllImport("Mfplat.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "MFCreateAttributes")]
        private unsafe static extern int MFCreateAttributes_(IntPtr ptr, int initialSize);

        private const string c = "IMFAttributes";

        public MFAttributes(IntPtr ptr)
            : base(ptr)
        {
        }

        public MFAttributes()
            : this(0)
        {
        }

        public unsafe MFAttributes(int initialSize)
        {
            IntPtr zero = IntPtr.Zero;
            int result = MFCreateAttributes_(new IntPtr((void*)(&zero)), initialSize);
            MediaFoundationException.Try(result, "interop", "MFCreateAttributes");
            _basePtr = zero.ToPointer();
        }

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
            return InteropCalls.CalliMethodPtr(_basePtr, &key, (void*)valueRef, ((void**)(*(void**)_basePtr))[3]);
        }

        /// <summary>
        /// Retrieves the data type of the value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetItemType(Guid key, out MFAttributeType attributeType)
        {
            fixed (MFAttributeType* pat = &attributeType)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, &key, pat, ((void**)(*(void**)_basePtr))[4]);
            }
        }

        /// <summary>
        /// Retrieves the data type of the value associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public MFAttributeType GetItemType(Guid key)
        {
            MFAttributeType type;
            MediaFoundationException.Try(GetItemType(key, out type), c, "GetItemType");
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
                return InteropCalls.CalliMethodPtr(_basePtr, &key, &value, pr, ((void**)(*(void**)_basePtr))[5]);
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
                return InteropCalls.CalliMethodPtr(_basePtr, (void*)((theirs == null) ? IntPtr.Zero : theirs.BasePtr), matchType, pr, ((void**)(*(void**)_basePtr))[6]);
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
                return InteropCalls.CalliMethodPtr(_basePtr, &key, pr, ((void**)(*(void**)_basePtr))[7]);
            }
        }

        /// <summary>
        /// Retrieves a UINT32 value associated with a key.
        /// </summary>
        public int GetUINT32(Guid key)
        {
            int result;
            MediaFoundationException.Try(GetUINT32(key, out result), c, "GetUINT32");
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
                return InteropCalls.CalliMethodPtr(_basePtr, &key, pr, ((void**)(*(void**)_basePtr))[8]);
            }
        }

        /// <summary>
        /// Retrieves a UINT64 value associated with a key.
        /// </summary>
        public long GetUINT64(Guid key)
        {
            long result;
            MediaFoundationException.Try(GetUINT64(key, out result), c, "GetUINT64");
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
                return InteropCalls.CalliMethodPtr(_basePtr, &key, pr, ((void**)(*(void**)_basePtr))[9]);
            }
        }

        /// <summary>
        /// Retrieves a double value associated with a key.
        /// </summary>
        public double GetDouble(Guid key)
        {
            double result;
            MediaFoundationException.Try(GetDouble(key, out result), c, "GetDouble");
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
                return InteropCalls.CalliMethodPtr(_basePtr, &key, pr, ((void**)(*(void**)_basePtr))[10]);
            }
        }

        /// <summary>
        /// Retrieves a GUID value associated with a key.
        /// </summary>
        public Guid GetGuid(Guid key)
        {
            Guid result;
            MediaFoundationException.Try(GetGuid(key, out result), c, "GetGuid");
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
                return InteropCalls.CalliMethodPtr(_basePtr, &key, pr, ((void**)(*(void**)_basePtr))[11]);
            }
        }

        /// <summary>
        /// Retrieves the length of a string value associated with a key.
        /// </summary>
        public int GetStringLength(Guid key)
        {
            int result;
            MediaFoundationException.Try(GetStringLength(key, out result), c, "GetStringLength");
            return result;
        }

        /// <summary>
        /// Retrieves a wide-character string associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetString(Guid key, IntPtr wszValue, int cchBufSize, IntPtr cchLength)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, &wszValue, cchBufSize, &cchLength, ((void**)(*(void**)_basePtr))[12]);
        }

        /// <summary>
        /// Retrieves a wide-character string associated with a key.
        /// </summary>
        public unsafe string GetString(Guid key)
        {
            int stringLength = GetStringLength(key);
            char* value = stackalloc char[(int)(stringLength + 1)];
            var res = GetString(key, new IntPtr((void*)value), stringLength + 1, IntPtr.Zero);
            MediaFoundationException.Try(res, c, "GetString");
            return Marshal.PtrToStringUni(new IntPtr((void*)value));
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
                return InteropCalls.CalliMethodPtr(_basePtr, &key, (void*)wszValue, (IntPtr*)pcchl, ((void**)(*(void**)_basePtr))[13]);
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
                return InteropCalls.CalliMethodPtr(_basePtr, &key, psize, ((void**)(*(void**)_basePtr))[14]);
            }
        }

        /// <summary>
        /// Retrieves the length of a byte array associated with a key.
        /// </summary>
        public int GetBlobSize(Guid key)
        {
            int result;
            MediaFoundationException.Try(GetBlobSize(key, out result), c, "GetBlobSize");
            return result;
        }

        /// <summary>
        /// Retrieves a byte array associated with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetBlob(Guid key, IntPtr buf, int cbBufSize, IntPtr cbBlobSize)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, (void*)buf, cbBufSize, (void*)cbBlobSize, ((void**)(*(void**)_basePtr))[15]);
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
                int result = InteropCalls.CalliMethodPtr(_basePtr, &key, &ptr, ppcbsize, ((void**)(*(void**)_basePtr))[16]);
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
                return InteropCalls.CalliMethodPtr(_basePtr, &key, &riid, ptr, ((void**)(*(void**)_basePtr))[17]);
            }
        }

        /// <summary>
        /// Associates an attribute value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetItemNative(Guid key, PropertyVariant value)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, &value, ((void**)(*(void**)_basePtr))[18]);
        }

        /// <summary>
        /// Associates an attribute value with a key.
        /// </summary>
        public void SetItem(Guid key, PropertyVariant value)
        {
            MediaFoundationException.Try(SetItemNative(key, value), c, "SetItem");
        }

        /// <summary>
        /// Removes a key/value pair from the object's attribute list.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int DeleteItem(Guid key)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, ((void**)(*(void**)_basePtr))[19]);
        }

        /// <summary>
        /// Removes all key/value pairs from the object's attribute list.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int DeleteAllItems()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[20]);
        }

        /// <summary>
        /// Associates a UINT32 value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetUINT32(Guid key, int value)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, value, ((void**)(*(void**)_basePtr))[21]);
        }

        /// <summary>
        /// Associates a UINT64 value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetUINT64(Guid key, long value)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, value, ((void**)(*(void**)_basePtr))[22]);
        }

        /// <summary>
        /// Associates a double value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetDouble(Guid key, double value)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, value, ((void**)(*(void**)_basePtr))[23]);
        }

        /// <summary>
        /// Associates a GUID value with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetGuid(Guid key, Guid value)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, &value, ((void**)(*(void**)_basePtr))[24]);
        }

        /// <summary>
        /// Associates a wide-character string with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetString(Guid key, string value)
        {
            IntPtr intPtr = Marshal.StringToHGlobalUni(value);
            int result = InteropCalls.CalliMethodPtr(_basePtr, &key, (void*)intPtr, ((void**)(*(void**)_basePtr))[25]);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }

        /// <summary>
        /// Associates a byte array with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetBlob(Guid key, IntPtr buf, int cbBufSize)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, (void*)buf, cbBufSize, ((void**)(*(void**)_basePtr))[26]);
        }

        /// <summary>
        /// Associates an IUnknown pointer with a key.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetUnknown(Guid key, IntPtr unknown)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &key, unknown, ((void**)(*(void**)_basePtr))[27]);
        }

        /// <summary>
        /// Locks the attribute store so that no other thread can access it.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int LockStore()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[28]);
        }

        /// <summary>
        /// Unlocks the attribute store.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int UnlockStore()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[29]);
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
                return InteropCalls.CalliMethodPtr(_basePtr, ptr, ((void**)(*(void**)_basePtr))[30]);
            }
        }

        /// <summary>
        /// Retrieves the number of attributes that are set on this object.
        /// </summary>
        public int GetCount()
        {
            int count;
            MediaFoundationException.Try(GetCountNative(out count), c, "GetCount");
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
                return InteropCalls.CalliMethodPtr(_basePtr, index, new IntPtr(ptr), (void*)value, ((void**)(*(void**)_basePtr))[31]);
            }
        }

        /// <summary>
        /// Retrieves an attribute at the specified index.
        /// </summary>
        public unsafe PropertyVariant GetItemByIndex(int index, out Guid key)
        {
            PropertyVariant value = default(PropertyVariant);
            MediaFoundationException.Try(GetItemByIndexNative(index, out key, new IntPtr(&value)), c, "GetItemByIndex");
            return value;
        }

        /// <summary>
        /// Copies all of the attributes from this object into another attribute store.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int CopyAllItems(MFAttributes destination)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, (void*)((destination == null) ? IntPtr.Zero : destination.BasePtr), ((void**)(*(void**)_basePtr))[32]);
        }

        public object Get(Guid key)
        {
            var itemType = GetItemType(key);
            var MFAttributeType = itemType;
            if (MFAttributeType <= MFAttributeType.UInt64)
            {
                if (MFAttributeType == MFAttributeType.Double)
                {
                    return this.Get<double>(key);
                }
                if (MFAttributeType == MFAttributeType.IUnknown)
                {
                    return this.Get<ComObject>(key);
                }
                switch (MFAttributeType)
                {
                    case MFAttributeType.UInt32:
                        return this.Get<int>(key);

                    case MFAttributeType.UInt64:
                        return this.Get<long>(key);
                }
            }
            else
            {
                if (MFAttributeType == MFAttributeType.String)
                {
                    return this.Get<string>(key);
                }
                if (MFAttributeType == MFAttributeType.Guid)
                {
                    return this.Get<Guid>(key);
                }
                if (MFAttributeType == MFAttributeType.Blob)
                {
                    return this.Get<byte[]>(key);
                }
            }
            throw new ArgumentException("Valuetype is not supported");
        }

        public unsafe T Get<T>(Guid key)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(bool) || typeof(T) == typeof(byte) || typeof(T) == typeof(uint) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) || typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
            {
                return (T)((object)Convert.ChangeType(GetUINT32(key), typeof(T)));
            }
            if (typeof(T).IsEnum)
            {
                return (T)((object)Enum.ToObject(typeof(T), GetUINT32(key)));
            }
            if (typeof(T) == typeof(IntPtr))
            {
                return (T)((object)new IntPtr(GetUINT64(key)));
            }
            if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong))
            {
                return (T)((object)Convert.ChangeType(this.GetUINT64(key), typeof(T)));
            }
            if (typeof(T) == typeof(Guid))
            {
                return (T)((object)GetGuid(key));
            }
            if (typeof(T) == typeof(string))
            {
                return (T)((object)GetString(key));
            }
            if (typeof(T) == typeof(double) || typeof(T) == typeof(float))
            {
                return (T)((object)Convert.ChangeType(this.GetDouble(key), typeof(T)));
            }
            if (typeof(T) == typeof(byte[]))
            {
                int blobSize = this.GetBlobSize(key);
                byte[] array = new byte[blobSize];
                fixed (void* ptr = &array[0])
                {
                    this.GetBlob(key, (IntPtr)ptr, array.Length, IntPtr.Zero);
                }
                return (T)((object)array);
            }
            if (typeof(T).IsValueType)
            {
                T result = default(T);
                var h = GCHandle.Alloc(result, GCHandleType.Pinned);

                int blobSize2 = this.GetBlobSize(key);
                this.GetBlob(key, h.AddrOfPinnedObject(), Marshal.SizeOf(result), IntPtr.Zero);

                h.Free();

                return result;
            }
            else
            {
                if (typeof(T) == typeof(ComObject))
                {
                    IntPtr pointer;
                    this.GetUnknown(key, typeof(IUnknown).GetGuid(), out pointer);
                    return (T)((object)new ComObject(pointer));
                }
                if (typeof(T).IsSubclassOf(typeof(ComObject)))
                {
                    IntPtr iunknownPtr;
                    this.GetUnknown(key, typeof(T).GetGuid(), out iunknownPtr);
                    return (T)((object)new ComObject(iunknownPtr).QueryInterface1<T>());
                }
                throw new ArgumentException("The type of the value is not supported");
            }
        }

        public unsafe void Set<T>(Guid key, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(bool) || typeof(T) == typeof(byte) || typeof(T) == typeof(uint) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) || typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte) || typeof(T).IsEnum)
            {
                SetUINT32(key, Convert.ToInt32(value));
            }
            else if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong))
            {
                SetUINT64(key, Convert.ToInt64(value));
            }
            else if (typeof(T) == typeof(IntPtr))
            {
                SetUINT64(key, ((IntPtr)((object)value)).ToInt64());
            }
            else if (typeof(T) == typeof(Guid))
            {
                SetGuid(key, (Guid)((object)value));
            }
            else if (typeof(T) == typeof(string))
            {
                SetString(key, value.ToString());
            }
            else if (typeof(T) == typeof(double) || typeof(T) == typeof(float))
            {
                SetDouble(key, Convert.ToDouble(value));
            }
            else if (typeof(T) == typeof(byte[]))
            {
                byte[] array = (byte[])((object)value);
                fixed (void* ptr = &array[0])
                {
                    SetBlob(key, (IntPtr)(ptr), array.Length);
                }
            }
            else if (typeof(T).IsValueType)
            {
                var h = GCHandle.Alloc(value, GCHandleType.Pinned);
                SetBlob(key, h.AddrOfPinnedObject(), Marshal.SizeOf(value)); //todo: test
                h.Free();
            }
            else if (typeof(T) == typeof(ComObject) || typeof(T).IsSubclassOf(typeof(ComObject)))
            {
                Set(key, (ComObject)((object)value));
            }
            else
            {
                throw new ArgumentException("The type of the value is not supported");
            }
        }

        public void Set<T>(MFAttribute<T> keyValuePair)
        {
            if (keyValuePair == null)
                throw new ArgumentNullException("keyValuePair");
            Set(keyValuePair.Key, keyValuePair.Value);
        }
    }
}