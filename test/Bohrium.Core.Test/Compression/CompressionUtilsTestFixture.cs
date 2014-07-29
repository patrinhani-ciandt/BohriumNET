using System;
using Bohrium.Core.Compression;
using Bohrium.Core.Test.TestHelpers;
using NUnit.Framework;
using Bohrium.Core.Extensions;

namespace Bohrium.Core.Test.Compression
{
    [TestFixture]
    public class CompressionUtilsTestFixture
    {
        [Test]
        public void should_be_able_to_compress_and_decompress_a_serializable_object_into_a_byte_array()
        {
            var dataTestObject = new DataTestObject();

            var dataTestObjectBytes = dataTestObject.ToByteArray();

            var compressedDataTestObjectBytes = CompressionUtils.Compress(dataTestObjectBytes);

            Assert.AreNotEqual(dataTestObjectBytes, compressedDataTestObjectBytes);
            Assert.AreNotSame(dataTestObjectBytes, compressedDataTestObjectBytes);

            var decompressedDataTestObjectBytes = CompressionUtils.Decompress(compressedDataTestObjectBytes);

            Assert.AreEqual(dataTestObjectBytes, decompressedDataTestObjectBytes);
            Assert.AreNotSame(dataTestObjectBytes, decompressedDataTestObjectBytes);

            Console.WriteLine("Original object size: {0} bytes.", dataTestObjectBytes.Length);
            Console.WriteLine("Compressed object size: {0} bytes.", compressedDataTestObjectBytes.Length);

            var compressRate = (compressedDataTestObjectBytes.Length.CastTo<double>() / dataTestObjectBytes.Length.CastTo<double>()) * 100;

            Console.WriteLine("Compression rate: {0:N2}%.", compressRate);
        }
    }
}
