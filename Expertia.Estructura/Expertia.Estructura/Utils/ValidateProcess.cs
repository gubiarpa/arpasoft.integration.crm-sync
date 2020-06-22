using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public static bool validarEmail(string email_de)
        {
            String expresion; expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email_de, expresion))
            {
                if (Regex.Replace(email_de, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}