using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueSteel.Host.Data
{
    /// <summary>
    /// Defines the value repository.
    /// </summary>
    public interface IValueRepository
    {
        /// <summary>
        /// Get a collecton of the values.
        /// </summary>
        IEnumerable<string> Values { get; }

        /// <summary>
        /// Check if a value of the specified id is contained within.
        /// </summary>
        /// <param name="id">The id to test for.</param>
        /// <returns>Returns true if the value is within the set, false otherwise.</returns>
        bool Contains(int id);

        /// <summary>
        /// Get or set the item by index.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <returns>Returns the requested value.</returns>
        string this[int index]
        {
            get;
            set;
        }

        /// <summary>
        /// Remove the item specified.
        /// </summary>
        /// <param name="index">Teh index to remove.</param>
        void Remove(int index);

        /// <summary>
        /// Add an item to the repository.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>Returns the index of the new value.</returns>
        int Add(string value);
    }
}
