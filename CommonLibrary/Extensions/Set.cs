#region Set 文件信息
/***********************************************************
**文 件 名：Set 
**命名空间：CommonLibrary.Extensions 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2016-10-14 11:00:12 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Collections.Generic;

namespace CommonLibrary.Extensions
{
    internal class Set<TElement>
    {
        private readonly IEqualityComparer<TElement> _comparer;
        private int _count;
        private int _freeList;
        private Slot<TElement>[] _slots;
        private int[] _buckets;

        public Set(): this(null){ }

        public Set(IEqualityComparer<TElement> comparer)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<TElement>.Default;
            }
            _comparer = comparer;
            _slots = new Slot<TElement>[7];
            _buckets = new int[7];
            _freeList = -1;
        }


        private bool Find(TElement value, bool add)
        {
            var hashCode = InternalGetHashCode(value);
            for (var i = _buckets[hashCode % _buckets.Length] - 1; i >= 0; i = _slots[i].Next)
            {
                if ((_slots[i].HashCode == hashCode) && _comparer.Equals(_slots[i].Value, value))
                {
                    return true;
                }
            }
            if (add)
            {
                int freeList;
                if (_freeList >= 0)
                {
                    freeList = _freeList;
                    _freeList = _slots[freeList].Next;
                }
                else
                {
                    if (_count == _slots.Length)
                    {
                        Resize();
                    }
                    freeList = _count;
                    _count++;
                }
                var index = hashCode % _buckets.Length;
                _slots[freeList].HashCode = hashCode;
                _slots[freeList].Value = value;
                _slots[freeList].Next = _buckets[index] - 1;
                _buckets[index] = freeList + 1;
            }
            return false;
        }

        private void Resize()
        {
            var num = (_count * 2) + 1;
            var numArray = new int[num];
            var destinationArray = new Slot<TElement>[num];
            Array.Copy(_slots, 0, destinationArray, 0, _count);
            for (var i = 0; i < _count; i++)
            {
                var index = destinationArray[i].HashCode % num;
                destinationArray[i].Next = numArray[index] - 1;
                numArray[index] = i + 1;
            }
            _buckets = numArray;
            _slots = destinationArray;
        }

        public bool Add(TElement value)
        {
            return !Find(value, true);
        }

        internal int InternalGetHashCode(TElement value)
        {
            if (value != null)
            {
                return (_comparer.GetHashCode(value));
            }
            return 0;
        }
    }

    internal struct Slot<TElement>
    {
        internal int HashCode;
        internal TElement Value;
        internal int Next;
    }
}
