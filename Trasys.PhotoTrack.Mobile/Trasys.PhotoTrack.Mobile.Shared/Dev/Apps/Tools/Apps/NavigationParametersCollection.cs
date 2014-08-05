using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trasys.Dev.Tools.Apps
{
    /// <summary>
    /// Class to manage a list of NavigationParameters
    /// </summary>
    public class NavigationParametersCollection : CollectionBase, IEnumerable<KeyValuePair<string, object>>
    {
        #region CONSTRUCTORS

        /// <summary>
        /// Initialize a new instance of NavigationParametersCollection
        /// </summary>
        public NavigationParametersCollection()
        {

        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets or sets the NavigationParameter at the specified index.
        /// </summary>
        /// <param name="index">The zero-based of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public virtual KeyValuePair<string, object> this[int index]
        {
            get
            {
                return ((KeyValuePair<string, object>)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the NavigationParameter at the named position.
        /// </summary>
        /// <param name="name">The name of the element to get or set.</param>        
        /// <returns>The element with the specified name.</returns>
        public virtual KeyValuePair<string, object> this[string name]
        {
            get
            {
                foreach (KeyValuePair<string, object> item in this.List)
                {
                    if (item.Key.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return item;
                }
                return new KeyValuePair<string, object>();
            }
            set
            {
                foreach (KeyValuePair<string, object> item in this.List)
                {
                    if (item.Key.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        this.List[this.List.IndexOf(item)] = value;
                }
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Adds a new NavigationParameter to this collection.
        /// </summary>
        /// <param name="value">The NavigationParameter to add to the collection.</param>
        /// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection.</returns>
        public virtual int Add(KeyValuePair<string, object> value)
        {
            return (List.Add(value));   // List.Add raises OnInsert method
        }

        /// <summary>
        /// Add a new NavigationParameter object with the specified name.
        /// </summary>
        /// <param name="name">Name of the new NavigationParameter</param>
        /// <param name="reference">Reference of the new NavigationParameter</param>
        /// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection.</returns>
        public virtual int Add(string name, string reference)
        {
            return this.Add(new KeyValuePair<string, object>(name, reference));
        }

        /// <summary>
        /// Inserts a new NavigationParameter to the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The NavigationParameter to insert into the collection.</param>
        public virtual void Insert(int index, KeyValuePair<string, object> value)
        {
            this.List.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of NavigationParameter from the collection.
        /// </summary>
        /// <param name="value">The NavigationParameter object to remove from the collection.</param>
        public virtual void Remove(KeyValuePair<string, object> value)
        {
            List.Remove(value);     // List.Remove raises OnRemove method
        }

        /// <summary>
        /// Removes the first occurrence of NavigationParameter from the collection, based on his name.
        /// </summary>
        /// <param name="name">NavigationParameter's name to remove from the collection.</param>
        public virtual void Remove(string name)
        {
            this.Remove(this[name]);
        }

        /// <summary>
        /// Determines the index of a specific NavigationParameter in this collection.
        /// </summary>
        /// <param name="value">The object to locate in the collection.</param>
        /// <returns>The index of value if found in the list; otherwise, -1.</returns>
        public virtual int IndexOf(KeyValuePair<string, object> value)
        {
            return this.List.IndexOf(value);
        }

        /// <summary>
        /// Determines whether this collection contains a specific NavigationParameter.
        /// </summary>
        /// <param name="value">The object to locate in the collection.</param>
        /// <returns>True if the NavigationParameter is found in the collection; otherwise, false.</returns>
        public virtual bool Contains(KeyValuePair<string, object> value)
        {
            return this.List.Contains(value);
        }

        /// <summary>
        /// Copies the elements of this to an Array, starting at a particular Array index. 
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the items copied from the dictionary. The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public virtual void CopyTo(KeyValuePair<string, object>[] array, int index)
        {
            this.List.CopyTo(array, index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A IEnumerator(Of NavigationParameter) that can be used to iterate through the collection.</returns>
        public virtual new IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (KeyValuePair<string, object> item in this.List)
                yield return item;
        }

        #endregion

    }
}
