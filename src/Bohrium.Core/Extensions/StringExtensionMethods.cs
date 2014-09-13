namespace Bohrium.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

#if !SILVERLIGHT

#else
    using System.Windows.Media;
#endif

    public static class StringExtensionMethods
    {
        public static bool IsNullOrTrimEmpty(this string strValue)
        {
            return String.IsNullOrWhiteSpace(strValue);
        }

        public static string GetNumbers(this string strValue)
        {
            if (strValue.IsNullOrTrimEmpty())
            {
                return string.Empty;
            }

            var strBuilder = new StringBuilder();

            foreach (char ch in strValue)
            {
                if (char.IsNumber(ch))
                {
                    strBuilder.Append(ch.ToString());
                }
            }

            return strBuilder.ToString();
        }

        public static string RemoveAccents(this string strValue)
        {
#if !SILVERLIGHT
            string normalized = strValue.Normalize(NormalizationForm.FormKD);
#else
            string normalized = strValue;
#endif

            var strBuilder = new StringBuilder();

            foreach (var c in normalized)
            {
                if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    strBuilder.Append(c);
                }
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Returns an empty string if the current string object is null.
        /// </summary>
        public static string DefaultIfNull(this string s)
        {
            return s ?? string.Empty;
        }

        /// <summary>
        /// Aplica Uppercase na primeira letra da string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Capitalize(this String str)
        {
            var strReturn = str;

            if (!strReturn.IsNullOrTrimEmpty())
            {
                if (str.Trim().Length > 1)
                {
                    strReturn = str.Substring(0, 1).ToUpper() + str.Substring(1);
                }
                else
                {
                    strReturn = str.ToUpper();
                }
            }

            return strReturn;
        }

        /// <summary>
        /// Replaces a specified string from the current string by regular expression.
        /// </summary>
        /// <param name="value">Value to modify</param>
        /// <param name="regularExpr">regular expression, which will be search from value</param>
        /// <param name="replaceValue">Replace string value</param>
        /// <returns>Modified string</returns>
        public static String RegexReplace(this String value, string regularExpr, string replaceValue)
        {
            return Regex.Replace(value, regularExpr, replaceValue);
        }

        /// <summary>
        /// Delete a specified string from the current string by regular expression.
        /// </summary>
        /// <param name="value">Value to modify</param>
        /// <param name="regularExpr">regular expression, which will be deleted from value</param>
        /// <returns>Modified string</returns>
        public static String RegexDelete(this String value, string regularExpr)
        {
            return value.RegexReplace(regularExpr, String.Empty);
        }

        /// <summary>
        /// Delete a specified string from the current string
        /// </summary>
        /// <param name="value">Value to modify</param>
        /// <param name="strToDelete">Substring, which is deleted from value</param>
        /// <returns>Modified string</returns>
        public static String Delete(this String value, String strToDelete)
        {
            return value.IsNullOrTrimEmpty() ? null : value.Replace(strToDelete, "");
        }

        /// <summary>
        /// Returns a string array containing the substrings from the current string 
        /// object that are delimited by the given separator.
        /// </summary>
        public static string[] Split(this string s, string separator)
        {
            return s.Split(separator.ToCharArray());
        }

#if !SILVERLIGHT

        /// <summary>
        /// Returns a Dictionary instance created from the current string 
        /// object if it contains a format like "firstkey=value1|second=Val2|...".
        /// </summary>
        public static Dictionary<string, string> ToDictionary(this string s)
        {
            return ToDictionary(s, "|");
        }

        /// <summary>
        /// Returns a Dictionary instance created from the current string 
        /// object if it contains a format like 
        /// "firstkey=value1[separator]second=Val2[separator]...".
        /// </summary>
        public static Dictionary<string, string> ToDictionary(this string s, string separator)
        {
            var dic = new Dictionary<string, string>();

            var collection = ToNameValueCollection(s, separator);
            
            foreach (var key in collection.AllKeys)
                dic.Add(key, collection[key]);

            return dic;
        }

        /// <summary>
        /// Returns a NameValueCollection instance created from the current string 
        /// object if it contains a format like "firstkey=value1|second=Val2|...".
        /// </summary>
        public static NameValueCollection ToNameValueCollection(this string s)
        {
            return ToNameValueCollection(s, "|");
        }

        /// <summary>
        /// Returns a NameValueCollection instance created from the current string 
        /// object if it contains a format like 
        /// "firstkey=value1[separator]second=Val2[separator]...".
        /// </summary>
        public static NameValueCollection ToNameValueCollection(this string s, string separator)
        {
            if (string.IsNullOrEmpty(separator))
                throw new ArgumentNullException("separator");

            var collection = new NameValueCollection();

            var nameValuePairs = s.Split(separator.ToCharArray());

            foreach (var nvs in nameValuePairs)
            {
                var nvp = nvs.Split("=".ToCharArray());

                var name = nvp[0].Trim();
                var value = nvp.Length > 1 ? nvp[1].Trim() : string.Empty;

                if (name.Length > 0)
                    collection.Add(name, value);
            }

            return collection;
        }
#else
        /// <summary>
        /// Returns a Dictionary instance created from the current string 
        /// object if it contains a format like "firstkey=value1|second=Val2|...".
        /// </summary>
        public static Dictionary<string, string> ToDictionary(this string s)
        {
            return ToDictionary(s, "|");
        }

        /// <summary>
        /// Returns a Dictionary instance created from the current string 
        /// object if it contains a format like 
        /// "firstkey=value1[separator]second=Val2[separator]...".
        /// </summary>
        public static Dictionary<string, string> ToDictionary(this string s, string separator)
        {
            if (string.IsNullOrEmpty(separator))
                throw new ArgumentNullException("separator");

            Dictionary<string, string> dic = new Dictionary<string, string>();

            string[] nameValuePairs = s.Split(separator.ToCharArray());

            foreach (string nvs in nameValuePairs)
            {
                string[] nvp = nvs.Split("=".ToCharArray());

                string name = nvp[0].Trim();
                string value = nvp.Length > 1 ? nvp[1].Trim() : string.Empty;

                if (name.Length > 0)
                    dic.Add(name, value);
            }

            return dic;
        }
#endif

        /// <summary>
        /// Returns a List(string) instance from the current |-separated string.
        /// </summary>
        public static List<string> ToList(this string s)
        {
            return ToList(s, "|");
        }

        /// <summary>
        /// Returns a List(string) instance from the current string.
        /// </summary>
        public static List<string> ToList(this string s, string separator)
        {
            var list = new List<string>();

            foreach (var e in s.Split(separator.ToCharArray()))
                list.Add(e.Trim());

            return list;
        }

#if !SILVERLIGHT
        /// <summary>
        /// Returns a MD5 representation of the current string object.
        /// </summary>
        public static string ToMD5(this string s)
        {
            var bytes = (new MD5CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(s));

            var returnValue = bytes.ToHex().ToLower();

            return returnValue;
        }
#endif

        /// <summary>
        /// Obtém o enumerador correspondente ao tipo informado.
        /// </summary>
        /// <typeparam name="T">Tipo do Enumerador de Retorno.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseToEnum<T>(this string value)
        {
            return Enum.Parse(typeof(T), value, false).SafeCast<T>();
        }

        /// <summary>
        /// Pesquisa um padrão (compatível com o comando "LIKE" do SQL ANSI) não "CaseSensitive" na string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="likePattern">Padrão (compatível com o comando "LIKE" do SQL ANSI)</param>
        /// <returns>"true" casa o padrão requisitado seja encontrado, caso contrário retorna "false"</returns>
        public static bool ContainsLike(this string str, string likePattern)
        {
            likePattern = Regex.Escape(likePattern);

            string strSearchPatt = String.Format(@"^({0})$",
                likePattern.Replace("%", @"(.*)"));

            bool returnValue = Regex.IsMatch(str, strSearchPatt, (RegexOptions.IgnoreCase));

            return returnValue;
        }

        /// <summary>
        /// Removes all HTML markup tags from the supplied string.
        /// </summary>
        /// <param name="str">The text to strip</param>
        /// <returns>The source sring stripped of all HTML markup</returns>
        public static string StripHtml(this string str)
        {
            string stripped = Regex.Replace(str, @"<(.|\n)*?>", string.Empty);

            // TODO: I'm sure this can be improved further..
            /*
             & (ampersand character) 
             &amp; 
             Must be used both for attribute values and for content of an element. 
             
            > (greater-than character) 
             &gt; 
             Must be used for attribute value, but > is acceptable as the content of an element so long as < does not precede it. 
             
            < (less-than character) 
             &lt; 
             Must be used for attribute value, but < is acceptable as the content of an element so long as > does not follow it. 
             
            " (double quote character) 
             &quot; 
             Must be used for attribute value, but " is acceptable as the content of an element. Note that attribute values themselves may be enclosed either by ' or "; whichever character appears first will define the attribute value enclosure, and the alternative quote can then be used as a literal within the value. 
             
            ' (single quote character) 
             &apos; 
             Must be used for attribute value, but ' is acceptable as the content of an element. Note that attribute values themselves may be enclosed either by ' or "; whichever character appears first will define the attribute value enclosure, and the alternative quote can then be used as a literal within the value. 
             
            (numeric character mappings) 
             &#[integer]; or &#x[hex]; 
             XAML supports numeric character mappings into the encoding that is active. 
             
            (nonbreaking space) 
             &#160; (assuming UTF-8 encoding) 
             For flow document elements, or elements that take text such as TextBox, nonbreaking spaces are not normalized out of the markup, even for xml
             */

            stripped = stripped.Replace("&nbsp;", " ");
            stripped = stripped.Replace("&#160;", " ");
            stripped = stripped.Replace("&gt;", ">");
            stripped = stripped.Replace("&lt;", "<");
            stripped = stripped.Replace("&quot;", "\"");
            stripped = stripped.Replace("&amp;", "&");
            stripped = stripped.Replace("&apos;", "'");

            return stripped;
        }

        /// <summary>
        /// Splits the supplied string into an array of lines using the system default
        /// line terminator.
        /// </summary>
        /// <param name="str">The string to split</param>
        /// <param name="trimLines">If true, leading and trailing whitespace will be removed from each line.</param>
        /// <returns>A string list representing each line of text</returns>
        public static List<string> ToLines(this string str, bool trimLines)
        {
            List<string> formatted = new List<string>();
            string[] crlfs = { "\r\n", "\n\r", "\n", "\r", "<br>", "<br />", "<BR>", "<BR />" };
            string[] lines = str.Split(crlfs, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                if (trimLines)
                {
                    formatted.Add(lines[i].Trim());
                }
                else
                {
                    formatted.Add(lines[i]);
                }
            }

            return formatted;
        }

        /// <summary>
        /// Deserializes an XML string into an object instance, using the encoding method specified in
        /// <see>
        ///     <cref>ExtensionMethodsSettings.DefaultEncoding</cref>
        /// </see>
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="str">The XML string representantion of the object</param>
        /// <returns>An instance of T</returns>
        public static T ToObjectFromXml<T>(this string str)
        {
            return ToObjectFromXml<T>(str, Encoding.UTF8);
        }

        /// <summary>
        /// Deserializes an XML string into an object instance
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="str">The XML string representantion of the object</param>
        /// <param name="encoding">The encoding scheme to use when deserializing the object</param>
        /// <returns>An instance of T</returns>
        public static T ToObjectFromXml<T>(this string str, Encoding encoding)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("The source string cannot be null or empty");
            }

            if (encoding == null)
            {
                throw new ArgumentException("An encoding scheme must be specified");
            }

            using (var stream = str.ToStream(encoding))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Coneverts a string into a <see cref="Stream"/>, using the encoding method specified in
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>A stream representation of the source string</returns>
        public static Stream ToStream(this string str)
        {
            return ToStream(str, Encoding.UTF8);
        }

        /// <summary>
        /// Coneverts a string into a <see cref="Stream"/>
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <param name="encoding">The encoding scheme to use for the conversion</param>
        /// <returns>A stream representation of the source string</returns>
        public static Stream ToStream(this string str, Encoding encoding)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("The source string cannot be null or empty");
            }
            if (encoding == null)
            {
                throw new ArgumentException("An encoding scheme must be specified");
            }

            return new MemoryStream(str.ToByteArray(encoding));
        }

        /// <summary>
        /// Converts a string into a byte array, using the encoding method specified in
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>A byte array representation of the source string</returns>
        public static byte[] ToByteArray(this string str)
        {
            return ToByteArray(str, Encoding.UTF8);
        }

        /// <summary>
        /// Converts a string into a byte array
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <param name="encoding">The encoding scheme to use for the conversion</param>
        /// <returns>A byte array representation of the source string</returns>
        public static byte[] ToByteArray(this string str, Encoding encoding)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("The source string cannot be null or empty");
            }
            if (encoding == null)
            {
                throw new ArgumentException("An encoding scheme must be specified");
            }

            return encoding.GetBytes(str);
        }

        /// <summary>
        /// Returns null if the string is either
        /// null or empty.
        /// </summary>
        /// <param name="source">String to test.</param>
        /// <returns>Value of the string, or null.</returns>
        public static string NullIfEmpty(
            this string source)
        {
            return (source.IsNullOrTrimEmpty() ? null : source);
        }
    }
}
