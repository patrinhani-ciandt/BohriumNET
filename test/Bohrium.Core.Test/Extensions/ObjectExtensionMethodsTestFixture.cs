using Bohrium.Core.Extensions;
using Bohrium.Core.Test.TestHelpers;
using NUnit.Framework;

namespace Bohrium.Core.Test.Extensions
{
    [TestFixture]
    public class ObjectExtensionMethodsTestFixture
    {
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
