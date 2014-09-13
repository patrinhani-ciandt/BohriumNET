using System;
using System.Text.RegularExpressions;
using Bohrium.Core.Extensions;
using Bohrium.Core.Test.TestHelpers;
using NUnit.Framework;

namespace Bohrium.Core.Test.Extensions
{
    [TestFixture]
    public class StringExtensionMethodsTestFixture : TestFixtureBase
    {
        /* Test is covering the methods bellow:
         * T RegexDelete(this String value, string regularExpr)
         */
        [Test]
        public void should_be_able_to_replace_a_part_of_a_string_by_a_regular_expression()
        {
            var sourceString = "Test of a    string with white spaces";
            var expectedString = "Test of a different string with white spaces";

            var regExpr = @"(of(\s+)a(\s+))";

            var targetString = sourceString.RegexReplace(regExpr, "of a different ");

            Assert.AreEqual(targetString, expectedString);
        }

        /* Test is covering the methods bellow:
         * T RegexDelete(this String value, string regularExpr)
         */
        [Test]
        public void should_be_able_to_delete_a_part_of_a_string_by_a_regular_expression()
        {
            var sourceString = "Test of a    string with white spaces";
            var expectedString = "Test a    string with white spaces";

            var regExpr = @"(of[ ])";

            var targetString = sourceString.RegexDelete(regExpr);

            Assert.AreEqual(targetString, expectedString);
        }
    }
}
