using System;
using Bohrium.Core.Extensions;
using NUnit.Framework;

namespace Bohrium.Core.Test.Extensions
{
    [TestFixture]
    public class DateTimeExtensionMethodsTestFixture : TestFixtureBase
    {
        /* Test is covering the methods bellow:
         * DateTime FirstDayOfMonth(this DateTime date)
         */
        [Test]
        public void test_FirstDayOfMonth()
        {
            var originalDate = new DateTime(2014, 8, 17);

            var firstDayOfMonth = originalDate.FirstDayOfMonth();

            Assert.AreEqual(firstDayOfMonth, new DateTime(2014, 8, 1));
        }

        /* Test is covering the methods bellow:
         * DateTime LastDayOfMonth(this DateTime date)
         */
        [Test]
        public void test_LastDayOfMonth()
        {
            var originalDate = new DateTime(2014, 8, 17);

            var lastDayOfMonth = originalDate.LastDayOfMonth();

            Assert.AreEqual(lastDayOfMonth, new DateTime(2014, 8, 31));
        }

        /* Test is covering the methods bellow:
         * DateTime FirstDayOfWeek(this DateTime date)
         */
        [Test]
        public void test_FirstDayOfWeek()
        {
            var originalDate = new DateTime(2014, 8, 16);

            var firstDayOfWeek = originalDate.FirstDayOfWeek();

            Assert.AreEqual(firstDayOfWeek, new DateTime(2014, 8, 10));
        }

        /* Test is covering the methods bellow:
         * DateTime FirstWeekDayOfMonth(this DateTime date)
         */
        [Test]
        public void test_FirstWeekDayOfMonth()
        {
            var originalDate = new DateTime(2014, 8, 16);

            var dateTime = originalDate.FirstWeekDayOfMonth();

            Assert.AreEqual(dateTime, new DateTime(2014, 8, 1));

            originalDate = new DateTime(2014, 6, 16);

            dateTime = originalDate.FirstWeekDayOfMonth();

            Assert.AreEqual(dateTime, new DateTime(2014, 6, 2));
        }

        /* Test is covering the methods bellow:
         * DateTime LastWeekDayOfMonth(this DateTime date)
         */
        [Test]
        public void test_LastWeekDayOfMonth()
        {
            var originalDate = new DateTime(2014, 8, 16);

            var dateTime = originalDate.LastWeekDayOfMonth();

            Assert.AreEqual(dateTime, new DateTime(2014, 8, 29));
        }

        /* Test is covering the methods bellow:
         * DateTime NextWeekDayOfMonth(this DateTime date)
         */
        [Test]
        public void test_NextWeekDayOfMonth()
        {
            var originalDate = new DateTime(2014, 8, 16);

            var dateTime = originalDate.NextWeekDayOfMonth();

            Assert.AreEqual(dateTime, new DateTime(2014, 8, 18));
        }

        /* Test is covering the methods bellow:
         * DateTime PreviousWeekDayOfMonth(this DateTime date)
         */
        [Test]
        public void test_PreviousWeekDayOfMonth()
        {
            var originalDate = new DateTime(2014, 8, 17);

            var dateTime = originalDate.PreviousWeekDayOfMonth();

            Assert.AreEqual(dateTime, new DateTime(2014, 8, 15));
        }

        /* Test is covering the methods bellow:
         * DateTime LastDayOfWeek(this DateTime date)
         */
        [Test]
        public void test_LastDayOfWeek()
        {
            var originalDate = new DateTime(2014, 8, 10);

            var lastDayOfWeek = originalDate.LastDayOfWeek();

            Assert.AreEqual(lastDayOfWeek, new DateTime(2014, 8, 16));
        }

        /* Test is covering the methods bellow:
         * DateTime GetEndOfDay(this DateTime date)
         */
        [Test]
        public void test_GetEndOfDay()
        {
            var originalDate = new DateTime(2014, 8, 17);

            var lastEndOfDay = originalDate.GetEndOfDay();

            var expectedEndOfDay = new DateTime(2014, 8, 17, 23, 59, 59, 999);

            Assert.AreEqual(lastEndOfDay, expectedEndOfDay);
        }

        /* Test is covering the methods bellow:
         * DateTime SetTime(this DateTime date)
         */
        [Test]
        public void test_SetTime()
        {
            var originalDate = new DateTime(2014, 8, 17);

            var dateWithSetTime = originalDate.SetTime(14, 35, 48, 789);

            var expectedDayTime = new DateTime(2014, 8, 17, 14, 35, 48, 789);

            Assert.AreEqual(dateWithSetTime, expectedDayTime);
        }

        /* Test is covering the methods bellow:
         * DateTime IsWeekend(this DateTime date)
         */
        [Test]
        public void test_IsWeekend()
        {
            var originalDate = new DateTime(2014, 8, 10);
            Assert.IsTrue(originalDate.IsWeekend());

            originalDate = new DateTime(2014, 8, 11);
            Assert.IsFalse(originalDate.IsWeekend());

            originalDate = new DateTime(2014, 8, 12);
            Assert.IsFalse(originalDate.IsWeekend());

            originalDate = new DateTime(2014, 8, 13);
            Assert.IsFalse(originalDate.IsWeekend());

            originalDate = new DateTime(2014, 8, 14);
            Assert.IsFalse(originalDate.IsWeekend());

            originalDate = new DateTime(2014, 8, 15);
            Assert.IsFalse(originalDate.IsWeekend());

            originalDate = new DateTime(2014, 8, 16);
            Assert.IsTrue(originalDate.IsWeekend());
        }

        /* Test is covering the methods bellow:
         * DateTime IsWeekDay(this DateTime date)
         */
        [Test]
        public void test_IsWeekDay()
        {
            var originalDate = new DateTime(2014, 8, 10);
            Assert.IsFalse(originalDate.IsWeekDay());

            originalDate = new DateTime(2014, 8, 11);
            Assert.IsTrue(originalDate.IsWeekDay());

            originalDate = new DateTime(2014, 8, 12);
            Assert.IsTrue(originalDate.IsWeekDay());

            originalDate = new DateTime(2014, 8, 13);
            Assert.IsTrue(originalDate.IsWeekDay());

            originalDate = new DateTime(2014, 8, 14);
            Assert.IsTrue(originalDate.IsWeekDay());

            originalDate = new DateTime(2014, 8, 15);
            Assert.IsTrue(originalDate.IsWeekDay());

            originalDate = new DateTime(2014, 8, 16);
            Assert.IsFalse(originalDate.IsWeekDay());
        }
    }
}
