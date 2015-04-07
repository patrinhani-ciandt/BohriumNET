using System;
using System.Collections.Generic;

namespace Bohrium.Core.Test.TestHelpers
{
    [Serializable]
    public class DataTestObject
    {
        private string _strValue;

        public int Id { get; set; }

        public Guid ObjectId { get; set; }

        public string StrValue
        {
            get { return _strValue; }
            set { _strValue = value; }
        }

        public long LongValue { get; set; }

        public DateTime DateTimeValue { get; set; }

        public bool BoolValue { get; set; }

        public List<DateTime> ListDateTimeValue { get; set; }

        public DataTestObject RefTestObject { get; set; }

        public IList<DataTestObject> RefTestObjects { get; set; }

        public static DataTestObject CreateDefault()
        {
            return new DataTestObject();
        }

        private void privateMethodSetBoolValueToTrue()
        {
            this.BoolValue = true;
        }
    }
}