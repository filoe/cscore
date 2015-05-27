using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CSCore.CoreAudioAPI;

namespace CSCore.Win32
{
    //propsys.h
    //http://msdn.microsoft.com/en-us/library/windows/desktop/bb761474(v=vs.85).aspx
    /// <summary>
    ///     Exposes methods for enumerating, getting, and setting property values.
    /// </summary>
    /// <remarks>
    ///     For more information,
    ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/bb761474(v=vs.85).aspx" />.
    /// </remarks>
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    public class PropertyStore : ComObject, IEnumerable<KeyValuePair<PropertyKey, PropertyVariant>>
    {
        private const string InterfaceName = "IPropertyStore";

        /// <summary>
        /// Device description - key
        /// </summary>
        public static readonly PropertyKey DeviceDesc = new PropertyKey(
            new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22),
            2);

        /// <summary>
        /// Device interface enabled - key
        /// </summary>
        public static readonly PropertyKey DeviceInterfaceEnabled = new PropertyKey(
            new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22),
            3);

        /// <summary>
        /// Device interface CLSID - key
        /// </summary>
        public static readonly PropertyKey DeviceInterfaceClassGuid = new PropertyKey(
            new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22),
            4);

        /// <summary>
        /// Device friendly name - key
        /// </summary>
        public static readonly PropertyKey FriendlyName = new PropertyKey(
            new Guid("{a45c254e-df1c-4efd-8020-67d146a850e0}"),
            14);

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyStore" /> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public PropertyStore(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Gets the number of properties available.
        /// </summary>
        public int Count
        {
            get { return GetCount(); }
        }

        /// <summary>
        /// Gets or sets the <see cref="PropertyVariant"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="PropertyVariant"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns>The <see cref="PropertyVariant"/> at the specified index.</returns>
        public PropertyVariant this[int index]
        {
            get { return GetValue(index); }
            set { SetValue(index, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="PropertyVariant"/> for the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        public PropertyVariant this[PropertyKey key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="PropertyStore"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the <see cref="PropertyStore"/>.
        /// </returns>
        public IEnumerator<KeyValuePair<PropertyKey, PropertyVariant>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                var pair = new KeyValuePair<PropertyKey, PropertyVariant>(GetKey(i), GetValue(i));
                yield return pair;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="PropertyStore"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the <see cref="PropertyStore"/>.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets data for a specific property.
        /// </summary>
        /// <param name="index">The zero-based index of the property.</param>
        /// <returns>The data of the specified property.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="index"/> is bigger or equal to <see cref="Count"/>.</exception>
        public PropertyVariant GetValue(int index)
        {
            if (index >= GetCount())
                throw new ArgumentOutOfRangeException("index");
            return GetValue(GetKey(index));
        }

        /// <summary>
        /// Gets data for a specific property.
        /// </summary>
        /// <param name="key">The <see cref="PropertyKey"/> of the property. The key can be obtained by calling the <see cref="GetKey"/> method.</param>
        /// <returns>The data of the specified property.</returns>
        public PropertyVariant GetValue(PropertyKey key)
        {
            PropertyVariant value;
            int result = GetValueNative(key, out value);
            Win32ComException.Try(result, InterfaceName, "GetValue");
            return value;
        }

        /// <summary>
        /// Gets a property key from an item's array of properties.
        /// </summary>
        /// <param name="index">The zero-based index of the property key in the array of <see cref="PropertyKey"/> structures.</param>
        /// <returns>The <see cref="PropertyKey"/>.</returns>
        public PropertyKey GetKey(int index)
        {
            if (index >= Count)
                throw new ArgumentOutOfRangeException("index");

            PropertyKey key;
            int result = GetAtNative(index, out key);
            Win32ComException.Try(result, InterfaceName, "GetAt");
            return key;
        }

        /// <summary>
        /// Sets a new property value, or replaces or removes an existing value.
        /// </summary>
        /// <param name="index">The index of the property.</param>
        /// <param name="value">The new property data.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="index"/> is bigger or equal to <see cref="Count"/>.</exception>
        public void SetValue(int index, PropertyVariant value)
        {
            if (index >= GetCount())
                throw new ArgumentOutOfRangeException("index");
            SetValue(GetKey(index), value);
        }

        /// <summary>
        /// Sets a new property value, or replaces or removes an existing value.
        /// </summary>
        /// <param name="key">The <see cref="PropertyKey"/> of the property. The key can be obtained by calling the <see cref="GetKey"/> method.</param>
        /// <param name="value">The new property data.</param>
        public void SetValue(PropertyKey key, PropertyVariant value)
        {
            int result = SetValueNative(key, value);
            Win32ComException.Try(result, InterfaceName, "SetValue");
        }

        private unsafe int GetCountNative(out int propertyCount)
        {
            fixed (void* ppc = &propertyCount)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ppc, ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
        }

        private unsafe int GetAtNative(int propertyIndex, out PropertyKey propertyKey)
        {
            propertyKey = new PropertyKey();
            fixed (void* ppk = &propertyKey)
            {
                return InteropCalls.CallI(UnsafeBasePtr, propertyIndex, ppk, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        private unsafe int GetValueNative(PropertyKey key, out PropertyVariant value)
        {
            fixed (void* pvalue = &value)
            {
                return InteropCalls.CallI(UnsafeBasePtr, &key, pvalue, ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
        }

        private unsafe int SetValueNative(PropertyKey key, PropertyVariant value)
        {
            return InteropCalls.CallI(UnsafeBasePtr, &key, &value, ((void**) (*(void**) UnsafeBasePtr))[6]);
        }

        private unsafe int CommitNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[7]);
        }

        /// <summary>
        ///     Saves a property change.
        /// </summary>
        /// <remarks>
        ///     For more information see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/bb761470%28v=vs.85%29.aspx" />.
        /// </remarks>
        public void Commit()
        {
            Win32ComException.Try(CommitNative(), InterfaceName, "Commit");
        }

        private int GetCount()
        {
            int count;
            Win32ComException.Try(GetCountNative(out count), InterfaceName, "GetCount");
            return count;
        }
    }
}