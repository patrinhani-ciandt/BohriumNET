using System.Linq.Expressions;

namespace Bohrium.Core.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml.Serialization;

#if !SILVERLIGHT

#endif

    /// <summary>
    /// Class with extension methods for an object
    /// </summary>
    public static class ObjectExtensionMethods
    {
#if !SILVERLIGHT

        /// <summary>
        /// Can convert a serializable object to an simple byte[] or to a compressed one
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compress"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this object value, bool compress = false)
        {
            byte[] ret;
            using (var m = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(m, value);

                ret = m.ToArray();

                if (compress)
                {
                    ret = ret.Compress();
                }
            }

            return ret;
        }

#endif

        /// <summary>
        /// Indicates that the specified reference is a null reference
        /// </summary>
        /// <param name="value">Reference to be tested</param>
        /// <returns>true, if specified value is a null reference, otherwise false</returns>
        public static bool IsNull(this object value)
        {
            return (value == null);
        }

        /// <summary>
        /// Indicates that the specified reference is not a null reference
        /// </summary>
        /// <param name="value">Reference to be tested</param>
        /// <returns>true, if specified value is not a null reference, otherwise false</returns>
        public static bool IsNotNull(this object value)
        {
            return !value.IsNull();
        }

        /// <summary>
        /// Cast specified object to another object
        /// </summary>
        /// <typeparam name="T">Type of the casting result</typeparam>
        /// <param name="value">Value to be casted</param>
        /// <returns>Casting result</returns>
        /// <exception cref="System.InvalidCastException">The exception that is thrown if value could not be casted to specified type</exception>
        public static T UnsafeCast<T>(this object value)
        {
            return value.IsNull() ? default(T) : (T)value;
        }

        /// <summary>
        /// Cast specified object to another object
        /// </summary>
        /// <typeparam name="T">Type of the casting result</typeparam>
        /// <param name="value">Value to be castd</param>
        /// <returns>Casting result or null if it it impossible</returns>
        public static T SafeCast<T>(this object value)
        {
            return value is T ? value.UnsafeCast<T>() : default(T);
        }

        /// <summary>
        /// Executa um cast forçando conversão de tipo caso necessário.
        /// </summary>
        /// <typeparam name="T">Tipo de destino</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CastTo<T>(this object obj)
        {
            if (obj != null)
            {
                if (typeof(T) != obj.GetType())
                {
#if !SILVERLIGHT
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertTo(obj, typeof(T));
#else
                    return (T)obj;
#endif
                }

                return (T)obj;
            }

            return default(T);
        }

        /// <summary>
        /// Checks if an object is compatible with a given type.
        /// </summary>
        /// <typeparam name="T">Type for compatibility checking</typeparam>
        /// <param name="value">Value for testing</param>
        /// <returns>true, if specified object can be converted to the specified type</returns>
        public static bool InstanceOf<T>(this object value)
        {
            return value is T;
        }

        /// <summary>
        /// Takes an object and turns it into a dictionary. Each public property is
        /// added to the dictionary, with the name of the property being the dictionary key,
        /// and its value being the dictionary value.
        /// </summary>
        /// <remarks>
        /// Particularly useful for dealing with anonymous type decleration passed as objects
        /// to a method.
        /// </remarks>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var properties = obj.GetType().GetProperties();
            var hash = new Dictionary<string, object>(properties.Count());
            foreach (PropertyInfo descriptor in properties)
            {
                hash.Add(descriptor.Name, descriptor.GetValue(obj, null));
            }
            return hash;
        }

        /// <summary>
        /// Obtém o enumerador correspondente ao tipo informado.
        /// </summary>
        /// <typeparam name="T">Tipo do Enumerador de Retorno.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this object value)
        {
            return Enum.ToObject(typeof(T), value).SafeCast<T>();
        }

        /// <summary>
        /// Converte um valor para Guid se possível
        /// </summary>
        /// <param name="value">Valor a ser convertido</param>
        /// <returns>Guid convertido</returns>
        public static Guid ToGuid(this object value)
        {
            try
            {
                if (value == null)
                    return Guid.Empty;
                if (value == DBNull.Value)
                    return Guid.Empty;
                if (value is string)
                    return new Guid(value.ToString());
                if (value is Guid)
                    return (Guid)value;

                return new Guid(value.ToString());
            }
            catch
            {
                return Guid.Empty;
            }
        }

#if !SILVERLIGHT

        /// <summary>
        /// Computes the MD5 hash data for an object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>MD5 hash data</returns>
        public static byte[] ComputeMD5Hash(this object obj)
        {
            byte[] objBytes = obj.ToByteArray();

            return System.Security.Cryptography.MD5.Create().ComputeHash(objBytes);
        }

        /// <summary>
        /// Computes the MD5 hash data for an object and return the string representation into hexadecimal string characteres.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToMD5HashString(this object obj)
        {
            var objHash = obj.ComputeMD5Hash();

            return objHash.ToHex();
        }

#endif

        #region ToStringDescriptionDetails

        /// <summary>
        /// Retorno os detalhes do objeto em formato de
        /// string para gerações de Log, Debug, etc...
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToStringDescriptionDetails(this object obj)
        {
            return obj.ToStringDescriptionDetails(10);
        }

        /// <summary>
        /// Retorno os detalhes do objeto em formato de
        /// string para gerações de Log, Debug, etc...
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="maxLevels">Número máximo de níveis de encadeamento de objetos.</param>
        /// <returns></returns>
        public static String ToStringDescriptionDetails(this object obj, int maxLevels)
        {
            string strReturn = toStringDescriptionDetailsResursiveOperation(obj, 0, maxLevels);

            return strReturn;
        }

        private static String toStringDescriptionDetailsResursiveOperation(object obj, int level, int maxLevels)
        {
            StringBuilder strBuilder = new StringBuilder();

            Type typeObj = obj.GetType();

            if (level <= 0)
            {
                strBuilder.AppendLine("===============================");
                strBuilder.AppendLine("BEGIN : " + typeObj.FullName);
                strBuilder.AppendLine("===============================");
            }

            if (level < maxLevels)
            {
                if (obj is IList)
                {
                    IList myIList = new List<object>();

                    if (typeObj.IsGenericType)
                    {
                        myIList = (IList)obj;
                    }

                    for (int i = 0; i < myIList.Count; i++)
                    {
                        strBuilder.AppendLine("-------------------------------");
                        strBuilder.AppendLine("IList [" + i.ToString() + "] : " + myIList[0].GetType().FullName);

                        strBuilder.Append(toStringDescriptionDetailsResursiveOperation(myIList[i], level + 1, maxLevels));
                    }
                }
                else
                    if (typeObj.IsPrimitive)
                    {
                        strBuilder.AppendLine("= " + obj.ToString());
                    }
                    else
                    {
                        PropertyInfo[] properties = typeObj.GetProperties();

                        properties = properties.OrderBy(a => a.Name).ToArray();

                        IList listArrayObjectFromProp = new List<object>();
                        IList listIListObjectFromProp = new List<object>();

                        foreach (PropertyInfo itemProp in properties)
                        {
                            string showValue = "<NULL>";

                            object objPropValue;

                            ParameterInfo[] indexParam = itemProp.GetIndexParameters();

                            if ((indexParam.Length > 0) &&
                                (itemProp.PropertyType == typeof(Char)))
                            {
                                objPropValue = obj.ToString();
                            }
                            else
                            {
                                objPropValue = itemProp.GetValue(obj, null);
                            }

                            if (objPropValue.IsNotNull())
                            {
                                showValue = objPropValue.ToString();
                            }

                            if ((objPropValue.IsNotNull()) &&
                                (objPropValue.GetType().IsArray))
                            {
                                foreach (object childItem in ((object[])objPropValue))
                                    listArrayObjectFromProp.Add(childItem);
                            }
                            else
                                if ((objPropValue.IsNotNull()) &&
                                (objPropValue is IList))
                                {
                                    listIListObjectFromProp = (IList)objPropValue;
                                }
                                else
                                {
                                    strBuilder.AppendLine(getLevelIndentifierString(level) + itemProp.Name + " = " + showValue);
                                }
                        }

                        #region Array Properties

                        for (int i = 0; i < listArrayObjectFromProp.Count; i++)
                        {
                            strBuilder.AppendLine("-------------------------------");
                            strBuilder.AppendLine("Array [" + i.ToString() + "] : " + listArrayObjectFromProp[i].GetType().FullName);

                            strBuilder.Append(toStringDescriptionDetailsResursiveOperation(listArrayObjectFromProp[i], level + 1, maxLevels));
                        }

                        #endregion Array Properties

                        #region IList Properties

                        for (int i = 0; i < listIListObjectFromProp.Count; i++)
                        {
                            strBuilder.AppendLine("-------------------------------");
                            strBuilder.AppendLine("IList [" + i.ToString() + "] : " + listIListObjectFromProp[i].GetType().FullName);

                            strBuilder.Append(toStringDescriptionDetailsResursiveOperation(listIListObjectFromProp[i], level + 1, maxLevels));
                        }

                        #endregion IList Properties
                    }
            }

            if (level <= 0)
            {
                strBuilder.AppendLine("===============================");
                strBuilder.AppendLine("END : " + typeObj.FullName);
                strBuilder.AppendLine("===============================");
            }

            return strBuilder.ToString();
        }

        private static String getLevelIndentifierString(int level)
        {
            String strReturn = "";

            if (level > 0)
            {
                for (int i = 0; i < level; i++)
                {
                    strReturn += "--";
                }

                strReturn += "> ";
            }

            return strReturn;
        }

        #endregion ToStringDescriptionDetails

        /// <summary>
        /// Copies the readable and writable public property values from the source object to the target
        /// </summary>
        /// <remarks>The source and target objects must be of the same type.</remarks>
        /// <param name="target">The target object</param>
        /// <param name="source">The source object</param>
        public static void CopyPropertiesFrom(this object target, object source)
        {
            CopyPropertiesFrom(target, source, string.Empty);
        }

        /// <summary>
        /// Copies the readable and writable public property values from the source object to the target
        /// </summary>
        /// <remarks>The source and target objects must be of the same type.</remarks>
        /// <param name="target">The target object</param>
        /// <param name="source">The source object</param>
        /// <param name="ignoreProperties">An array of property names to ignore</param>
        public static void CopyPropertiesFrom(this object target, object source, params string[] ignoreProperties)
        {
            // Get and check the object types
            var type = source.GetType();
            if (target.GetType() != type)
            {
                throw new ArgumentException("The source type must be the same as the target");
            }

            // Build a clean list of property names to ignore
            var ignoreList = new List<string>();

            if (ignoreProperties.IsNotNull())
            {
                foreach (string item in ignoreProperties)
                {
                    if (!string.IsNullOrEmpty(item) && !ignoreList.Contains(item))
                    {
                        ignoreList.Add(item);
                    }
                }
            }

            // Copy the properties
            foreach (var property in type.GetProperties())
            {
                if (property.CanWrite
                    && property.CanRead
                    && !ignoreList.Contains(property.Name))
                {
                    var val = property.GetValue(source, null);

                    property.SetValue(target, val, null);
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the objects property values
        /// </summary>
        /// <param name="source">The object for the string representation</param>
        /// <returns>A string</returns>
        public static string ToPropertiesString(this object source)
        {
            return ToPropertiesString(source, Environment.NewLine);
        }

        /// <summary>
        /// Returns a string representation of the objects property values
        /// </summary>
        /// <param name="source">The object for the string representation</param>
        /// <param name="delimiter">The line terminstor string to use between properties</param>
        /// <returns>A string</returns>
        public static string ToPropertiesString(this object source, string delimiter)
        {
            if (source == null)
            {
                return string.Empty;
            }

            Type type = source.GetType();

            StringBuilder sb = new StringBuilder(type.Name);
            sb.Append(delimiter);

            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.CanWrite
                    && property.CanRead)
                {
                    object val = property.GetValue(source, null);
                    sb.Append(property.Name);
                    sb.Append(": ");
                    sb.Append(val == null ? "[NULL]" : val.ToString());
                    sb.Append(delimiter);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates a deep clone of the specified object using XML serialization
        /// </summary>
        /// <remarks>
        /// The object to be cloned should be decorated with the
        /// <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
        /// </remarks>
        /// <param name="source">The object to deep clone</param>
        /// <returns>A copy of the source object</returns>
        public static object DeepClone(this object source)
        {
            if (source == null)
            {
                throw new ArgumentException("The source object cannot be null.");
            }

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(stream, source);
                stream.Position = 0;
                return serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Serializes the object into an XML string, using the encoding method specified in
        /// <see>
        ///     <cref>ExtensionMethodsSettings.DefaultEncoding</cref>
        /// </see>
        /// </summary>
        /// <remarks>
        /// The object to be serialized should be decorated with the
        /// <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
        /// </remarks>
        /// <param name="source">The object to serialize</param>
        /// <returns>An XML encoded string representation of the source object</returns>
        public static string ToXml(this object source)
        {
            return ToXml(source, Encoding.UTF8);
        }

        /// <summary>
        /// Serializes the object into an XML string
        /// </summary>
        /// <remarks>
        /// The object to be serialized should be decorated with the
        /// <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
        /// </remarks>
        /// <param name="source">The object to serialize</param>
        /// <param name="encoding">The Encoding scheme to use when serializing the data to XML</param>
        /// <returns>An XML encoded string representation of the source object</returns>
        public static string ToXml(this object source, Encoding encoding)
        {
            if (source == null)
            {
                throw new ArgumentException("The source object cannot be null.");
            }

            if (encoding == null)
            {
                throw new Exception("You must specify an encoder to use for serialization.");
            }

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(stream, source);
                stream.Position = 0;
#if !SILVERLIGHT
                return encoding.GetString(stream.ToArray());
#else
                return encoding.GetString(stream.ToArray(), 0, stream.ToArray().Length);
#endif
            }
        }

        /// <summary>
        /// Returns the supplied alternative value
        /// if the source is DBNull.Value.
        /// </summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <param name="value">Source value.</param>
        /// <param name="alternative">Alternate value.</param>
        /// <returns>Source value or alternate value if source
        /// is DBNull.Value.</returns>
        public static T CoalesceDBNull<T>(
            this object value,
            T alternative)
        {
            return (value is DBNull ? alternative : (T)value);
        }

        /// <summary>
        /// Returns default(T) if the source is
        /// DBNull.Value.
        /// </summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <param name="value">Source value.</param>
        /// <returns>Source value or default(T) if source
        /// is DBNull.Value.</returns>
        public static T CoalesceDBNull<T>(
            this object value)
        {
            return value.CoalesceDBNull(default(T));
        }

        /// <summary>
        /// Return as string the member name passed as a Lambda Expression.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldExpression"></param>
        /// <returns></returns>
        public static string GetMemberName<TSource>(this TSource obj, Expression<Func<TSource, object>> fieldExpression)
        {
            if (object.Equals(fieldExpression, null))
            {
                throw new NullReferenceException("Field is required");
            }

            MemberExpression expr = null;

            var body = fieldExpression.Body as MemberExpression;

            if (body != null)
            {
                expr = body;
            }
            else if (fieldExpression.Body is UnaryExpression)
            {
                var unaryBody = fieldExpression.Body as UnaryExpression;

                expr = unaryBody.Operand as MemberExpression;
            }
            else
            {
                const string Format = "Expression '{0}' not supported.";
                string message = string.Format(Format, fieldExpression);

                throw new ArgumentException(message, "Field");
            }

            return expr.Member.Name;
        }

        /// <summary>
        /// This method calls the MemberwiseClone method to perform a shallow copy operation creating a new object,
        /// and then copying the nonstatic fields of the current object to the new object.
        /// If a field is a value type, a bit-by-bit copy of the field is performed.
        /// If a field is a reference type, the reference is copied but the referred object is not;
        /// therefore, the original object and its clone refer to the same object.
        /// </summary>
        /// <param name="source">Object to be copied.</param>
        /// <returns></returns>
        public static object ShallowCopy(this object source)
        {
            if (source == null)
            {
                throw new ArgumentException("The source object cannot be null.");
            }

            return source.CallMethod("MemberwiseClone");
        }
    }
}