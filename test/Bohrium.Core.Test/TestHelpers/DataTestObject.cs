using System;
using System.Collections.Generic;

namespace Bohrium.Core.Test.TestHelpers
{
    [Serializable]
    public class DataTestObject
    {
        public int Id { get; set; }
        public Guid ObjectId { get; set; }
        public string StrValue { get; set; }
        public long LongValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public bool BoolValue { get; set; }
        public List<DateTime> ListDateTimeValue { get; set; }

        public static DataTestObject CreateDefault()
        {
            return new DataTestObject();
        }
    }
}