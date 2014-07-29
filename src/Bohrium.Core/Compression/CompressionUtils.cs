namespace Bohrium.Core.Compression
{
    using System;
    using System.IO;
    using System.IO.Compression;

    public class CompressionUtils
    {
        /// <summary>
        /// Compress a byte[].
        /// </summary>
        /// <param name="buffer">byte[] to be compressed.</param>
        /// <returns>byte array compressed.</returns>
        public static byte[] Compress(byte[] buffer)
        {
            var ms = new MemoryStream();

            var zip = new GZipStream(ms, CompressionMode.Compress, true);

            zip.Write(buffer, 0, buffer.Length);

            zip.Close();

            ms.Position = 0;

            var outStream = new MemoryStream();

            var compressed = new byte[ms.Length];

            ms.Read(compressed, 0, compressed.Length);

            var gzBuffer = new byte[compressed.Length + 4];

            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);

            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);

            return gzBuffer;
        }

        /// <summary>
        /// Decompress a byte[].
        /// </summary>
        /// <param name="buffer">byte[] to be decompressed.</param>
        /// <returns>byte array decompressed.</returns>
        public static byte[] Decompress(byte[] gzBuffer)
        {
            var ms = new MemoryStream();

            var msgLength = BitConverter.ToInt32(gzBuffer, 0);

            ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

            var buffer = new byte[msgLength];

            ms.Position = 0;

            var zip = new GZipStream(ms, CompressionMode.Decompress);

            zip.Read(buffer, 0, buffer.Length);

            return buffer;
        }
    }
}