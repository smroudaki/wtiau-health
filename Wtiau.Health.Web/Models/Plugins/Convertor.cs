using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.Plugins
{
    public class Convertor
    {
        public static double SetPrecision(double num, int precision)
        {
            string p = string.Empty;

            for (int i = 0; i < precision; i++)
            {
                p += 0;
            }

            return Convert.ToDouble(string.Format("{0:0."+ p + "}", num));
        }
    }
}