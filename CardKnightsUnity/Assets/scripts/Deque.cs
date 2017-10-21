
// Author: Tom Jacques, VP Engineering OKCupid
// Source: https://github.com/tejacques/Deque/tree/master/src/Deque

using DequeUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DequeUtility
{
    internal static class Utility
    {
        public static int ClosestPowerOfTwoGreaterThan(int x)
        {
            x--;
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (x + 1);
        }

        
        /// Jon Skeet's excellent reimplementation of LINQ Count.
        
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <param name="source">The source IEnumerable.</param>
        // The number of items in the source.
        public static int Count<TSource>(IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            // Optimization for ICollection<T> 
            ICollection<TSource> genericCollection = source as ICollection<TSource>;
            if (genericCollection != null)
            {
                return genericCollection.Count;
            }

            // Optimization for ICollection 
            ICollection nonGenericCollection = source as ICollection;
            if (nonGenericCollection != null)
            {
                return nonGenericCollection.Count;
            }

            // Do it the slow way - and make sure we overflow appropriately 
            checked
            {
                int count = 0;
                using (var iterator = source.GetEnumerator())
                {
                    while (iterator.MoveNext())
                    {
                        count++;
                    }
                }
                return count;
            }
        }

    } // end class Utility

}  // end namespace DequeUtility


/////////////////////////////////////////////////////////////////////////////////////////////////

// Add class to namespace System.Collections.Generic
namespace System.Collections.Generic
{
    
    /// A genetic Deque class. It can be thought of as
    /// a double-ended queue, hence Deque. This allows for
    /// an O(1) AddFront, AddBack, RemoveFront, RemoveBack.
    /// The Deque also has O(1) indexed lookup, as it is backed
    /// by a circular array.
    
    /// <typeparam name="T">
    /// The type of objects to store in the deque.
    /// </typeparam>
    public class Deque<T> : IList<T>
    {
        /// The default capacity of the deque.        
        private const int defaultCapacity = 16;
    
        /// The first element offset from the beginning of the data array.       
        private int startOffset;

        /// The circular array holding the items.       
        private T[] buffer;
       
        /// Creates a new instance of the Deque class with
        /// the default capacity.
        public Deque() : this(defaultCapacity) { }
        
        /// Creates a new instance of the Deque class with
        /// the specified capacity.       
        /// <param name="capacity">The initial capacity of the Deque.</param>
        public Deque(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "capacity", "capacity is less than 0.");
            }
            this.Capacity = capacity;
        }

        
        /// Create a new instance of the Deque class with the elements
        /// from the specified collection.        
        /// <param name="collection">The co</param>
        public Deque(IEnumerable<T> collection)
            : this(Utility.Count(collection))
        {
            InsertRange(0, collection);
        }

        private int capacityClosestPowerOfTwoMinusOne;

        
        /// Gets or sets the total number of elements
        /// the internal array can hold without resizing.        
        public int Capacity
        {
            get
            {
                return buffer.Length;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Capacity is less than 0.");
                }
                else if (value < this.Count)
                {
                    throw new InvalidOperationException("Capacity cannot be set to a value less than Count");
                }
                else if (null != buffer && value == buffer.Length)
                {
                    return;
                }

                // Create a new array and copy the old values.
                int powOfTwo = Utility.ClosestPowerOfTwoGreaterThan(value);

                value = powOfTwo;

                T[] newBuffer = new T[value];
                this.CopyTo(newBuffer, 0);

                // Set up to use the new buffer.
                buffer = newBuffer;
                startOffset = 0;
                this.capacityClosestPowerOfTwoMinusOne = powOfTwo - 1;
            }
        }

        
        /// Gets whether or not the Deque is filled to capacity.       
        public bool IsFull
        {
            get { return this.Count == this.Capacity; }
        }

        
        /// Gets whether or not the Deque is empty.       
        public bool IsEmpty
        {
            get { return 0 == this.Count; }
        }

        private void ensureCapacityFor(int numElements)
        {
            if (this.Count + numElements > this.Capacity)
            {
                this.Capacity = this.Count + numElements;
            }
        }

        private int toBufferIndex(int index)
        {
            int bufferIndex;

            bufferIndex = (index + this.startOffset)
                & this.capacityClosestPowerOfTwoMinusOne;

            return bufferIndex;
        }

        private void checkIndexOutOfRange(int index)
        {
            if (index >= this.Count)
            {
                throw new IndexOutOfRangeException(
                    "The supplied index is greater than the Count");
            }
        }

        private static void checkArgumentsOutOfRange(
            int length,
            int offset,
            int count)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "offset", "Invalid offset " + offset);
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "count", "Invalid count " + count);
            }

            if (length - offset < count)
            {
                throw new ArgumentException(
                    String.Format(
                    "Invalid offset ({0}) or count + ({1}) "
                    + "for source length {2}",
                    offset, count, length));
            }
        }

        private int shiftStartOffset(int value)
        {
            this.startOffset = toBufferIndex(value);

            return this.startOffset;
        }

        private int preShiftStartOffset(int value)
        {
            int offset = this.startOffset;
            this.shiftStartOffset(value);
            return offset;
        }

        private int postShiftStartOffset(int value)
        {
            return shiftStartOffset(value);
        }

        #region IEnumerable

        
        /// Returns an enumerator that iterates through the Deque.
        /// An iterator that can be used to iterate through the Deque.        
        public IEnumerator<T> GetEnumerator()
        {

            // The below is done for performance reasons.
            // Rather than doing bounds checking and modulo arithmetic
            // that would go along with calls to Get(index), we can skip
            // all of that by referencing the underlying array.
            if (this.startOffset + this.Count > this.Capacity)
            {
                for (int i = this.startOffset; i < this.Capacity; i++)
                {
                    yield return buffer[i];
                }

                int endIndex = toBufferIndex(this.Count);
                for (int i = 0; i < endIndex; i++)
                {
                    yield return buffer[i];
                }
            }
            else
            {
                int endIndex = this.startOffset + this.Count;
                for (int i = this.startOffset; i < endIndex; i++)
                {
                    yield return buffer[i];
                }
            }
        }

        
        /// Returns an enumerator that iterates through the Deque.      
        /// An iterator that can be used to iterate through the Deque.        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region ICollection
        
        /// Gets a value indicating whether the Deque is read-only.       
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        
        /// Gets the number of elements contained in the Deque.        
        public int Count
        {
            get;
            private set;
        }

        private void incrementCount(int value)
        {
            this.Count = this.Count + value;
        }

        private void decrementCount(int value)
        {
            this.Count = Math.Max(this.Count - value, 0);
        }

        
        /// Adds an item to the Deque.       
        /// <param name="item">The object to add to the Deque.</param>
        public void Add(T item)
        {
            AddBack(item);
        }

        private void ClearBuffer(int logicalIndex, int length)
        {
            int offset = toBufferIndex(logicalIndex);
            if (offset + length > this.Capacity)
            {
                int len = this.Capacity - offset;
                Array.Clear(this.buffer, offset, len);

                len = toBufferIndex(logicalIndex + length);
                Array.Clear(this.buffer, 0, len);
            }
            else
            {
                Array.Clear(this.buffer, offset, length);
            }
        }

        
        /// Removes all items from the Deque.        
        public void Clear()
        {
            if (this.Count > 0)
            {
                ClearBuffer(0, this.Count);
            }
            this.Count = 0;
            this.startOffset = 0;
        }

        
        /// Determines whether the Deque contains a specific value.       
        /// <param name="item">The object to locate in the Deque.</param>      
        /// true if item is found in the Deque; otherwise, false.       
        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        
        ///     Copies the elements of the Deque to a System.Array,
        ///     starting at a particular System.Array index.
        
        /// <param name="array">
        ///     The one-dimensional System.Array that is the destination of
        ///     the elements copied from the Deque. The System.Array must
        ///     have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        ///     The zero-based index in array at which copying begins.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     array is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     arrayIndex is less than 0.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     The number of elements in the source Deque is greater than
        ///     the available space from arrayIndex to the end of the
        ///     destination array.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (null == array)
            {
                throw new ArgumentNullException("array", "Array is null");
            }

            // Nothing to copy
            if (null == this.buffer)
            {
                return;
            }

            checkArgumentsOutOfRange(array.Length, arrayIndex, this.Count);

            if (0 != this.startOffset
                && this.startOffset + this.Count >= this.Capacity)
            {
                int lengthFromStart = this.Capacity - this.startOffset;
                int lengthFromEnd = this.Count - lengthFromStart;

                Array.Copy(
                    buffer, this.startOffset, array, 0, lengthFromStart);

                Array.Copy(
                    buffer, 0, array, lengthFromStart, lengthFromEnd);
            }
            else
            {
                Array.Copy(
                    buffer, this.startOffset, array, 0, Count);
            }
        }

        
        /// Removes the first occurrence of a specific object from the Deque.
        
        /// <param name="item">The object to remove from the Deque.</param>
        
        ///     true if item was successfully removed from the Deque;
        ///     otherwise, false. This method also returns false if item
        ///     is not found in the original
        
        public bool Remove(T item)
        {
            bool result = true;
            int index = IndexOf(item);

            if (-1 == index)
            {
                result = false;
            }
            else
            {
                RemoveAt(index);
            }

            return result;
        }

        #endregion

        #region List<T>
        
        /// Gets or sets the element at the specified index.        
        /// <param name="index">
        ///     The zero-based index of the element to get or set.
        /// </param>
        // The element at the specified index</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is not a valid index in this deque
        /// </exception>
        public T this[int index]
        {
            get
            {
                return this.Get(index);
            }

            set
            {
                this.Set(index, value);
            }
        }

        
        /// Inserts an item to the Deque at the specified index.
        
        /// <param name="index">
        /// The zero-based index at which item should be inserted.
        /// </param>
        /// <param name="item">The object to insert into the Deque.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the Deque.
        /// </exception>
        public void Insert(int index, T item)
        {
            ensureCapacityFor(1);

            if (index == 0)
            {
                AddFront(item);
                return;
            }
            else if (index == Count)
            {
                AddBack(item);
                return;
            }

            InsertRange(index, new[] { item });
        }

        
        /// Determines the index of a specific item in the deque.        
        /// <param name="item">The object to locate in the deque.</param>        
        /// The index of the item if found in the deque; otherwise, -1.       
        public int IndexOf(T item)
        {
            int index = 0;
            foreach (var myItem in this)
            {
                if (myItem.Equals(item))
                {
                    break;
                }
                ++index;
            }

            if (index == this.Count)
            {
                index = -1;
            }
            return index;
        }

        
        /// Removes the item at the specified index.    
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the Deque.
        /// </exception>
        public void RemoveAt(int index)
        {
            if (index == 0)
            {
                RemoveFront();
                return;
            }
            else if (index == Count - 1)
            {
                RemoveBack();
                return;
            }

            RemoveRange(index, 1);
        }
        #endregion

        
        /// Adds the provided item to the front of the Deque.        
        /// <param name="item">The item to add.</param>
        public void AddFront(T item)
        {
            ensureCapacityFor(1);
            buffer[postShiftStartOffset(-1)] = item;
            incrementCount(1);
        }

        
        /// Adds the provided item to the back of the Deque.       
        /// <param name="item">The item to add.</param>
        public void AddBack(T item)
        {
            ensureCapacityFor(1);
            buffer[toBufferIndex(this.Count)] = item;
            incrementCount(1);
        }

        
        /// Removes an item from the front of the Deque and returns it.        
        // The item at the front of the Deque.
        public T RemoveFront()
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException("The Deque is empty");
            }

            T result = buffer[this.startOffset];
            buffer[preShiftStartOffset(1)] = default(T);
            decrementCount(1);
            return result;
        }

        
        /// Removes an item from the back of the Deque and returns it.        
        // The item in the back of the Deque.
        public T RemoveBack()
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException("The Deque is empty");
            }

            decrementCount(1);
            int endIndex = toBufferIndex(this.Count);
            T result = buffer[endIndex];
            buffer[endIndex] = default(T);

            return result;
        }

        
        /// Adds a collection of items to the Deque.
        
        /// <param name="collection">The collection to add.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            AddBackRange(collection);
        }

        
        /// Adds a collection of items to the front of the Deque.
        
        /// <param name="collection">The collection to add.</param>
        public void AddFrontRange(IEnumerable<T> collection)
        {
            AddFrontRange(collection, 0, Utility.Count(collection));
        }

        
        /// Adds count items from a collection of items
        /// from a specified index to the Deque.
        
        /// <param name="collection">The collection to add.</param>
        /// <param name="fromIndex">
        /// The index in the collection to begin adding from.
        /// </param>
        /// <param name="count">
        /// The number of items in the collection to add.
        /// </param>
        public void AddFrontRange(
            IEnumerable<T> collection,
            int fromIndex,
            int count)
        {
            InsertRange(0, collection, fromIndex, count);
        }

        
        /// Adds a collection of items to the back of the Deque.
        
        /// <param name="collection">The collection to add.</param>
        public void AddBackRange(IEnumerable<T> collection)
        {
            AddBackRange(collection, 0, Utility.Count(collection));
        }

        
        /// Adds count items from a collection of items
        /// from a specified index to the back of the Deque.
        
        /// <param name="collection">The collection to add.</param>
        /// <param name="fromIndex">
        /// The index in the collection to begin adding from.
        /// </param>
        /// <param name="count">
        /// The number of items in the collection to add.
        /// </param>
        public void AddBackRange(
            IEnumerable<T> collection,
            int fromIndex,
            int count)
        {
            InsertRange(this.Count, collection, fromIndex, count);
        }

        
        /// Inserts a collection of items into the Deque
        /// at the specified index.
        
        /// <param name="index">
        /// The index in the Deque to insert the collection.
        /// </param>
        /// <param name="collection">The collection to add.</param>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            int count = Utility.Count(collection);
            this.InsertRange(index, collection, 0, count);
        }

        
        /// Inserts count items from a collection of items from a specified
        /// index into the Deque at the specified index.
        
        /// <param name="index">
        /// The index in the Deque to insert the collection.
        /// </param>
        /// <param name="collection">The collection to add.</param>
        /// <param name="fromIndex">
        /// The index in the collection to begin adding from.
        /// </param>
        /// <param name="count">
        /// The number of items in the colleciton to add.
        /// </param>
        public void InsertRange(int index, IEnumerable<T> collection, int fromIndex,int count)
        {
            checkIndexOutOfRange(index - 1);

            if (0 == count)
                return;

            // Make room
            ensureCapacityFor(count);

            if (index < this.Count / 2)
            {
                // Inserting into the first half of the list
                if (index > 0)
                {
                    // Move items down:
                    //  [0, index) -> 
                    //  [Capacity - count, Capacity - count + index)
                    int copyCount = index;
                    int shiftIndex = this.Capacity - count;
                    for (int j = 0; j < copyCount; j++)
                    {
                        buffer[toBufferIndex(shiftIndex + j)] =
                            buffer[toBufferIndex(j)];
                    }
                }
                // shift the starting offset
                this.shiftStartOffset(-count);
            }
            else
            {
                // Inserting into the second half of the list
                if (index < this.Count)
                {
                    // Move items up:
                    // [index, Count) -> [index + count, count + Count)
                    int copyCount = this.Count - index;
                    int shiftIndex = index + count;
                    for (int j = 0; j < copyCount; j++)
                    {
                        buffer[toBufferIndex(shiftIndex + j)] =
                            buffer[toBufferIndex(index + j)];
                    }
                }
            }

            // Copy new items into place
            int i = index;
            foreach (T item in collection)
            {
                buffer[toBufferIndex(i)] = item;
                ++i;
            }
            // Adjust valid count
            incrementCount(count);
        }

        
        ///     Removes a range of elements from the view.      
        /// <param name="index">
        ///     The index into the view at which the range begins.
        /// </param>
        /// <param name="count">
        ///     The number of elements in the range. This must be greater
        ///     than 0 and less than or equal to <see cref="Count"/>.
        /// </param>
        public void RemoveRange(int index, int count)
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException("The Deque is empty");
            }
            if (index > Count - count)
            {
                throw new IndexOutOfRangeException(
                    "The supplied index is greater than the Count");
            }

            // Clear out the underlying array
            ClearBuffer(index, count);

            if (index == 0)
            {
                // Removing from the beginning: shift the start offset
                this.shiftStartOffset(count);
                this.Count -= count;
                return;
            }
            else if (index == Count - count)
            {
                // Removing from the ending: trim the existing view
                this.Count -= count;
                return;
            }

            if ((index + (count / 2)) < Count / 2)
            {
                // Removing from first half of list

                // Move items up:
                //  [0, index) -> [count, count + index)
                int copyCount = index;
                int writeIndex = count;
                for (int j = 0; j < copyCount; j++)
                {
                    buffer[toBufferIndex(writeIndex + j)]
                        = buffer[toBufferIndex(j)];
                }
                // Rotate to new view
                this.shiftStartOffset(count);
            }
            else
            {
                // Removing from second half of list
                // Move items down:
                // [index + collectionCount, count) ->
                // [index, count - collectionCount)
                int copyCount = Count - count - index;
                int readIndex = index + count;
                for (int j = 0; j < copyCount; ++j)
                {
                    buffer[toBufferIndex(index + j)] =
                        buffer[toBufferIndex(readIndex + j)];
                }
            }
            // Adjust valid count
            decrementCount(count);
        }

        
        /// Gets the value at the specified index of the Deque        
        /// <param name="index">The index of the Deque.
        public T Get(int index)
        {
            checkIndexOutOfRange(index);
            return buffer[toBufferIndex(index)];
        }

        
        /// Sets the value at the specified index of the
        /// Deque to the given item.      
        /// <param name="index">The index of the deque to set the item.</param>
        /// <param name="item">The item to set at the specified index.</param>
        public void Set(int index, T item)
        {
            checkIndexOutOfRange(index);
            buffer[toBufferIndex(index)] = item;
        }

    } // end class Deque<T>

} // end namespace System.Collections.Generic