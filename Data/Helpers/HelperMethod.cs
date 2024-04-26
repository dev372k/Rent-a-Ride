using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Helpers
{
    public static class HelperMethod
    {
        public static string FormatNumber(this double number)
        {
            if (number >= 1000000)
            {
                double result = Math.Round(number / 1000000, 2);
                return result.ToString() + "M";
            }
            else if (number >= 1000)
            {
                double result = Math.Round(number / 1000, 2);
                return result.ToString() + "k";
            }
            else
            {
                return number.ToString();
            }
        }

    }
}
