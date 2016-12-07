using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueSteel.Host.Data
{
    /// <summary>
    /// Defines the memory value repo.
    /// </summary>
    public class MemoryValueRepository : IValueRepository
    {
        /// <summary>
        /// Holds the values.
        /// </summary>
        private Dictionary<int, string> pairs;

        /// <summary>
        /// Create the repository.
        /// </summary>
        public MemoryValueRepository()
        {
            this.pairs = new Dictionary<int, string>();
        }

        /// <summary>
        /// Get or set the value.
        /// </summary>
        /// <param name="index">The items index.</param>
        /// <returns>Returns the requested value.</returns>
        public string this[int index]
        {
            get
            {
                return this.pairs[index];
            }

            set
            {
                this.pairs[index] = value;
            }
        }

        /// <summary>
        /// Get the values.
        /// </summary>
        public IEnumerable<string> Values
        {
            get
            {
                return this.pairs.Values;
            }
        }

        /// <summary>
        /// Add a new item to the repo.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>Returns the index of the new value.</returns>
        public int Add(string value)
        {
            int index = 0;
            if (pairs.Count > 0)
            {
                /// Not the perfect logic here... but whatever, it's good enough for an example.
                index = (from id in pairs.Keys select id).Max() + 1;
            }

            this.Add(index, value);

            return index;
        }

        /// <summary>
        /// Add a new item to the repo.
        /// </summary>
        /// <param name="id">The id to add.</param>
        /// <param name="value">The value to add.</param>
        public void Add(int id, string value)
        {
            this.pairs.Add(id, value);
        }

        /// <summary>
        /// Check if the id is contained within.
        /// </summary>
        /// <param name="id">The value to test for.</param>
        /// <returns>Returns true if the value is contained in the repo, false otherwise.</returns>
        public bool Contains(int id)
        {
            return this.pairs.ContainsKey(id);
        }

        /// <summary>
        /// Remove the value.
        /// </summary>
        /// <param name="index">The index to remove.</param>
        public void Remove(int index)
        {
            this.pairs.Remove(index);
        }
    }
}
