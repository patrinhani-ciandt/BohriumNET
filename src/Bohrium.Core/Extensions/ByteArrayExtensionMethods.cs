using Bohrium.Core.Compression;

namespace Bohrium.Core.Extensions
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

#if !SILVERLIGHT
#endif

    /// <summary>
    /// Class with extension methods for byte[]
    /// </summary>
    public static class ByteArrayExtensionMethods
    {
#if !SILVERLIGHT

        /// <summary>
        /// Deserialize a byte[] to an object.
        /// </summary>
        /// <typeparam name="T">Target type of the object.</typeparam>
        /// <param name="value">byte[] to be deserialized</param>
        /// <returns>Deserialized object</returns>
        public static T ToObject<T>(this byte[] value)
        {
            MemoryStream m = null;

            try
            {
                m = new MemoryStream(value);

                var bf = new BinaryFormatter();

                return (T)bf.Deserialize(m);
            }
            finally
            {
                if (m != null) m.Dispose();
            }
        }

        /// <summary>
        /// Compress a serializable object to byte[] using the <see cref="CompressionUtils"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Compress(this byte[] value)
        {
            return CompressionUtils.Compress(value);
        }

        /// <summary>
        /// Decompress a serialized byte[] using the <see cref="CompressionUtils"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Decompress(this byte[] value)
        {
            return CompressionUtils.Decompress(value);
        }

#endif

        /// <summary>
        /// Converts a byte array into a hex string
        /// </summary>
        public static string ToHex(this byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var b in bytes)
            {
                sb.Append(b.ToHex());
            }

            return sb.ToString();
        }
    }
}