using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AreteTester.Actions
{
    public class Assert
    {
        public static bool AreEqual(object expected, object actual, AssertEqualType matchType)
        {
            bool matched= false;
            switch (matchType)
            {
                case AssertEqualType.String:
                    matched = (string)expected == (string)actual;
                    break;
                case AssertEqualType.Integer:
                    matched = Convert.ToInt32(expected) == Convert.ToInt32(actual);
                    break;
                case AssertEqualType.Decimal:
                    matched = Convert.ToDecimal(expected) == Convert.ToDecimal(actual);
                    break;
            }
            return matched;
        }

        public static bool IsTrue(object value)
        {
            return (bool)value == true;
        }

        public static bool IsFalse(object value)
        {
            return (bool)value == false;
        }
    }
}
