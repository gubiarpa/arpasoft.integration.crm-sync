using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public static class ValidateProcess
    {
        public static bool isInt32(String num)
        {
            bool isNum;
            double retNum;

            try
            {
                isNum = Double.TryParse(Convert.ToString(num), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
                return isNum;
            }
            catch
            {
                return false;
            }
        }

    }
}