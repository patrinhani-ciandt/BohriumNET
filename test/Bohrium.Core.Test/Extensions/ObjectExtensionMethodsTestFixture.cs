using Bohrium.Core.Extensions;
using Bohrium.Core.Test.TestHelpers;
using NUnit.Framework;

namespace Bohrium.Core.Test.Extensions
{
    [TestFixture]
    public class ObjectExtensionMethodsTestFixture : TestFixtureBase
    {
        /* Test is covering the methods bellow:
         * byte[] ToByteArray(this object value, bool compress = false)
         */
        [Test]
        public void should_be_able_to_convert_an_object_to_byte_array()
        {
            var dataTestObject = DataTestObject.CreateDefault();

            var byteArrayDataTestObject = dataTestObject.ToByteArray();

            var dataTestObjectCompressed = dataTestObject.ToByteArray(true);

            Assert.AreNotEqual(byteArrayDataTestObject, dataTestObjectCompressed);

            Assert.IsTrue(dataTestObjectCompressed.Length < byteArrayDataTestObject.Length);
        }

        /*
         * bool IsNull(this object value)
         * bool IsNotNull(this object value)
         */
        [Test]
        public void test_for_IsNull_IsNotNull()
        {
            DataTestObject dataTestObject = null;

            Assert.AreEqual((dataTestObject.IsNull()), (dataTestObject == null));

            dataTestObject = DataTestObject.CreateDefault();

            Assert.AreEqual((dataTestObject.IsNotNull()), (dataTestObject != null));
        }

        /* Test is covering the methods bellow:
         * byte[] ComputeMD5Hash(this object obj)
         */
        [Test]
        public void should_be_able_to_compute_the_MD5_hash_for_a_serializable_type_and_compare_different_instances()
        {
            var dataTestObject01 = DataTestObject.CreateDefault();
            var dataTestObject02 = DataTestObject.CreateDefault();

            Assert.AreNotEqual(dataTestObject01, dataTestObject02);
            Assert.AreNotSame(dataTestObject01, dataTestObject02);

            var dataTestObject01MD5 = dataTestObject01.ComputeMD5Hash();
            var dataTestObject02MD5 = dataTestObject02.ComputeMD5Hash();

            Assert.AreEqual(dataTestObject01MD5, dataTestObject02MD5);
        }

        /* Test is covering the methods bellow:
         * string ToMD5HashString(this object obj)
         */
        [Test]
        public void should_be_able_to_compute_the_MD5_hash_and_get_MD5_string_representation_for_a_serializable_type_and_compare_different_instances()
        {
            var dataTestObject01 = DataTestObject.CreateDefault();
            var dataTestObject02 = DataTestObject.CreateDefault();

            Assert.AreNotEqual(dataTestObject01, dataTestObject02);
            Assert.AreNotSame(dataTestObject01, dataTestObject02);

            var dataTestObject01MD5 = dataTestObject01.ToMD5HashString();
            var dataTestObject02MD5 = dataTestObject02.ToMD5HashString();

            Assert.AreEqual(dataTestObject01MD5, dataTestObject02MD5);
        }
    }
}
