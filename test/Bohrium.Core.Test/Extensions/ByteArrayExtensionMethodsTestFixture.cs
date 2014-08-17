using System;
using Bohrium.Core.Extensions;
using Bohrium.Core.Test.TestHelpers;
using NUnit.Framework;

namespace Bohrium.Core.Test.Extensions
{
    [TestFixture]
    public class ByteArrayExtensionMethodsTestFixture : TestFixtureBase
    {
        /* Test is covering the methods bellow:
         * T ToObject<T>(this byte[] value)
         */
        [Test]
        public void should_be_able_to_deserialize_a_byte_array_to_a_serializable_type()
        {
            var dataTestObject = DataTestObject.CreateDefault();

            var byteArrayDataTestObject = dataTestObject.ToByteArray();

            var expectedDataTestObject = byteArrayDataTestObject.ToObject<DataTestObject>();

            var dataTestObjectMD5 = dataTestObject.ComputeMD5Hash();
            var expectedDataTestObjectMD5 = expectedDataTestObject.ComputeMD5Hash();

            Assert.IsNotNull(expectedDataTestObject);
            Assert.IsInstanceOf<DataTestObject>(expectedDataTestObject);
            Assert.AreEqual(dataTestObjectMD5, expectedDataTestObjectMD5);
            Assert.AreNotSame(dataTestObject, expectedDataTestObject);
        }

        /* Test is covering the methods bellow:
         * byte[] Compress(this byte[] value)
         * byte[] Decompress(this byte[] value)
         */
        [Test]
        public void should_be_able_to_compress_and_decompress_an_object()
        {
            var dataTestObject = DataTestObject.CreateDefault();

            var byteArrayDataTestObject = dataTestObject.ToByteArray();

            var dataTestObjectCompressed = byteArrayDataTestObject.Compress();

            Assert.AreNotEqual(byteArrayDataTestObject, dataTestObjectCompressed);

            Assert.IsTrue(dataTestObjectCompressed.Length < byteArrayDataTestObject.Length);

            var dataTestObjectDecompressed = dataTestObjectCompressed.Decompress();

            Assert.AreEqual(byteArrayDataTestObject, dataTestObjectDecompressed);
        }

        /* Test is covering the methods bellow:
         * string ToHex(this byte[] bytes)
         */
        [Test]
        public void should_be_able_to_convert_a_byte_array_to_a_hex_string()
        {
            var bytes = new byte[] { 0xF1, 0x00, 0x40, 0xAA };

            Assert.AreEqual(bytes.ToHex(), "f10040aa");
        }
    }
}
