using System;
using System.Linq;

namespace Bohrium.Core.Extensions
{
    public static class TypeExtensionMethods
    {
        /// <summary>
        /// Return a dafault value for a type, if the type is a class and has a constructor without parameters then will return a new instance of this class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contructIfClass"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type, bool contructIfClass = true)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            
            if ((type.IsClass) && (contructIfClass))
            {
                if (type.GetConstructors().Any(c => !c.GetParameters().Any()))
                {
                    return Activator.CreateInstance(type);
                }
            }

            return null;
        }
    }
}
