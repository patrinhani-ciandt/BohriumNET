namespace Bohrium.Core.Extensions
{
    /// <summary>
    /// Class with extension methods for byte
    /// </summary>
    public static class ByteExtensionMethods
    {
        /// <summary>
        /// Converts a single byte to a hex string
        /// </summary>
        public static string ToHex(this byte byteValue)
        {
            return byteValue.ToString("x2");
        }
    }
}