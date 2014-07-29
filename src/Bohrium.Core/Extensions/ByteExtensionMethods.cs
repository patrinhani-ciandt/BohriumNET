namespace Bohrium.Core.Extensions
{
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
