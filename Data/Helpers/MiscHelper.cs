using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Helpers
{
    public class MiscHelper
    {
        public static string FormatNumber(double number)
        {
            const double Million = 1000000;
            const double Thousand = 1000;

            if (number >= Million)
                return (number / Million).ToString("0.##M");
            else if (number >= Thousand)
                return (number / Thousand).ToString("0.##K");
            else
                return number.ToString();
        }
    }
}
