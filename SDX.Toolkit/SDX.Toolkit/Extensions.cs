using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace SDX.Toolkit
{
    public static class Extensions
    {
        /// <summary>
        /// Removes all instances of a type of object from the children collection.
        /// </summary>
        /// <typeparam name="T">The type of object you want to remove.</typeparam>
        /// <param name="targetCollection">A reference to the canvas you want items removed from.</param>
        public static void Remove<T>(this UIElementCollection targetCollection)
        {
            // This will loop to the end of the children collection.
            int index = 0;

            // Loop over every element in the children collection.
            while (index < targetCollection.Count)
            {
                // Remove the item if it's of type T
                if (targetCollection[index] is T)
                    targetCollection.RemoveAt(index);
                else
                    index++;
            }
        }
    }
}
