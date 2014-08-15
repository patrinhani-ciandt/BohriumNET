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
    }
}
