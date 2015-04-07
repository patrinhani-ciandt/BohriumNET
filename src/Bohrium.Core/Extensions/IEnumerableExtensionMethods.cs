namespace Bohrium.Core.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

#if !SILVERLIGHT

#endif

    public static class IEnumerableExtensionMethods
    {
#if !SILVERLIGHT

        public static DataTable ToDataTable<T>(this IEnumerable<T> enumerable)
        {
            return ToDataTable(enumerable, null);
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> enumerable, Func<IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> filterPropertiesDelegate)
        {
            var table = new DataTable();

            IEnumerable<PropertyInfo> props = typeof(T).GetProperties();

            if (filterPropertiesDelegate != null)
                props = filterPropertiesDelegate(props);

            foreach (PropertyInfo prop in props)
                table.Columns.Add(prop.Name, prop.PropertyType);

            foreach (T t in enumerable)
            {
                var row = table.NewRow();

                foreach (PropertyInfo prop in props)
                    row[prop.Name] = prop.GetValue(t, null);

                table.Rows.Add(row);
            }

            return table;
        }

#endif

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> equalityComparer)
        {
            var sourceCount = source.Count();

            for (int i = 0; i < sourceCount; i++)
            {
                bool found = false;

                for (int j = 0; j < i; j++)
                    if (equalityComparer(source.ElementAt(i), source.ElementAt(j))) // In some cases, it's better to convert source in List<T> (before first for)
                    {
                        found = true;
                        break;
                    }

                if (!found)
                    yield return source.ElementAt(i);
            }
        }

        /// <summary>
        /// Performs the specified action on each element of the list
        /// </summary>
        public static void Each<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }

        /// <summary>
        /// Performs the specified action on each element of the list and includes
        /// an index value (starting at 0)
        /// </summary>
        public static void EachIndex<T>(this IEnumerable<T> list, Action<T, int> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            var enumerator = list.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext())
            {
                action(enumerator.Current, count++);
            }
        }

        /// <summary>
        /// Validates that the predicate is true for each element of the list
        /// </summary>
        public static bool TrueForAll<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!predicate(enumerator.Current))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Retuns a list of all items matching the predicate
        /// </summary>
        public static List<T> FindAll<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            var found = new List<T>();
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    found.Add(enumerator.Current);
                }
            }
            return found;
        }

        /// <summary>
        /// Retuns the first matching item
        /// </summary>
        public static T Find<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    return enumerator.Current;
                }
            }
            return default(T);
        }

        /// <summary>
        /// Finds the index of an item
        /// </summary>
        public static int Index<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            var enumerator = list.GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); ++i)
            {
                if (predicate(enumerator.Current))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Determines whether or not the item exists
        /// </summary>
        public static bool Exists<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            return list.Index(predicate) > -1;
        }

        /// <summary>
        /// new
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this IEnumerable list)
        {
            return (list == null) || (list.GetEnumerator().MoveNext() == false);
        }

        /// <summary>
        /// new
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsEmpty(this IEnumerable list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return list.GetEnumerator().MoveNext() == false;
        }
    }
}