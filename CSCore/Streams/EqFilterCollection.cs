using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class EqFilterCollection : ICollection<EqFilterEntry>
    {
        private List<EqFilterEntry> _filters = new List<EqFilterEntry>();
        private int _channelCount;

        public EqFilterCollection(int channelCount)
        {
            if (channelCount <= 0)
                throw new ArgumentOutOfRangeException("channels");

            _channelCount = channelCount;
        }

        public EqFilterEntry this[int index]
        {
            get { return _filters[index]; }
        }

        public void Add(EqFilterEntry item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            _filters.Add(item);
        }

        public void Add(EqFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            _filters.Add(new EqFilterEntry(_channelCount, filter));
        }

        public void Clear()
        {
            _filters.Clear();
        }

        public bool Contains(EqFilterEntry item)
        {
            return _filters.Contains(item);
        }

        public void CopyTo(EqFilterEntry[] array, int arrayIndex)
        {
            _filters.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _filters.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(EqFilterEntry item)
        {
            return _filters.Remove(item);
        }

        public IEnumerator<EqFilterEntry> GetEnumerator()
        {
            return _filters.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
