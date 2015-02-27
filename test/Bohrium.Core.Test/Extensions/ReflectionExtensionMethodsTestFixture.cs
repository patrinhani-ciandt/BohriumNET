using Bohrium.Core.Extensions;
using Bohrium.Core.Test.TestHelpers;
using NUnit.Framework;
using System;

namespace Bohrium.Core.Test.Extensions
{
    [TestFixture]
    public class ReflectionExtensionMethodsTestFixture : TestFixtureBase
    {
        private DataTestObject createDataTestObject()
        {
            return new DataTestObject
            {
                BoolValue = false,
                StrValue = "StrValue",
                DateTimeValue = DateTime.Today,
            };
        }

        [Test]
        public void should_be_able_to_get_and_set_value_of_properties()
        {
            var testObj = createDataTestObject();

            var targetStrValue = "New Str Value";

            testObj.SetPropertyValue("StrValue", targetStrValue);

            Assert.AreEqual(targetStrValue, testObj.StrValue);

            var propValue = testObj.GetPropertyValue("StrValue");

            Assert.AreEqual(propValue, testObj.StrValue);
        }

        [Test]
        public void should_be_able_to_get_and_set_value_of_fields()
        {
            var testObj = createDataTestObject();

            var targetStrValue = "New Str Value";

            testObj.SetFieldValue("_strValue", targetStrValue);

            Assert.AreEqual(targetStrValue, testObj.StrValue);

            var propValue = testObj.GetFieldValue("_strValue");

            Assert.AreEqual(propValue, testObj.StrValue);
        }

        [Test]
        public void should_be_able_to_call_methods_with_return_value()
        {
            var testObj = createDataTestObject();

            Assert.AreEqual(testObj.ToString(), testObj.CallMethod("ToString"));
        }

        [Test]
        public void should_be_able_to_call_methods_without_return_value()
        {
            var testObj = createDataTestObject();

            testObj.CallMethod("privateMethodSetBoolValueToTrue");

            Assert.AreEqual(testObj.BoolValue, true);
        }

        [Test]
        public void should_be_able_to_call_methods_with_parameters()
        {
            var testObj = createDataTestObject();

            var targetStrValue = "New Str Value";

            testObj.CallMethod("set_StrValue", objParams: targetStrValue);

            Assert.AreEqual(testObj.StrValue, targetStrValue);
        }
    }
}