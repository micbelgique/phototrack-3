using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


public static class ArrayExtensions
{
    /// <summary>
    /// Adds an objects list to the end of the System.Collections.ObjectModel.Collection<T>.
    /// </summary>
    /// <typeparam name="T">Type of collection item</typeparam>
    /// <param name="collection">Initial collection</param>
    /// <param name="values">A list of elements to be added to the end of the System.Collections.ObjectModel.Collection<T>.</param>
    public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> values)
    {
        foreach (T item in values)
        {
            collection.Add(item);
        }
    }

}
