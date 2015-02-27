using System.Linq;
using System.Reflection;

namespace Bohrium.Core.Extensions
{
    /// <summary>
    /// Class with extension methods to perform reflection operations.
    /// </summary>
    public static class ReflectionExtensionMethods
    {
        /// <summary>
        /// Search a specific property with specific binding into the object type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(this object obj, string propertyName,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return obj.GetType().GetProperty(propertyName, bindingFlags);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static object GetStaticPropertyValue(this object obj, string propertyName,
            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return obj.GetPropertyValue(propertyName, bindingFlags);
        }

        /// <summary>
        /// Get value from a property by reflection.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string propertyName,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            object objValue = null;

            var propertyInfo = obj.GetPropertyInfo(propertyName, bindingFlags);

            if (propertyInfo != null)
            {
                objValue = propertyInfo.GetValue(obj);
            }

            return objValue;
        }

        /// <summary>
        /// Set value to a property by reflection.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="propValue"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static void SetPropertyValue(this object obj, string propertyName, object propValue,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            var propertyInfo = obj.GetPropertyInfo(propertyName, bindingFlags);

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(obj, propValue);
            }
        }

        /// <summary>
        /// Search a specific field with specific binding into the object type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static FieldInfo GetFieldInfo(this object obj, string fieldName,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return obj.GetType().GetField(fieldName, bindingFlags);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static object GetStaticFieldValue(this object obj, string fieldName,
            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return obj.GetFieldValue(fieldName, bindingFlags);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static object GetFieldValue(this object obj, string fieldName,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            object objValue = null;

            var fieldInfo = obj.GetFieldInfo(fieldName, bindingFlags);

            if (fieldInfo != null)
            {
                objValue = fieldInfo.GetValue(obj);
            }

            return objValue;
        }

        /// <summary>
        /// Set value to a field by reflection.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static void SetFieldValue(this object obj, string fieldName, object fieldValue,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            var fieldInfo = obj.GetFieldInfo(fieldName, bindingFlags);

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(obj, fieldValue);
            }
        }

        /// <summary>
        /// Search a specific method with specific method binding into the object type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(this object obj, string methodName,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return obj.GetType().GetMethod(methodName, bindingFlags);
        }

        /// <summary>
        /// Search all parameters for a specific method with specific method binding into the object type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static ParameterInfo[] GetMethodParameters(this object obj, string methodName,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        {
            var methodInfo = obj.GetMethodInfo(methodName, bindingFlags);

            if (methodInfo != null)
            {
                var parameterInfos = methodInfo.GetParameters();

                if (parameterInfos.Any())
                {
                    return parameterInfos;
                }
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public static object CallStaticMethod(this object obj, string methodName,
            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
            params object[] objParams)
        {
            return obj.CallMethod(methodName, bindingFlags);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public static object CallMethod(this object obj, string methodName,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            params object[] objParams)
        {
            object objValue = null;

            var methodInfo = obj.GetMethodInfo(methodName, bindingFlags);

            if (methodInfo != null)
            {
                objValue = methodInfo.Invoke(obj, objParams);
            }

            return objValue;
        }
    }
}