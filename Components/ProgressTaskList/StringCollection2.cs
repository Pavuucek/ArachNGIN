using System.Collections;

namespace ArachNGIN.Components.ProgressTaskList
{
    /// <summary>
    ///     A helper collection class to manage items being inserted/removed to the TaskItems property.
    ///     Supports modifying the collection through VS.
    /// </summary>
    public class StringCollection2 : CollectionBase
    {
        private readonly ProgressTaskList _parent;

        /// <summary>
        ///     Constructor requires the  string that owns this collection
        /// </summary>
        /// <param name="parent">ProgressProgressTaskList</param>
        public StringCollection2(ProgressTaskList parent)
        {
            _parent = parent;
        }

        /// <summary>
        ///     Finds the string in the collection
        /// </summary>
        public string this[int index]
        {
            get { return ((string) List[index]); }
            set { List[index] = value; }
        }

        /// <summary>
        ///     Returns the ProgressProgressTaskList that owns this collection
        /// </summary>
        public ProgressTaskList Parent
        {
            get { return _parent; }
        }


        /// <summary>
        ///     Adds a string into the Collection
        /// </summary>
        /// <param name="value">The string to add</param>
        /// <returns></returns>
        public int Add(string value)
        {
            int result = List.Add(value);
            return result;
        }


        /// <summary>
        ///     Adds an array of strings into the collection. Used by the Studio Designer generated code
        /// </summary>
        /// <param name="strings">Array of strings to add</param>
        public void AddRange(string[] strings)
        {
            // Use external to validate and add each entry
            foreach (string s in strings)
            {
                Add(s);
            }
        }

        /// <summary>
        ///     Finds the position of the string in the colleciton
        /// </summary>
        /// <param name="value">string to find position of</param>
        /// <returns>Index of string in collection</returns>
        public int IndexOf(string value)
        {
            return (List.IndexOf(value));
        }

        /// <summary>
        ///     Adds a new string at a particular position in the Collection
        /// </summary>
        /// <param name="index">Position</param>
        /// <param name="value">string to be added</param>
        public void Insert(int index, string value)
        {
            List.Insert(index, value);
        }


        /// <summary>
        ///     Removes the given string from the collection
        /// </summary>
        /// <param name="value">string to remove</param>
        public void Remove(string value)
        {
            //Remove the item
            List.Remove(value);
        }

        /// <summary>
        ///     Detects if a given string is in the Collection
        /// </summary>
        /// <param name="value">string to find</param>
        /// <returns></returns>
        public bool Contains(string value)
        {
            // If value is not of type Int16, this will return false.
            return (List.Contains(value));
        }


        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            _parent.InitLabels();
        }

        /// <summary>
        ///     Propogates when external designers remove items from string
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            _parent.InitLabels();
        }
    }
}