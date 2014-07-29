namespace Bohrium.Core.Extensions
{
    using System.Linq;

    public static class GenericExtensionMethods
    {
        /// <summary>
        /// Check if a value is present into a array.
        /// </summary>
        /// <typeparam name="T">Type of the object which will be compared.</typeparam>
        /// <param name="value">object which will be compared.</param>
        /// <param name="tCompareValues">array which will be the comparable list.</param>
        /// <returns></returns>
        public static bool In<T>(this T value, params T[] tCompareValues)
        {
            var exists = false;

            if (tCompareValues.IsNotNull())
            {
                exists = tCompareValues.Contains(value);
            }

            return exists;
        }

        /// <summary>
        /// Converts the current value into array with single element
        /// </summary>
        /// <typeparam name="T">Type of the array</typeparam>
        /// <param name="value"></param>
        /// <returns>Array with single element</returns>
        public static T[] AsArray<T>(this T value)
        {
            return new[] { value };
        }

        /// <summary>
        /// Returns value indicating that the current value is a default
        /// value for it type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>Value indicating that the current value is a default
        /// value for it type</returns>
        public static bool IsDefault<T>(this T value)
        {
            var defValue = default(T);
            if (value.IsNull())
                return defValue.IsNull();
            return value.Equals(defValue);
        }

        /// <summary>
        /// Retorna o valor passado como parâmetro caso o objeto do tipo informado for 
        /// nulo.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T DefaultIfNull<T>(this T value, T defaultValue)
        {
            T returnValue = value;

            if (value.IsNull())
                returnValue = defaultValue;

            return returnValue;
        }
    }
}
