using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    /// <summary>
    /// used to take a string and filter a set of key strings and convert them into something else
    /// </summary>
    public static class TextFilter
    {
        public static List<Filter> Filters { get; private set; } = new List<Filter>();

        /// <summary>
        /// set the text filters
        /// </summary>
        /// <param name="filterList">
        /// string containing all the filters
        /// Use this format "{key1}>{convertTo1}|{key2}>{convertTo2}..."
        /// </param>
        public static void SetFilters(string filterList)
        {
            Filters.Clear();

            List<string> filterSplit = filterList.Split('|').ToList();

            for (int i = 0; i < filterSplit.Count; i++)
            {
                filterSplit[i] = filterSplit[i].Replace("{", "").Replace("}", "");

                List<string> individualFilter = filterSplit[i].Split('>').ToList();

                Filter filter = new Filter()
                {
                    key = individualFilter[0],
                    convertTo = individualFilter[1]
                };

                Filters.Add(filter);
            }
        }

        /// <summary>
        /// filters a string based on the filters defined in Filters
        /// </summary>
        /// <param name="input">
        /// Unfiltered string
        /// </param>
        /// <returns>
        /// string with filters applied
        /// </returns>
        public static string FilterString(string input)
        {
            foreach (Filter filter in Filters)
            {
                input = input.Replace(filter.key, filter.convertTo);
            }

            return input;
        }
    }

    public struct Filter
    {
        public string key;
        public string convertTo;
    }
}
