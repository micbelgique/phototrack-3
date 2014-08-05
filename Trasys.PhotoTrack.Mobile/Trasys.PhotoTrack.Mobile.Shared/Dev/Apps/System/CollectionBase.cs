using System;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Collections
{
    /// <summary>Provides the abstract base class for a strongly typed collection.</summary>
    /// <filterpriority>2</filterpriority>S
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class CollectionBase : IList, ICollection, IEnumerable
    {
        private ArrayList list;
        /// <summary>Gets an <see cref="T:System.Collections.ArrayList" /> containing the list of elements in the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        /// <returns>An <see cref="T:System.Collections.ArrayList" /> representing the <see cref="T:System.Collections.CollectionBase" /> instance itself.Retrieving the value of this property is an O(1) operation.</returns>
        protected ArrayList InnerList
        {
            get
            {

                if (this.list == null)
                {
                    this.list = new ArrayList();
                }
                return this.list;
            }
        }
        /// <summary>Gets an <see cref="T:System.Collections.IList" /> containing the list of elements in the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        /// <returns>An <see cref="T:System.Collections.IList" /> representing the <see cref="T:System.Collections.CollectionBase" /> instance itself.</returns>
        protected IList List
        {
            get
            {
                return this;
            }
        }
        /// <summary>Gets or sets the number of elements that the <see cref="T:System.Collections.CollectionBase" /> can contain.</summary>
        /// <returns>The number of elements that the <see cref="T:System.Collections.CollectionBase" /> can contain.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <see cref="P:System.Collections.CollectionBase.Capacity" /> is set to a value that is less than <see cref="P:System.Collections.CollectionBase.Count" />.</exception>
        /// <exception cref="T:System.OutOfMemoryException">There is not enough memory available on the system.</exception>
        /// <filterpriority>2</filterpriority>
        [ComVisible(false)]
        public int Capacity
        {
            get
            {
                return this.InnerList.Capacity;
            }
            set
            {
                this.InnerList.Capacity = value;
            }
        }
        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.CollectionBase" /> instance. This property cannot be overridden.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.CollectionBase" /> instance.Retrieving the value of this property is an O(1) operation.</returns>
        /// <filterpriority>2</filterpriority>
        public int Count
        {
            get
            {
                if (this.list != null)
                {
                    return this.list.Count;
                }
                return 0;
            }
        }
        bool IList.IsReadOnly
        {
            get
            {
                return this.InnerList.IsReadOnly;
            }
        }
        bool IList.IsFixedSize
        {
            get
            {
                return this.InnerList.IsFixedSize;
            }
        }
        bool ICollection.IsSynchronized
        {
            get
            {
                return this.InnerList.IsSynchronized;
            }
        }
        object ICollection.SyncRoot
        {
            get
            {
                return this.InnerList.SyncRoot;
            }
        }
        object IList.this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                return this.InnerList[index];
            }
            set
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                this.OnValidate(value);
                object obj = this.InnerList[index];
                this.OnSet(index, obj, value);
                this.InnerList[index] = value;
                try
                {
                    this.OnSetComplete(index, obj, value);
                }
                catch
                {
                    this.InnerList[index] = obj;
                    throw;
                }
            }
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.CollectionBase" /> class with the default initial capacity.</summary>
        protected CollectionBase()
        {
            this.list = new ArrayList();
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.CollectionBase" /> class with the specified capacity.</summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        protected CollectionBase(int capacity)
        {
            this.list = new ArrayList(capacity);
        }
        /// <summary>Removes all objects from the <see cref="T:System.Collections.CollectionBase" /> instance. This method cannot be overridden.</summary>
        /// <filterpriority>2</filterpriority>
        public void Clear()
        {
            this.OnClear();
            this.InnerList.Clear();
            this.OnClearComplete();
        }
        /// <summary>Removes the element at the specified index of the <see cref="T:System.Collections.CollectionBase" /> instance. This method is not overridable.</summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or-<paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.CollectionBase.Count" />.</exception>
        /// <filterpriority>2</filterpriority>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
            }
            object value = this.InnerList[index];
            this.OnValidate(value);
            this.OnRemove(index, value);
            this.InnerList.RemoveAt(index);
            try
            {
                this.OnRemoveComplete(index, value);
            }
            catch
            {
                this.InnerList.Insert(index, value);
                throw;
            }
        }
        void ICollection.CopyTo(Array array, int index)
        {
            this.InnerList.CopyTo(array, index);
        }
        bool IList.Contains(object value)
        {
            return this.InnerList.Contains(value);
        }
        int IList.Add(object value)
        {
            this.OnValidate(value);
            this.OnInsert(this.InnerList.Count, value);
            int num = this.InnerList.Add(value);
            try
            {
                this.OnInsertComplete(num, value);
            }
            catch
            {
                this.InnerList.RemoveAt(num);
                throw;
            }
            return num;
        }
        void IList.Remove(object value)
        {
            this.OnValidate(value);
            int num = this.InnerList.IndexOf(value);
            if (num < 0)
            {
                throw new ArgumentException(GetMyResource("Arg_RemoveArgNotFound"));
            }
            this.OnRemove(num, value);
            this.InnerList.RemoveAt(num);
            try
            {
                this.OnRemoveComplete(num, value);
            }
            catch
            {
                this.InnerList.Insert(num, value);
                throw;
            }
        }
        int IList.IndexOf(object value)
        {
            return this.InnerList.IndexOf(value);
        }
        void IList.Insert(int index, object value)
        {
            if (index < 0 || index > this.Count)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
            }
            this.OnValidate(value);
            this.OnInsert(index, value);
            this.InnerList.Insert(index, value);
            try
            {
                this.OnInsertComplete(index, value);
            }
            catch
            {
                this.InnerList.RemoveAt(index);
                throw;
            }
        }
        /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.CollectionBase" /> instance.</returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }
        /// <summary>Performs additional custom processes before setting a value in the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        /// <param name="index">The zero-based index at which <paramref name="oldValue" /> can be found.</param>
        /// <param name="oldValue">The value to replace with <paramref name="newValue" />.</param>
        /// <param name="newValue">The new value of the element at <paramref name="index" />.</param>
        protected virtual void OnSet(int index, object oldValue, object newValue)
        {
        }
        /// <summary>Performs additional custom processes before inserting a new element into the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        /// <param name="index">The zero-based index at which to insert <paramref name="value" />.</param>
        /// <param name="value">The new value of the element at <paramref name="index" />.</param>
        protected virtual void OnInsert(int index, object value)
        {
        }
        /// <summary>Performs additional custom processes when clearing the contents of the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        protected virtual void OnClear()
        {
        }
        /// <summary>Performs additional custom processes when removing an element from the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        /// <param name="index">The zero-based index at which <paramref name="value" /> can be found.</param>
        /// <param name="value">The value of the element to remove from <paramref name="index" />.</param>
        protected virtual void OnRemove(int index, object value)
        {
        }
        /// <summary>Performs additional custom processes when validating a value.</summary>
        /// <param name="value">The object to validate.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="value" /> is null.</exception>
        protected virtual void OnValidate(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
        }
        /// <summary>Performs additional custom processes after setting a value in the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        /// <param name="index">The zero-based index at which <paramref name="oldValue" /> can be found.</param>
        /// <param name="oldValue">The value to replace with <paramref name="newValue" />.</param>
        /// <param name="newValue">The new value of the element at <paramref name="index" />.</param>
        protected virtual void OnSetComplete(int index, object oldValue, object newValue)
        {
        }
        /// <summary>Performs additional custom processes after inserting a new element into the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        /// <param name="index">The zero-based index at which to insert <paramref name="value" />.</param>
        /// <param name="value">The new value of the element at <paramref name="index" />.</param>
        protected virtual void OnInsertComplete(int index, object value)
        {
        }
        /// <summary>Performs additional custom processes after clearing the contents of the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        protected virtual void OnClearComplete()
        {
        }
        /// <summary>Performs additional custom processes after removing an element from the <see cref="T:System.Collections.CollectionBase" /> instance.</summary>
        /// <param name="index">The zero-based index at which <paramref name="value" /> can be found.</param>
        /// <param name="value">The value of the element to remove from <paramref name="index" />.</param>
        protected virtual void OnRemoveComplete(int index, object value)
        {
        }

        public static string GetMyResource(string text)
        {
            return text;
        }

    }

    /// <summary>Implements the <see cref="T:System.Collections.IList" /> interface using an array whose size is dynamically increased as required.</summary>
    /// <filterpriority>1</filterpriority>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1039:ListsAreStronglyTyped"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class ArrayList : IList, ICollection, IEnumerable
    {
        private sealed class ArrayListEnumerator : IEnumerator
        {
            private ArrayList list;
            private int index;
            private int endIndex;
            private int version;
            private object currentElement;
            private int startIndex;
            public object Current
            {
                get
                {
                    if (this.index < this.startIndex)
                    {
                        throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumNotStarted"));
                    }
                    if (this.index > this.endIndex)
                    {
                        throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumEnded"));
                    }
                    return this.currentElement;
                }
            }
            internal ArrayListEnumerator(ArrayList list, int index, int count)
            {
                this.list = list;
                this.startIndex = index;
                this.index = index - 1;
                this.endIndex = this.index + count;
                this.version = list._version;
                this.currentElement = null;
            }
            public object Clone()
            {
                return base.MemberwiseClone();
            }
            public bool MoveNext()
            {
                if (this.version != this.list._version)
                {
                    throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumFailedVersion"));
                }
                if (this.index < this.endIndex)
                {
                    this.currentElement = this.list[++this.index];
                    return true;
                }
                this.index = this.endIndex + 1;
                return false;
            }
            public void Reset()
            {
                if (this.version != this.list._version)
                {
                    throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumFailedVersion"));
                }
                this.index = this.startIndex - 1;
            }
        }

        private sealed class ArrayListEnumeratorSimple : IEnumerator
        {
            private ArrayList list;
            private int index;
            private int version;
            private object currentElement;

            private bool isArrayList;
            private static object dummyObject = new object();
            public object Current
            {
                get
                {
                    object obj = this.currentElement;
                    if (ArrayList.ArrayListEnumeratorSimple.dummyObject != obj)
                    {
                        return obj;
                    }
                    if (this.index == -1)
                    {
                        throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumNotStarted"));
                    }
                    throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumEnded"));
                }
            }
            internal ArrayListEnumeratorSimple(ArrayList list)
            {
                this.list = list;
                this.index = -1;
                this.version = list._version;
                this.isArrayList = (list.GetType() == typeof(ArrayList));
                this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
            }
            public object Clone()
            {
                return base.MemberwiseClone();
            }
            public bool MoveNext()
            {
                if (this.version != this.list._version)
                {
                    throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumFailedVersion"));
                }
                if (this.isArrayList)
                {
                    if (this.index < this.list._size - 1)
                    {
                        this.currentElement = this.list._items[++this.index];
                        return true;
                    }
                    this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
                    this.index = this.list._size;
                    return false;
                }
                else
                {
                    if (this.index < this.list.Count - 1)
                    {
                        this.currentElement = this.list[++this.index];
                        return true;
                    }
                    this.index = this.list.Count;
                    this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
                    return false;
                }
            }
            public void Reset()
            {
                if (this.version != this.list._version)
                {
                    throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumFailedVersion"));
                }
                this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
                this.index = -1;
            }
        }
        internal class ArrayListDebugView
        {
            private ArrayList arrayList;
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public object[] Items
            {
                get
                {
                    return this.arrayList.ToArray();
                }
            }
            public ArrayListDebugView(ArrayList arrayList)
            {
                if (arrayList == null)
                {
                    throw new ArgumentNullException("arrayList");
                }
                this.arrayList = arrayList;
            }
        }

        private class IListWrapper : ArrayList
        {

            private sealed class IListWrapperEnumWrapper : IEnumerator
            {
                private IEnumerator _en;
                private int _remaining;
                private int _initialStartIndex;
                private int _initialCount;
                private bool _firstCall;
                public object Current
                {
                    get
                    {
                        if (this._firstCall)
                        {
                            throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumNotStarted"));
                        }
                        if (this._remaining < 0)
                        {
                            throw new InvalidOperationException(GetMyResource("InvalidOperation_EnumEnded"));
                        }
                        return this._en.Current;
                    }
                }
                private IListWrapperEnumWrapper()
                {
                }
                internal IListWrapperEnumWrapper(ArrayList.IListWrapper listWrapper, int startIndex, int count)
                {
                    this._en = listWrapper.GetEnumerator();
                    this._initialStartIndex = startIndex;
                    this._initialCount = count;
                    while (startIndex-- > 0 && this._en.MoveNext())
                    {
                    }
                    this._remaining = count;
                    this._firstCall = true;
                }

                public bool MoveNext()
                {
                    if (this._firstCall)
                    {
                        this._firstCall = false;
                        return this._remaining-- > 0 && this._en.MoveNext();
                    }
                    if (this._remaining < 0)
                    {
                        return false;
                    }
                    bool flag = this._en.MoveNext();
                    return flag && this._remaining-- > 0;
                }
                public void Reset()
                {
                    this._en.Reset();
                    int initialStartIndex = this._initialStartIndex;
                    while (initialStartIndex-- > 0 && this._en.MoveNext())
                    {
                    }
                    this._remaining = this._initialCount;
                    this._firstCall = true;
                }
            }
            private IList _list;
            public override int Capacity
            {
                get
                {
                    return this._list.Count;
                }
                set
                {
                    if (value < this.Count)
                    {
                        throw new ArgumentOutOfRangeException("value", GetMyResource("ArgumentOutOfRange_SmallCapacity"));
                    }
                }
            }
            public override int Count
            {
                get
                {
                    return this._list.Count;
                }
            }
            public override bool IsReadOnly
            {
                get
                {
                    return this._list.IsReadOnly;
                }
            }
            public override bool IsFixedSize
            {
                get
                {
                    return this._list.IsFixedSize;
                }
            }
            public override bool IsSynchronized
            {
                get
                {
                    return this._list.IsSynchronized;
                }
            }
            public override object this[int index]
            {
                get
                {
                    return this._list[index];
                }
                set
                {
                    this._list[index] = value;
                    this._version++;
                }
            }
            public override object SyncRoot
            {
                get
                {
                    return this._list.SyncRoot;
                }
            }
            internal IListWrapper(IList list)
            {
                this._list = list;
                this._version = 0;
            }
            public override int Add(object obj)
            {
                int result = this._list.Add(obj);
                this._version++;
                return result;
            }
            public override void AddRange(ICollection c)
            {
                this.InsertRange(this.Count, c);
            }
            public override int BinarySearch(int index, int count, object value, IComparer comparer)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this.Count - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                if (comparer == null)
                {
                    comparer = null;  // Comparer.Default;
                }
                int i = index;
                int num = index + count - 1;
                while (i <= num)
                {
                    int num2 = (i + num) / 2;
                    int num3 = comparer.Compare(value, this._list[num2]);
                    if (num3 == 0)
                    {
                        return num2;
                    }
                    if (num3 < 0)
                    {
                        num = num2 - 1;
                    }
                    else
                    {
                        i = num2 + 1;
                    }
                }
                return ~i;
            }
            public override void Clear()
            {
                if (this._list.IsFixedSize)
                {
                    throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
                }
                this._list.Clear();
                this._version++;
            }
            public override object Clone()
            {
                return new ArrayList.IListWrapper(this._list);
            }
            public override bool Contains(object obj)
            {
                return this._list.Contains(obj);
            }
            public override void CopyTo(Array array, int index)
            {
                this._list.CopyTo(array, index);
            }
            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (index < 0 || arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "arrayIndex", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (array.Length - arrayIndex < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                if (array.Rank != 1)
                {
                    throw new ArgumentException(GetMyResource("Arg_RankMultiDimNotSupported"));
                }
                if (this._list.Count - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                for (int i = index; i < index + count; i++)
                {
                    array.SetValue(this._list[i], arrayIndex++);
                }
            }
            public override IEnumerator GetEnumerator()
            {
                return this._list.GetEnumerator();
            }
            public override IEnumerator GetEnumerator(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._list.Count - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                return new ArrayList.IListWrapper.IListWrapperEnumWrapper(this, index, count);
            }
            public override int IndexOf(object value)
            {
                return this._list.IndexOf(value);
            }
            public override int IndexOf(object value, int startIndex)
            {
                return this.IndexOf(value, startIndex, this._list.Count - startIndex);
            }
            public override int IndexOf(object value, int startIndex, int count)
            {
                if (startIndex < 0 || startIndex > this.Count)
                {
                    throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_Index"));
                }
                if (count < 0 || startIndex > this.Count - count)
                {
                    throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_Count"));
                }
                int num = startIndex + count;
                if (value == null)
                {
                    for (int i = startIndex; i < num; i++)
                    {
                        if (this._list[i] == null)
                        {
                            return i;
                        }
                    }
                    return -1;
                }
                for (int j = startIndex; j < num; j++)
                {
                    if (this._list[j] != null && this._list[j].Equals(value))
                    {
                        return j;
                    }
                }
                return -1;
            }
            public override void Insert(int index, object obj)
            {
                this._list.Insert(index, obj);
                this._version++;
            }
            public override void InsertRange(int index, ICollection c)
            {
                if (c == null)
                {
                    throw new ArgumentNullException("c", GetMyResource("ArgumentNull_Collection"));
                }
                if (index < 0 || index > this.Count)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                if (c.Count > 0)
                {
                    ArrayList arrayList = this._list as ArrayList;
                    if (arrayList != null)
                    {
                        arrayList.InsertRange(index, c);
                    }
                    else
                    {
                        IEnumerator enumerator = c.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            this._list.Insert(index++, enumerator.Current);
                        }
                    }
                    this._version++;
                }
            }
            public override int LastIndexOf(object value)
            {
                return this.LastIndexOf(value, this._list.Count - 1, this._list.Count);
            }
            public override int LastIndexOf(object value, int startIndex)
            {
                return this.LastIndexOf(value, startIndex, startIndex + 1);
            }
            public override int LastIndexOf(object value, int startIndex, int count)
            {
                if (this._list.Count == 0)
                {
                    return -1;
                }
                if (startIndex < 0 || startIndex >= this._list.Count)
                {
                    throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_Index"));
                }
                if (count < 0 || count > startIndex + 1)
                {
                    throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_Count"));
                }
                int num = startIndex - count + 1;
                if (value == null)
                {
                    for (int i = startIndex; i >= num; i--)
                    {
                        if (this._list[i] == null)
                        {
                            return i;
                        }
                    }
                    return -1;
                }
                for (int j = startIndex; j >= num; j--)
                {
                    if (this._list[j] != null && this._list[j].Equals(value))
                    {
                        return j;
                    }
                }
                return -1;
            }
            public override void Remove(object value)
            {
                int num = this.IndexOf(value);
                if (num >= 0)
                {
                    this.RemoveAt(num);
                }
            }
            public override void RemoveAt(int index)
            {
                this._list.RemoveAt(index);
                this._version++;
            }
            public override void RemoveRange(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._list.Count - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                if (count > 0)
                {
                    this._version++;
                }
                while (count > 0)
                {
                    this._list.RemoveAt(index);
                    count--;
                }
            }
            public override void Reverse(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._list.Count - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                int i = index;
                int num = index + count - 1;
                while (i < num)
                {
                    object value = this._list[i];
                    this._list[i++] = this._list[num];
                    this._list[num--] = value;
                }
                this._version++;
            }
            public override void SetRange(int index, ICollection c)
            {
                if (c == null)
                {
                    throw new ArgumentNullException("c", GetMyResource("ArgumentNull_Collection"));
                }
                if (index < 0 || index > this._list.Count - c.Count)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                if (c.Count > 0)
                {
                    IEnumerator enumerator = c.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        this._list[index++] = enumerator.Current;
                    }
                    this._version++;
                }
            }
            public override ArrayList GetRange(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._list.Count - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                return new ArrayList.Range(this, index, count);
            }
            public override void Sort(int index, int count, IComparer comparer)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._list.Count - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                object[] array = new object[count];
                this.CopyTo(index, array, 0, count);
                Array.Sort(array, 0, count, comparer);
                for (int i = 0; i < count; i++)
                {
                    this._list[i + index] = array[i];
                }
                this._version++;
            }
            public override object[] ToArray()
            {
                object[] array = new object[this.Count];
                this._list.CopyTo(array, 0);
                return array;
            }
            [SecuritySafeCritical]
            public override Array ToArray(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }
                Array array = Array.CreateInstance(type, this._list.Count);
                this._list.CopyTo(array, 0);
                return array;
            }
            public override void TrimToSize()
            {
            }
        }

        private class SyncArrayList : ArrayList
        {
            private ArrayList _list;
            private object _root;
            public override int Capacity
            {
                get
                {
                    int capacity;
                    lock (this._root)
                    {
                        capacity = this._list.Capacity;
                    }
                    return capacity;
                }
                set
                {
                    lock (this._root)
                    {
                        this._list.Capacity = value;
                    }
                }
            }
            public override int Count
            {
                get
                {
                    int count;
                    lock (this._root)
                    {
                        count = this._list.Count;
                    }
                    return count;
                }
            }
            public override bool IsReadOnly
            {
                get
                {
                    return this._list.IsReadOnly;
                }
            }
            public override bool IsFixedSize
            {
                get
                {
                    return this._list.IsFixedSize;
                }
            }
            public override bool IsSynchronized
            {
                get
                {
                    return true;
                }
            }
            public override object this[int index]
            {
                get
                {
                    object result;
                    lock (this._root)
                    {
                        result = this._list[index];
                    }
                    return result;
                }
                set
                {
                    lock (this._root)
                    {
                        this._list[index] = value;
                    }
                }
            }
            public override object SyncRoot
            {
                get
                {
                    return this._root;
                }
            }
            internal SyncArrayList(ArrayList list)
                : base(false)
            {
                this._list = list;
                this._root = list.SyncRoot;
            }
            public override int Add(object value)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.Add(value);
                }
                return result;
            }
            public override void AddRange(ICollection c)
            {
                lock (this._root)
                {
                    this._list.AddRange(c);
                }
            }
            public override int BinarySearch(object value)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.BinarySearch(value);
                }
                return result;
            }
            public override int BinarySearch(object value, IComparer comparer)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.BinarySearch(value, comparer);
                }
                return result;
            }
            public override int BinarySearch(int index, int count, object value, IComparer comparer)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.BinarySearch(index, count, value, comparer);
                }
                return result;
            }
            public override void Clear()
            {
                lock (this._root)
                {
                    this._list.Clear();
                }
            }
            public override object Clone()
            {
                object result;
                lock (this._root)
                {
                    result = new ArrayList.SyncArrayList((ArrayList)this._list.Clone());
                }
                return result;
            }
            public override bool Contains(object item)
            {
                bool result;
                lock (this._root)
                {
                    result = this._list.Contains(item);
                }
                return result;
            }
            public override void CopyTo(Array array)
            {
                lock (this._root)
                {
                    this._list.CopyTo(array);
                }
            }
            public override void CopyTo(Array array, int index)
            {
                lock (this._root)
                {
                    this._list.CopyTo(array, index);
                }
            }
            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                lock (this._root)
                {
                    this._list.CopyTo(index, array, arrayIndex, count);
                }
            }
            public override IEnumerator GetEnumerator()
            {
                IEnumerator enumerator;
                lock (this._root)
                {
                    enumerator = this._list.GetEnumerator();
                }
                return enumerator;
            }
            public override IEnumerator GetEnumerator(int index, int count)
            {
                IEnumerator enumerator;
                lock (this._root)
                {
                    enumerator = this._list.GetEnumerator(index, count);
                }
                return enumerator;
            }
            public override int IndexOf(object value)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.IndexOf(value);
                }
                return result;
            }
            public override int IndexOf(object value, int startIndex)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.IndexOf(value, startIndex);
                }
                return result;
            }
            public override int IndexOf(object value, int startIndex, int count)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.IndexOf(value, startIndex, count);
                }
                return result;
            }
            public override void Insert(int index, object value)
            {
                lock (this._root)
                {
                    this._list.Insert(index, value);
                }
            }
            public override void InsertRange(int index, ICollection c)
            {
                lock (this._root)
                {
                    this._list.InsertRange(index, c);
                }
            }
            public override int LastIndexOf(object value)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.LastIndexOf(value);
                }
                return result;
            }
            public override int LastIndexOf(object value, int startIndex)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.LastIndexOf(value, startIndex);
                }
                return result;
            }
            public override int LastIndexOf(object value, int startIndex, int count)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.LastIndexOf(value, startIndex, count);
                }
                return result;
            }
            public override void Remove(object value)
            {
                lock (this._root)
                {
                    this._list.Remove(value);
                }
            }
            public override void RemoveAt(int index)
            {
                lock (this._root)
                {
                    this._list.RemoveAt(index);
                }
            }
            public override void RemoveRange(int index, int count)
            {
                lock (this._root)
                {
                    this._list.RemoveRange(index, count);
                }
            }
            public override void Reverse(int index, int count)
            {
                lock (this._root)
                {
                    this._list.Reverse(index, count);
                }
            }
            public override void SetRange(int index, ICollection c)
            {
                lock (this._root)
                {
                    this._list.SetRange(index, c);
                }
            }
            public override ArrayList GetRange(int index, int count)
            {
                ArrayList range;
                lock (this._root)
                {
                    range = this._list.GetRange(index, count);
                }
                return range;
            }
            public override void Sort()
            {
                lock (this._root)
                {
                    this._list.Sort();
                }
            }
            public override void Sort(IComparer comparer)
            {
                lock (this._root)
                {
                    this._list.Sort(comparer);
                }
            }
            public override void Sort(int index, int count, IComparer comparer)
            {
                lock (this._root)
                {
                    this._list.Sort(index, count, comparer);
                }
            }
            public override object[] ToArray()
            {
                object[] result;
                lock (this._root)
                {
                    result = this._list.ToArray();
                }
                return result;
            }
            public override Array ToArray(Type type)
            {
                Array result;
                lock (this._root)
                {
                    result = this._list.ToArray(type);
                }
                return result;
            }
            public override void TrimToSize()
            {
                lock (this._root)
                {
                    this._list.TrimToSize();
                }
            }
        }

        private class SyncIList : IList, ICollection, IEnumerable
        {
            private IList _list;
            private object _root;
            public virtual int Count
            {
                get
                {
                    int count;
                    lock (this._root)
                    {
                        count = this._list.Count;
                    }
                    return count;
                }
            }
            public virtual bool IsReadOnly
            {
                get
                {
                    return this._list.IsReadOnly;
                }
            }
            public virtual bool IsFixedSize
            {
                get
                {
                    return this._list.IsFixedSize;
                }
            }
            public virtual bool IsSynchronized
            {
                get
                {
                    return true;
                }
            }
            public virtual object this[int index]
            {
                get
                {
                    object result;
                    lock (this._root)
                    {
                        result = this._list[index];
                    }
                    return result;
                }
                set
                {
                    lock (this._root)
                    {
                        this._list[index] = value;
                    }
                }
            }
            public virtual object SyncRoot
            {
                get
                {
                    return this._root;
                }
            }
            internal SyncIList(IList list)
            {
                this._list = list;
                this._root = list.SyncRoot;
            }
            public virtual int Add(object value)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.Add(value);
                }
                return result;
            }
            public virtual void Clear()
            {
                lock (this._root)
                {
                    this._list.Clear();
                }
            }
            public virtual bool Contains(object item)
            {
                bool result;
                lock (this._root)
                {
                    result = this._list.Contains(item);
                }
                return result;
            }
            public virtual void CopyTo(Array array, int index)
            {
                lock (this._root)
                {
                    this._list.CopyTo(array, index);
                }
            }
            public virtual IEnumerator GetEnumerator()
            {
                IEnumerator enumerator;
                lock (this._root)
                {
                    enumerator = this._list.GetEnumerator();
                }
                return enumerator;
            }
            public virtual int IndexOf(object value)
            {
                int result;
                lock (this._root)
                {
                    result = this._list.IndexOf(value);
                }
                return result;
            }
            public virtual void Insert(int index, object value)
            {
                lock (this._root)
                {
                    this._list.Insert(index, value);
                }
            }
            public virtual void Remove(object value)
            {
                lock (this._root)
                {
                    this._list.Remove(value);
                }
            }
            public virtual void RemoveAt(int index)
            {
                lock (this._root)
                {
                    this._list.RemoveAt(index);
                }
            }
        }

        private class FixedSizeList : IList, ICollection, IEnumerable
        {
            private IList _list;
            public virtual int Count
            {
                get
                {
                    return this._list.Count;
                }
            }
            public virtual bool IsReadOnly
            {
                get
                {
                    return this._list.IsReadOnly;
                }
            }
            public virtual bool IsFixedSize
            {
                get
                {
                    return true;
                }
            }
            public virtual bool IsSynchronized
            {
                get
                {
                    return this._list.IsSynchronized;
                }
            }
            public virtual object this[int index]
            {
                get
                {
                    return this._list[index];
                }
                set
                {
                    this._list[index] = value;
                }
            }
            public virtual object SyncRoot
            {
                get
                {
                    return this._list.SyncRoot;
                }
            }
            internal FixedSizeList(IList l)
            {
                this._list = l;
            }
            public virtual int Add(object obj)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public virtual void Clear()
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public virtual bool Contains(object obj)
            {
                return this._list.Contains(obj);
            }
            public virtual void CopyTo(Array array, int index)
            {
                this._list.CopyTo(array, index);
            }
            public virtual IEnumerator GetEnumerator()
            {
                return this._list.GetEnumerator();
            }
            public virtual int IndexOf(object value)
            {
                return this._list.IndexOf(value);
            }
            public virtual void Insert(int index, object obj)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public virtual void Remove(object value)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public virtual void RemoveAt(int index)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
        }

        private class FixedSizeArrayList : ArrayList
        {
            private ArrayList _list;
            public override int Count
            {
                get
                {
                    return this._list.Count;
                }
            }
            public override bool IsReadOnly
            {
                get
                {
                    return this._list.IsReadOnly;
                }
            }
            public override bool IsFixedSize
            {
                get
                {
                    return true;
                }
            }
            public override bool IsSynchronized
            {
                get
                {
                    return this._list.IsSynchronized;
                }
            }
            public override object this[int index]
            {
                get
                {
                    return this._list[index];
                }
                set
                {
                    this._list[index] = value;
                    this._version = this._list._version;
                }
            }
            public override object SyncRoot
            {
                get
                {
                    return this._list.SyncRoot;
                }
            }
            public override int Capacity
            {
                get
                {
                    return this._list.Capacity;
                }
                set
                {
                    throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
                }
            }
            internal FixedSizeArrayList(ArrayList l)
            {
                this._list = l;
                this._version = this._list._version;
            }
            public override int Add(object obj)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public override void AddRange(ICollection c)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public override int BinarySearch(int index, int count, object value, IComparer comparer)
            {
                return this._list.BinarySearch(index, count, value, comparer);
            }
            public override void Clear()
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public override object Clone()
            {
                return new ArrayList.FixedSizeArrayList(this._list)
                {
                    _list = (ArrayList)this._list.Clone()
                };
            }
            public override bool Contains(object obj)
            {
                return this._list.Contains(obj);
            }
            public override void CopyTo(Array array, int index)
            {
                this._list.CopyTo(array, index);
            }
            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                this._list.CopyTo(index, array, arrayIndex, count);
            }
            public override IEnumerator GetEnumerator()
            {
                return this._list.GetEnumerator();
            }
            public override IEnumerator GetEnumerator(int index, int count)
            {
                return this._list.GetEnumerator(index, count);
            }
            public override int IndexOf(object value)
            {
                return this._list.IndexOf(value);
            }
            public override int IndexOf(object value, int startIndex)
            {
                return this._list.IndexOf(value, startIndex);
            }
            public override int IndexOf(object value, int startIndex, int count)
            {
                return this._list.IndexOf(value, startIndex, count);
            }
            public override void Insert(int index, object obj)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public override void InsertRange(int index, ICollection c)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public override int LastIndexOf(object value)
            {
                return this._list.LastIndexOf(value);
            }
            public override int LastIndexOf(object value, int startIndex)
            {
                return this._list.LastIndexOf(value, startIndex);
            }
            public override int LastIndexOf(object value, int startIndex, int count)
            {
                return this._list.LastIndexOf(value, startIndex, count);
            }
            public override void Remove(object value)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public override void RemoveAt(int index)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public override void RemoveRange(int index, int count)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
            public override void SetRange(int index, ICollection c)
            {
                this._list.SetRange(index, c);
                this._version = this._list._version;
            }
            public override ArrayList GetRange(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this.Count - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                return new ArrayList.Range(this, index, count);
            }
            public override void Reverse(int index, int count)
            {
                this._list.Reverse(index, count);
                this._version = this._list._version;
            }
            public override void Sort(int index, int count, IComparer comparer)
            {
                this._list.Sort(index, count, comparer);
                this._version = this._list._version;
            }
            public override object[] ToArray()
            {
                return this._list.ToArray();
            }
            public override Array ToArray(Type type)
            {
                return this._list.ToArray(type);
            }
            public override void TrimToSize()
            {
                throw new NotSupportedException(GetMyResource("NotSupported_FixedSizeCollection"));
            }
        }

        private class ReadOnlyList : IList, ICollection, IEnumerable
        {
            private IList _list;
            public virtual int Count
            {
                get
                {
                    return this._list.Count;
                }
            }
            public virtual bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }
            public virtual bool IsFixedSize
            {
                get
                {
                    return true;
                }
            }
            public virtual bool IsSynchronized
            {
                get
                {
                    return this._list.IsSynchronized;
                }
            }
            public virtual object this[int index]
            {
                get
                {
                    return this._list[index];
                }
                set
                {
                    throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
                }
            }
            public virtual object SyncRoot
            {
                get
                {
                    return this._list.SyncRoot;
                }
            }
            internal ReadOnlyList(IList l)
            {
                this._list = l;
            }
            public virtual int Add(object obj)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public virtual void Clear()
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public virtual bool Contains(object obj)
            {
                return this._list.Contains(obj);
            }
            public virtual void CopyTo(Array array, int index)
            {
                this._list.CopyTo(array, index);
            }
            public virtual IEnumerator GetEnumerator()
            {
                return this._list.GetEnumerator();
            }
            public virtual int IndexOf(object value)
            {
                return this._list.IndexOf(value);
            }
            public virtual void Insert(int index, object obj)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public virtual void Remove(object value)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public virtual void RemoveAt(int index)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
        }

        private class ReadOnlyArrayList : ArrayList
        {
            private ArrayList _list;
            public override int Count
            {
                get
                {
                    return this._list.Count;
                }
            }
            public override bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }
            public override bool IsFixedSize
            {
                get
                {
                    return true;
                }
            }
            public override bool IsSynchronized
            {
                get
                {
                    return this._list.IsSynchronized;
                }
            }
            public override object this[int index]
            {
                get
                {
                    return this._list[index];
                }
                set
                {
                    throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
                }
            }
            public override object SyncRoot
            {
                get
                {
                    return this._list.SyncRoot;
                }
            }
            public override int Capacity
            {
                get
                {
                    return this._list.Capacity;
                }
                set
                {
                    throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
                }
            }
            internal ReadOnlyArrayList(ArrayList l)
            {
                this._list = l;
            }
            public override int Add(object obj)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override void AddRange(ICollection c)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override int BinarySearch(int index, int count, object value, IComparer comparer)
            {
                return this._list.BinarySearch(index, count, value, comparer);
            }
            public override void Clear()
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override object Clone()
            {
                return new ArrayList.ReadOnlyArrayList(this._list)
                {
                    _list = (ArrayList)this._list.Clone()
                };
            }
            public override bool Contains(object obj)
            {
                return this._list.Contains(obj);
            }
            public override void CopyTo(Array array, int index)
            {
                this._list.CopyTo(array, index);
            }
            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                this._list.CopyTo(index, array, arrayIndex, count);
            }
            public override IEnumerator GetEnumerator()
            {
                return this._list.GetEnumerator();
            }
            public override IEnumerator GetEnumerator(int index, int count)
            {
                return this._list.GetEnumerator(index, count);
            }
            public override int IndexOf(object value)
            {
                return this._list.IndexOf(value);
            }
            public override int IndexOf(object value, int startIndex)
            {
                return this._list.IndexOf(value, startIndex);
            }
            public override int IndexOf(object value, int startIndex, int count)
            {
                return this._list.IndexOf(value, startIndex, count);
            }
            public override void Insert(int index, object obj)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override void InsertRange(int index, ICollection c)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override int LastIndexOf(object value)
            {
                return this._list.LastIndexOf(value);
            }
            public override int LastIndexOf(object value, int startIndex)
            {
                return this._list.LastIndexOf(value, startIndex);
            }
            public override int LastIndexOf(object value, int startIndex, int count)
            {
                return this._list.LastIndexOf(value, startIndex, count);
            }
            public override void Remove(object value)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override void RemoveAt(int index)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override void RemoveRange(int index, int count)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override void SetRange(int index, ICollection c)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override ArrayList GetRange(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this.Count - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                return new ArrayList.Range(this, index, count);
            }
            public override void Reverse(int index, int count)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override void Sort(int index, int count, IComparer comparer)
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
            public override object[] ToArray()
            {
                return this._list.ToArray();
            }
            public override Array ToArray(Type type)
            {
                return this._list.ToArray(type);
            }
            public override void TrimToSize()
            {
                throw new NotSupportedException(GetMyResource("NotSupported_ReadOnlyCollection"));
            }
        }

        private class Range : ArrayList
        {
            private ArrayList _baseList;
            private int _baseIndex;
            private int _baseSize;
            private int _baseVersion;
            public override int Capacity
            {
                get
                {
                    return this._baseList.Capacity;
                }
                set
                {
                    if (value < this.Count)
                    {
                        throw new ArgumentOutOfRangeException("value", GetMyResource("ArgumentOutOfRange_SmallCapacity"));
                    }
                }
            }
            public override int Count
            {
                get
                {
                    this.InternalUpdateRange();
                    return this._baseSize;
                }
            }
            public override bool IsReadOnly
            {
                get
                {
                    return this._baseList.IsReadOnly;
                }
            }
            public override bool IsFixedSize
            {
                get
                {
                    return this._baseList.IsFixedSize;
                }
            }
            public override bool IsSynchronized
            {
                get
                {
                    return this._baseList.IsSynchronized;
                }
            }
            public override object SyncRoot
            {
                get
                {
                    return this._baseList.SyncRoot;
                }
            }
            public override object this[int index]
            {
                get
                {
                    this.InternalUpdateRange();
                    if (index < 0 || index >= this._baseSize)
                    {
                        throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                    }
                    return this._baseList[this._baseIndex + index];
                }
                set
                {
                    this.InternalUpdateRange();
                    if (index < 0 || index >= this._baseSize)
                    {
                        throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                    }
                    this._baseList[this._baseIndex + index] = value;
                    this.InternalUpdateVersion();
                }
            }
            internal Range(ArrayList list, int index, int count)
                : base(false)
            {
                this._baseList = list;
                this._baseIndex = index;
                this._baseSize = count;
                this._baseVersion = list._version;
                this._version = list._version;
            }
            private void InternalUpdateRange()
            {
                if (this._baseVersion != this._baseList._version)
                {
                    throw new InvalidOperationException(GetMyResource("InvalidOperation_UnderlyingArrayListChanged"));
                }
            }
            private void InternalUpdateVersion()
            {
                this._baseVersion++;
                this._version++;
            }
            public override int Add(object value)
            {
                this.InternalUpdateRange();
                this._baseList.Insert(this._baseIndex + this._baseSize, value);
                this.InternalUpdateVersion();
                return this._baseSize++;
            }
            public override void AddRange(ICollection c)
            {
                if (c == null)
                {
                    throw new ArgumentNullException("c");
                }
                this.InternalUpdateRange();
                int count = c.Count;
                if (count > 0)
                {
                    this._baseList.InsertRange(this._baseIndex + this._baseSize, c);
                    this.InternalUpdateVersion();
                    this._baseSize += count;
                }
            }
            public override int BinarySearch(int index, int count, object value, IComparer comparer)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._baseSize - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                this.InternalUpdateRange();
                int num = this._baseList.BinarySearch(this._baseIndex + index, count, value, comparer);
                if (num >= 0)
                {
                    return num - this._baseIndex;
                }
                return num + this._baseIndex;
            }
            public override void Clear()
            {
                this.InternalUpdateRange();
                if (this._baseSize != 0)
                {
                    this._baseList.RemoveRange(this._baseIndex, this._baseSize);
                    this.InternalUpdateVersion();
                    this._baseSize = 0;
                }
            }
            public override object Clone()
            {
                this.InternalUpdateRange();
                return new ArrayList.Range(this._baseList, this._baseIndex, this._baseSize)
                {
                    _baseList = (ArrayList)this._baseList.Clone()
                };
            }
            public override bool Contains(object item)
            {
                this.InternalUpdateRange();
                if (item == null)
                {
                    for (int i = 0; i < this._baseSize; i++)
                    {
                        if (this._baseList[this._baseIndex + i] == null)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                for (int j = 0; j < this._baseSize; j++)
                {
                    if (this._baseList[this._baseIndex + j] != null && this._baseList[this._baseIndex + j].Equals(item))
                    {
                        return true;
                    }
                }
                return false;
            }
            public override void CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (array.Rank != 1)
                {
                    throw new ArgumentException(GetMyResource("Arg_RankMultiDimNotSupported"));
                }
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (array.Length - index < this._baseSize)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                this.InternalUpdateRange();
                this._baseList.CopyTo(this._baseIndex, array, index, this._baseSize);
            }
            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (array.Rank != 1)
                {
                    throw new ArgumentException(GetMyResource("Arg_RankMultiDimNotSupported"));
                }
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (array.Length - arrayIndex < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                if (this._baseSize - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                this.InternalUpdateRange();
                this._baseList.CopyTo(this._baseIndex + index, array, arrayIndex, count);
            }
            public override IEnumerator GetEnumerator()
            {
                return this.GetEnumerator(0, this._baseSize);
            }
            public override IEnumerator GetEnumerator(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._baseSize - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                this.InternalUpdateRange();
                return this._baseList.GetEnumerator(this._baseIndex + index, count);
            }
            public override ArrayList GetRange(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._baseSize - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                this.InternalUpdateRange();
                return new ArrayList.Range(this, index, count);
            }
            public override int IndexOf(object value)
            {
                this.InternalUpdateRange();
                int num = this._baseList.IndexOf(value, this._baseIndex, this._baseSize);
                if (num >= 0)
                {
                    return num - this._baseIndex;
                }
                return -1;
            }
            public override int IndexOf(object value, int startIndex)
            {
                if (startIndex < 0)
                {
                    throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (startIndex > this._baseSize)
                {
                    throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_Index"));
                }
                this.InternalUpdateRange();
                int num = this._baseList.IndexOf(value, this._baseIndex + startIndex, this._baseSize - startIndex);
                if (num >= 0)
                {
                    return num - this._baseIndex;
                }
                return -1;
            }
            public override int IndexOf(object value, int startIndex, int count)
            {
                if (startIndex < 0 || startIndex > this._baseSize)
                {
                    throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_Index"));
                }
                if (count < 0 || startIndex > this._baseSize - count)
                {
                    throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_Count"));
                }
                this.InternalUpdateRange();
                int num = this._baseList.IndexOf(value, this._baseIndex + startIndex, count);
                if (num >= 0)
                {
                    return num - this._baseIndex;
                }
                return -1;
            }
            public override void Insert(int index, object value)
            {
                if (index < 0 || index > this._baseSize)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                this.InternalUpdateRange();
                this._baseList.Insert(this._baseIndex + index, value);
                this.InternalUpdateVersion();
                this._baseSize++;
            }
            public override void InsertRange(int index, ICollection c)
            {
                if (index < 0 || index > this._baseSize)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                if (c == null)
                {
                    throw new ArgumentNullException("c");
                }
                this.InternalUpdateRange();
                int count = c.Count;
                if (count > 0)
                {
                    this._baseList.InsertRange(this._baseIndex + index, c);
                    this._baseSize += count;
                    this.InternalUpdateVersion();
                }
            }
            public override int LastIndexOf(object value)
            {
                this.InternalUpdateRange();
                int num = this._baseList.LastIndexOf(value, this._baseIndex + this._baseSize - 1, this._baseSize);
                if (num >= 0)
                {
                    return num - this._baseIndex;
                }
                return -1;
            }
            public override int LastIndexOf(object value, int startIndex)
            {
                return this.LastIndexOf(value, startIndex, startIndex + 1);
            }
            public override int LastIndexOf(object value, int startIndex, int count)
            {
                this.InternalUpdateRange();
                if (this._baseSize == 0)
                {
                    return -1;
                }
                if (startIndex >= this._baseSize)
                {
                    throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_Index"));
                }
                if (startIndex < 0)
                {
                    throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                int num = this._baseList.LastIndexOf(value, this._baseIndex + startIndex, count);
                if (num >= 0)
                {
                    return num - this._baseIndex;
                }
                return -1;
            }
            public override void RemoveAt(int index)
            {
                if (index < 0 || index >= this._baseSize)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                this.InternalUpdateRange();
                this._baseList.RemoveAt(this._baseIndex + index);
                this.InternalUpdateVersion();
                this._baseSize--;
            }
            public override void RemoveRange(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._baseSize - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                this.InternalUpdateRange();
                if (count > 0)
                {
                    this._baseList.RemoveRange(this._baseIndex + index, count);
                    this.InternalUpdateVersion();
                    this._baseSize -= count;
                }
            }
            public override void Reverse(int index, int count)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._baseSize - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                this.InternalUpdateRange();
                this._baseList.Reverse(this._baseIndex + index, count);
                this.InternalUpdateVersion();
            }
            public override void SetRange(int index, ICollection c)
            {
                this.InternalUpdateRange();
                if (index < 0 || index >= this._baseSize)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                this._baseList.SetRange(this._baseIndex + index, c);
                if (c.Count > 0)
                {
                    this.InternalUpdateVersion();
                }
            }
            public override void Sort(int index, int count, IComparer comparer)
            {
                if (index < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (this._baseSize - index < count)
                {
                    throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
                }
                this.InternalUpdateRange();
                this._baseList.Sort(this._baseIndex + index, count, comparer);
                this.InternalUpdateVersion();
            }
            public override object[] ToArray()
            {
                this.InternalUpdateRange();
                object[] array = new object[this._baseSize];
                Array.Copy(this._baseList._items, this._baseIndex, array, 0, this._baseSize);
                return array;
            }
            [SecuritySafeCritical]
            public override Array ToArray(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }
                this.InternalUpdateRange();
                Array array = Array.CreateInstance(type, this._baseSize);
                this._baseList.CopyTo(this._baseIndex, array, 0, this._baseSize);
                return array;
            }
            public override void TrimToSize()
            {
                throw new NotSupportedException(GetMyResource("NotSupported_RangeCollection"));
            }
        }
        private object[] _items;
        private int _size;
        private int _version;

        private object _syncRoot;
        private static readonly object[] emptyArray = new object[0];
        private const int _defaultCapacity = 4;
        /// <summary>Gets or sets the number of elements that the <see cref="T:System.Collections.ArrayList" /> can contain.</summary>
        /// <returns>The number of elements that the <see cref="T:System.Collections.ArrayList" /> can contain.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <see cref="P:System.Collections.ArrayList.Capacity" /> is set to a value that is less than <see cref="P:System.Collections.ArrayList.Count" />.</exception>
        /// <exception cref="T:System.OutOfMemoryException">There is not enough memory available on the system.</exception>
        /// <filterpriority>1</filterpriority>
        public virtual int Capacity
        {
            get
            {
                return this._items.Length;
            }
            set
            {
                if (value < this._size)
                {
                    throw new ArgumentOutOfRangeException("value", GetMyResource("ArgumentOutOfRange_SmallCapacity"));
                }
                if (value != this._items.Length)
                {
                    if (value > 0)
                    {
                        object[] array = new object[value];
                        if (this._size > 0)
                        {
                            Array.Copy(this._items, 0, array, 0, this._size);
                        }
                        this._items = array;
                        return;
                    }
                    this._items = new object[4];
                }
            }
        }
        /// <summary>Gets the number of elements actually contained in the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>The number of elements actually contained in the <see cref="T:System.Collections.ArrayList" />.</returns>
        /// <filterpriority>1</filterpriority>
        public virtual int Count
        {
            get
            {
                return this._size;
            }
        }
        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.ArrayList" /> has a fixed size.</summary>
        /// <returns>true if the <see cref="T:System.Collections.ArrayList" /> has a fixed size; otherwise, false. The default is false.</returns>
        /// <filterpriority>2</filterpriority>
        public virtual bool IsFixedSize
        {
            get
            {
                return false;
            }
        }
        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.ArrayList" /> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.ArrayList" /> is read-only; otherwise, false. The default is false.</returns>
        /// <filterpriority>2</filterpriority>
        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ArrayList" /> is synchronized (thread safe).</summary>
        /// <returns>true if access to the <see cref="T:System.Collections.ArrayList" /> is synchronized (thread safe); otherwise, false. The default is false.</returns>
        /// <filterpriority>2</filterpriority>
        public virtual bool IsSynchronized
        {
            get
            {
                return false;
            }
        }
        /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ArrayList" />.</returns>
        /// <filterpriority>2</filterpriority>
        public virtual object SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }
        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <returns>The element at the specified index.</returns>
        /// <param name="index">The zero-based index of the element to get or set. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual object this[int index]
        {
            get
            {
                if (index < 0 || index >= this._size)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                return this._items[index];
            }
            set
            {
                if (index < 0 || index >= this._size)
                {
                    throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
                }
                this._items[index] = value;
                this._version++;
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "trash")]
        internal ArrayList(bool trash)
        {
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ArrayList" /> class that is empty and has the default initial capacity.</summary>
        public ArrayList()
        {
            this._items = ArrayList.emptyArray;
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ArrayList" /> class that is empty and has the specified initial capacity.</summary>
        /// <param name="capacity">The number of elements that the new list can initially store. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="capacity" /> is less than zero. </exception>
        public ArrayList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", GetMyResource("ArgumentOutOfRange_MustBeNonNegNum", new object[]
                {
                    "capacity"
                }));
            }
            if (capacity == 0)
            {
                this._items = ArrayList.emptyArray;
                return;
            }
            this._items = new object[capacity];
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ArrayList" /> class that contains elements copied from the specified collection and that has the same initial capacity as the number of elements copied.</summary>
        /// <param name="c">The <see cref="T:System.Collections.ICollection" /> whose elements are copied to the new list. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="c" /> is null. </exception>
        public ArrayList(ICollection c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("c", GetMyResource("ArgumentNull_Collection"));
            }
            int count = c.Count;
            if (count == 0)
            {
                this._items = ArrayList.emptyArray;
                return;
            }
            this._items = new object[count];
            this.AddRange(c);
        }
        /// <summary>Creates an <see cref="T:System.Collections.ArrayList" /> wrapper for a specific <see cref="T:System.Collections.IList" />.</summary>
        /// <returns>The <see cref="T:System.Collections.ArrayList" /> wrapper around the <see cref="T:System.Collections.IList" />.</returns>
        /// <param name="list">The <see cref="T:System.Collections.IList" /> to wrap.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="list" /> is null.</exception>
        /// <filterpriority>2</filterpriority>
        public static ArrayList Adapter(IList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new ArrayList.IListWrapper(list);
        }
        /// <summary>Adds an object to the end of the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>The <see cref="T:System.Collections.ArrayList" /> index at which the <paramref name="value" /> has been added.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to be added to the end of the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual int Add(object value)
        {
            if (this._size == this._items.Length)
            {
                this.EnsureCapacity(this._size + 1);
            }
            this._items[this._size] = value;
            this._version++;
            return this._size++;
        }
        /// <summary>Adds the elements of an <see cref="T:System.Collections.ICollection" /> to the end of the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <param name="c">The <see cref="T:System.Collections.ICollection" /> whose elements should be added to the end of the <see cref="T:System.Collections.ArrayList" />. The collection itself cannot be null, but it can contain elements that are null. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="c" /> is null. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual void AddRange(ICollection c)
        {
            this.InsertRange(this._size, c);
        }
        /// <summary>Searches a range of elements in the sorted <see cref="T:System.Collections.ArrayList" /> for an element using the specified comparer and returns the zero-based index of the element.</summary>
        /// <returns>The zero-based index of <paramref name="value" /> in the sorted <see cref="T:System.Collections.ArrayList" />, if <paramref name="value" /> is found; otherwise, a negative number, which is the bitwise complement of the index of the next element that is larger than <paramref name="value" /> or, if there is no larger element, the bitwise complement of <see cref="P:System.Collections.ArrayList.Count" />.</returns>
        /// <param name="index">The zero-based starting index of the range to search. </param>
        /// <param name="count">The length of the range to search. </param>
        /// <param name="value">The <see cref="T:System.Object" /> to locate. The value can be null. </param>
        /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- null to use the default comparer that is the <see cref="T:System.IComparable" /> implementation of each element. </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in the <see cref="T:System.Collections.ArrayList" />.-or- <paramref name="comparer" /> is null and neither <paramref name="value" /> nor the elements of <see cref="T:System.Collections.ArrayList" /> implement the <see cref="T:System.IComparable" /> interface. </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="comparer" /> is null and <paramref name="value" /> is not of the same type as the elements of the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual int BinarySearch(int index, int count, object value, IComparer comparer)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (this._size - index < count)
            {
                throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
            }
            return Array.BinarySearch(this._items, index, count, value, comparer);
        }
        /// <summary>Searches the entire sorted <see cref="T:System.Collections.ArrayList" /> for an element using the default comparer and returns the zero-based index of the element.</summary>
        /// <returns>The zero-based index of <paramref name="value" /> in the sorted <see cref="T:System.Collections.ArrayList" />, if <paramref name="value" /> is found; otherwise, a negative number, which is the bitwise complement of the index of the next element that is larger than <paramref name="value" /> or, if there is no larger element, the bitwise complement of <see cref="P:System.Collections.ArrayList.Count" />.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to locate. The value can be null. </param>
        /// <exception cref="T:System.ArgumentException">Neither <paramref name="value" /> nor the elements of <see cref="T:System.Collections.ArrayList" /> implement the <see cref="T:System.IComparable" /> interface. </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="value" /> is not of the same type as the elements of the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual int BinarySearch(object value)
        {
            return this.BinarySearch(0, this.Count, value, null);
        }
        /// <summary>Searches the entire sorted <see cref="T:System.Collections.ArrayList" /> for an element using the specified comparer and returns the zero-based index of the element.</summary>
        /// <returns>The zero-based index of <paramref name="value" /> in the sorted <see cref="T:System.Collections.ArrayList" />, if <paramref name="value" /> is found; otherwise, a negative number, which is the bitwise complement of the index of the next element that is larger than <paramref name="value" /> or, if there is no larger element, the bitwise complement of <see cref="P:System.Collections.ArrayList.Count" />.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to locate. The value can be null. </param>
        /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- null to use the default comparer that is the <see cref="T:System.IComparable" /> implementation of each element. </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="comparer" /> is null and neither <paramref name="value" /> nor the elements of <see cref="T:System.Collections.ArrayList" /> implement the <see cref="T:System.IComparable" /> interface. </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="comparer" /> is null and <paramref name="value" /> is not of the same type as the elements of the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual int BinarySearch(object value, IComparer comparer)
        {
            return this.BinarySearch(0, this.Count, value, comparer);
        }
        /// <summary>Removes all elements from the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual void Clear()
        {
            if (this._size > 0)
            {
                Array.Clear(this._items, 0, this._size);
                this._size = 0;
            }
            this._version++;
        }
        /// <summary>Creates a shallow copy of the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>A shallow copy of the <see cref="T:System.Collections.ArrayList" />.</returns>
        /// <filterpriority>2</filterpriority>
        public virtual object Clone()
        {
            ArrayList arrayList = new ArrayList(this._size);
            arrayList._size = this._size;
            arrayList._version = this._version;
            Array.Copy(this._items, 0, arrayList._items, 0, this._size);
            return arrayList;
        }
        /// <summary>Determines whether an element is in the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.ArrayList" />; otherwise, false.</returns>
        /// <param name="item">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <filterpriority>1</filterpriority>
        public virtual bool Contains(object item)
        {
            if (item == null)
            {
                for (int i = 0; i < this._size; i++)
                {
                    if (this._items[i] == null)
                    {
                        return true;
                    }
                }
                return false;
            }
            for (int j = 0; j < this._size; j++)
            {
                if (this._items[j] != null && this._items[j].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>Copies the entire <see cref="T:System.Collections.ArrayList" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the beginning of the target array.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ArrayList" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is null. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ArrayList" /> is greater than the number of elements that the destination <paramref name="array" /> can contain. </exception>
        /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ArrayList" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual void CopyTo(Array array)
        {
            this.CopyTo(array, 0);
        }
        /// <summary>Copies the entire <see cref="T:System.Collections.ArrayList" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ArrayList" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="arrayIndex" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ArrayList" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />. </exception>
        /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ArrayList" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual void CopyTo(Array array, int arrayIndex)
        {
            if (array != null && array.Rank != 1)
            {
                throw new ArgumentException(GetMyResource("Arg_RankMultiDimNotSupported"));
            }
            Array.Copy(this._items, 0, array, arrayIndex, this._size);
        }
        /// <summary>Copies a range of elements from the <see cref="T:System.Collections.ArrayList" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
        /// <param name="index">The zero-based index in the source <see cref="T:System.Collections.ArrayList" /> at which copying begins. </param>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ArrayList" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins. </param>
        /// <param name="count">The number of elements to copy. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="arrayIndex" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" /> is multidimensional.-or- <paramref name="index" /> is equal to or greater than the <see cref="P:System.Collections.ArrayList.Count" /> of the source <see cref="T:System.Collections.ArrayList" />.-or- The number of elements from <paramref name="index" /> to the end of the source <see cref="T:System.Collections.ArrayList" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />. </exception>
        /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ArrayList" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual void CopyTo(int index, Array array, int arrayIndex, int count)
        {
            if (this._size - index < count)
            {
                throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
            }
            if (array != null && array.Rank != 1)
            {
                throw new ArgumentException(GetMyResource("Arg_RankMultiDimNotSupported"));
            }
            Array.Copy(this._items, index, array, arrayIndex, count);
        }
        /// <summary>Returns an <see cref="T:System.Collections.IList" /> wrapper with a fixed size.</summary>
        /// <returns>An <see cref="T:System.Collections.IList" /> wrapper with a fixed size.</returns>
        /// <param name="list">The <see cref="T:System.Collections.IList" /> to wrap. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="list" /> is null. </exception>
        /// <filterpriority>2</filterpriority>
        public static IList FixedSize(IList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new ArrayList.FixedSizeList(list);
        }
        /// <summary>Returns an <see cref="T:System.Collections.ArrayList" /> wrapper with a fixed size.</summary>
        /// <returns>An <see cref="T:System.Collections.ArrayList" /> wrapper with a fixed size.</returns>
        /// <param name="list">The <see cref="T:System.Collections.ArrayList" /> to wrap. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="list" /> is null. </exception>
        /// <filterpriority>2</filterpriority>
        public static ArrayList FixedSize(ArrayList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new ArrayList.FixedSizeArrayList(list);
        }
        /// <summary>Returns an enumerator for the entire <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the entire <see cref="T:System.Collections.ArrayList" />.</returns>
        /// <filterpriority>2</filterpriority>
        public virtual IEnumerator GetEnumerator()
        {
            return new ArrayList.ArrayListEnumeratorSimple(this);
        }
        /// <summary>Returns an enumerator for a range of elements in the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the specified range of elements in the <see cref="T:System.Collections.ArrayList" />.</returns>
        /// <param name="index">The zero-based starting index of the <see cref="T:System.Collections.ArrayList" /> section that the enumerator should refer to. </param>
        /// <param name="count">The number of elements in the <see cref="T:System.Collections.ArrayList" /> section that the enumerator should refer to. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="index" /> and <paramref name="count" /> do not specify a valid range in the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual IEnumerator GetEnumerator(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (this._size - index < count)
            {
                throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
            }
            return new ArrayList.ArrayListEnumerator(this, index, count);
        }
        /// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the entire <see cref="T:System.Collections.ArrayList" />, if found; otherwise, -1.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <filterpriority>1</filterpriority>
        public virtual int IndexOf(object value)
        {
            return Array.IndexOf(this._items, value, 0, this._size);
        }
        /// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that extends from the specified index to the last element.</summary>
        /// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that extends from <paramref name="startIndex" /> to the last element, if found; otherwise, -1.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual int IndexOf(object value, int startIndex)
        {
            if (startIndex > this._size)
            {
                throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_Index"));
            }
            return Array.IndexOf(this._items, value, startIndex, this._size - startIndex);
        }
        /// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that starts at the specified index and contains the specified number of elements.</summary>
        /// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that starts at <paramref name="startIndex" /> and contains <paramref name="count" /> number of elements, if found; otherwise, -1.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.ArrayList" />.-or- <paramref name="count" /> is less than zero.-or- <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual int IndexOf(object value, int startIndex, int count)
        {
            if (startIndex > this._size)
            {
                throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_Index"));
            }
            if (count < 0 || startIndex > this._size - count)
            {
                throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_Count"));
            }
            return Array.IndexOf(this._items, value, startIndex, count);
        }
        /// <summary>Inserts an element into the <see cref="T:System.Collections.ArrayList" /> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted. </param>
        /// <param name="value">The <see cref="T:System.Object" /> to insert. The value can be null. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual void Insert(int index, object value)
        {
            if (index < 0 || index > this._size)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_ArrayListInsert"));
            }
            if (this._size == this._items.Length)
            {
                this.EnsureCapacity(this._size + 1);
            }
            if (index < this._size)
            {
                Array.Copy(this._items, index, this._items, index + 1, this._size - index);
            }
            this._items[index] = value;
            this._size++;
            this._version++;
        }
        /// <summary>Inserts the elements of a collection into the <see cref="T:System.Collections.ArrayList" /> at the specified index.</summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted. </param>
        /// <param name="c">The <see cref="T:System.Collections.ICollection" /> whose elements should be inserted into the <see cref="T:System.Collections.ArrayList" />. The collection itself cannot be null, but it can contain elements that are null. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="c" /> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual void InsertRange(int index, ICollection c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("c", GetMyResource("ArgumentNull_Collection"));
            }
            if (index < 0 || index > this._size)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
            }
            int count = c.Count;
            if (count > 0)
            {
                this.EnsureCapacity(this._size + count);
                if (index < this._size)
                {
                    Array.Copy(this._items, index, this._items, index + count, this._size - index);
                }
                object[] array = new object[count];
                c.CopyTo(array, 0);
                array.CopyTo(this._items, index);
                this._size += count;
                this._version++;
            }
        }
        /// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the last occurrence within the entire <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the entire the <see cref="T:System.Collections.ArrayList" />, if found; otherwise, -1.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <filterpriority>2</filterpriority>
        public virtual int LastIndexOf(object value)
        {
            return this.LastIndexOf(value, this._size - 1, this._size);
        }
        /// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that extends from the first element to the specified index.</summary>
        /// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that extends from the first element to <paramref name="startIndex" />, if found; otherwise, -1.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <param name="startIndex">The zero-based starting index of the backward search. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual int LastIndexOf(object value, int startIndex)
        {
            if (startIndex >= this._size)
            {
                throw new ArgumentOutOfRangeException("startIndex", GetMyResource("ArgumentOutOfRange_Index"));
            }
            return this.LastIndexOf(value, startIndex, startIndex + 1);
        }
        /// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that contains the specified number of elements and ends at the specified index.</summary>
        /// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that contains <paramref name="count" /> number of elements and ends at <paramref name="startIndex" />, if found; otherwise, -1.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <param name="startIndex">The zero-based starting index of the backward search. </param>
        /// <param name="count">The number of elements in the section to search. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.ArrayList" />.-or- <paramref name="count" /> is less than zero.-or- <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual int LastIndexOf(object value, int startIndex, int count)
        {
            if (this.Count != 0 && (startIndex < 0 || count < 0))
            {
                throw new ArgumentOutOfRangeException((startIndex < 0) ? "startIndex" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (this._size == 0)
            {
                return -1;
            }
            if (startIndex >= this._size || count > startIndex + 1)
            {
                throw new ArgumentOutOfRangeException((startIndex >= this._size) ? "startIndex" : "count", GetMyResource("ArgumentOutOfRange_BiggerThanCollection"));
            }
            return Array.LastIndexOf(this._items, value, startIndex, count);
        }
        /// <summary>Returns a read-only <see cref="T:System.Collections.IList" /> wrapper.</summary>
        /// <returns>A read-only <see cref="T:System.Collections.IList" /> wrapper around <paramref name="list" />.</returns>
        /// <param name="list">The <see cref="T:System.Collections.IList" /> to wrap. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="list" /> is null. </exception>
        /// <filterpriority>2</filterpriority>
        public static IList ReadOnly(IList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new ArrayList.ReadOnlyList(list);
        }
        /// <summary>Returns a read-only <see cref="T:System.Collections.ArrayList" /> wrapper.</summary>
        /// <returns>A read-only <see cref="T:System.Collections.ArrayList" /> wrapper around <paramref name="list" />.</returns>
        /// <param name="list">The <see cref="T:System.Collections.ArrayList" /> to wrap. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="list" /> is null. </exception>
        /// <filterpriority>2</filterpriority>
        public static ArrayList ReadOnly(ArrayList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new ArrayList.ReadOnlyArrayList(list);
        }
        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual void Remove(object obj)
        {
            int num = this.IndexOf(obj);
            if (num >= 0)
            {
                this.RemoveAt(num);
            }
        }
        /// <summary>Removes the element at the specified index of the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <param name="index">The zero-based index of the element to remove. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= this._size)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
            }
            this._size--;
            if (index < this._size)
            {
                Array.Copy(this._items, index + 1, this._items, index, this._size - index);
            }
            this._items[this._size] = null;
            this._version++;
        }
        /// <summary>Removes a range of elements from the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove. </param>
        /// <param name="count">The number of elements to remove. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="index" /> and <paramref name="count" /> do not denote a valid range of elements in the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual void RemoveRange(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (this._size - index < count)
            {
                throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
            }
            if (count > 0)
            {
                int i = this._size;
                this._size -= count;
                if (index < this._size)
                {
                    Array.Copy(this._items, index + count, this._items, index, this._size - index);
                }
                while (i > this._size)
                {
                    this._items[--i] = null;
                }
                this._version++;
            }
        }
        /// <summary>Returns an <see cref="T:System.Collections.ArrayList" /> whose elements are copies of the specified value.</summary>
        /// <returns>An <see cref="T:System.Collections.ArrayList" /> with <paramref name="count" /> number of elements, all of which are copies of <paramref name="value" />.</returns>
        /// <param name="value">The <see cref="T:System.Object" /> to copy multiple times in the new <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
        /// <param name="count">The number of times <paramref name="value" /> should be copied. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="count" /> is less than zero. </exception>
        /// <filterpriority>2</filterpriority>
        public static ArrayList Repeat(object value, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            ArrayList arrayList = new ArrayList((count > 4) ? count : 4);
            for (int i = 0; i < count; i++)
            {
                arrayList.Add(value);
            }
            return arrayList;
        }
        /// <summary>Reverses the order of the elements in the entire <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual void Reverse()
        {
            this.Reverse(0, this.Count);
        }
        /// <summary>Reverses the order of the elements in the specified range.</summary>
        /// <param name="index">The zero-based starting index of the range to reverse. </param>
        /// <param name="count">The number of elements in the range to reverse. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="index" /> and <paramref name="count" /> do not denote a valid range of elements in the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual void Reverse(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (this._size - index < count)
            {
                throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
            }
            Array.Reverse(this._items, index, count);
            this._version++;
        }
        /// <summary>Copies the elements of a collection over a range of elements in the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <param name="index">The zero-based <see cref="T:System.Collections.ArrayList" /> index at which to start copying the elements of <paramref name="c" />. </param>
        /// <param name="c">The <see cref="T:System.Collections.ICollection" /> whose elements to copy to the <see cref="T:System.Collections.ArrayList" />. The collection itself cannot be null, but it can contain elements that are null. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> plus the number of elements in <paramref name="c" /> is greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="c" /> is null. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual void SetRange(int index, ICollection c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("c", GetMyResource("ArgumentNull_Collection"));
            }
            int count = c.Count;
            if (index < 0 || index > this._size - count)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_Index"));
            }
            if (count > 0)
            {
                c.CopyTo(this._items, index);
                this._version++;
            }
        }
        /// <summary>Returns an <see cref="T:System.Collections.ArrayList" /> which represents a subset of the elements in the source <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <returns>An <see cref="T:System.Collections.ArrayList" /> which represents a subset of the elements in the source <see cref="T:System.Collections.ArrayList" />.</returns>
        /// <param name="index">The zero-based <see cref="T:System.Collections.ArrayList" /> index at which the range starts. </param>
        /// <param name="count">The number of elements in the range. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="index" /> and <paramref name="count" /> do not denote a valid range of elements in the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual ArrayList GetRange(int index, int count)
        {
            if (index < 0 || count < 0)
            {
                throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (this._size - index < count)
            {
                throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
            }
            return new ArrayList.Range(this, index, count);
        }
        /// <summary>Sorts the elements in the entire <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
        /// <filterpriority>1</filterpriority>
        public virtual void Sort()
        {
            this.Sort(0, this.Count, null); //Comparer.Default);
        }
        /// <summary>Sorts the elements in the entire <see cref="T:System.Collections.ArrayList" /> using the specified comparer.</summary>
        /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- A null reference (Nothing in Visual Basic) to use the <see cref="T:System.IComparable" /> implementation of each element. </param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
        /// <exception cref="T:System.InvalidOperationException">An error occurred while comparing two elements.</exception>
        /// <filterpriority>1</filterpriority>
        public virtual void Sort(IComparer comparer)
        {
            this.Sort(0, this.Count, comparer);
        }
        /// <summary>Sorts the elements in a range of elements in <see cref="T:System.Collections.ArrayList" /> using the specified comparer.</summary>
        /// <param name="index">The zero-based starting index of the range to sort. </param>
        /// <param name="count">The length of the range to sort. </param>
        /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- A null reference (Nothing in Visual Basic) to use the <see cref="T:System.IComparable" /> implementation of each element. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="index" /> and <paramref name="count" /> do not specify a valid range in the <see cref="T:System.Collections.ArrayList" />. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
        /// <exception cref="T:System.InvalidOperationException">An error occurred while comparing two elements.</exception>
        /// <filterpriority>1</filterpriority>
        public virtual void Sort(int index, int count, IComparer comparer)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", GetMyResource("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (this._size - index < count)
            {
                throw new ArgumentException(GetMyResource("Argument_InvalidOffLen"));
            }
            Array.Sort(this._items, index, count, comparer);
            this._version++;
        }
        /// <summary>Returns an <see cref="T:System.Collections.IList" /> wrapper that is synchronized (thread safe).</summary>
        /// <returns>An <see cref="T:System.Collections.IList" /> wrapper that is synchronized (thread safe).</returns>
        /// <param name="list">The <see cref="T:System.Collections.IList" /> to synchronize. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="list" /> is null. </exception>
        /// <filterpriority>2</filterpriority>
        public static IList Synchronized(IList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new ArrayList.SyncIList(list);
        }
        /// <summary>Returns an <see cref="T:System.Collections.ArrayList" /> wrapper that is synchronized (thread safe).</summary>
        /// <returns>An <see cref="T:System.Collections.ArrayList" /> wrapper that is synchronized (thread safe).</returns>
        /// <param name="list">The <see cref="T:System.Collections.ArrayList" /> to synchronize. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="list" /> is null. </exception>
        /// <filterpriority>2</filterpriority>
        public static ArrayList Synchronized(ArrayList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new ArrayList.SyncArrayList(list);
        }
        /// <summary>Copies the elements of the <see cref="T:System.Collections.ArrayList" /> to a new <see cref="T:System.Object" /> array.</summary>
        /// <returns>An <see cref="T:System.Object" /> array containing copies of the elements of the <see cref="T:System.Collections.ArrayList" />.</returns>
        /// <filterpriority>1</filterpriority>
        public virtual object[] ToArray()
        {
            object[] array = new object[this._size];
            Array.Copy(this._items, 0, array, 0, this._size);
            return array;
        }
        /// <summary>Copies the elements of the <see cref="T:System.Collections.ArrayList" /> to a new array of the specified element type.</summary>
        /// <returns>An array of the specified element type containing copies of the elements of the <see cref="T:System.Collections.ArrayList" />.</returns>
        /// <param name="type">The element <see cref="T:System.Type" /> of the destination array to create and copy elements to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> is null. </exception>
        /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ArrayList" /> cannot be cast automatically to the specified type. </exception>
        /// <filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        public virtual Array ToArray(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            Array array = Array.CreateInstance(type, this._size);
            Array.Copy(this._items, 0, array, 0, this._size);
            return array;
        }
        /// <summary>Sets the capacity to the actual number of elements in the <see cref="T:System.Collections.ArrayList" />.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
        /// <filterpriority>2</filterpriority>
        public virtual void TrimToSize()
        {
            this.Capacity = this._size;
        }
        private void EnsureCapacity(int min)
        {
            if (this._items.Length < min)
            {
                int num = (this._items.Length == 0) ? 4 : (this._items.Length * 2);
                if (num > 2146435071)
                {
                    num = 2146435071;
                }
                if (num < min)
                {
                    num = min;
                }
                this.Capacity = num;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args")]
        public static string GetMyResource(string text, params object[] args)
        {
            return text;
        }

    }

}
