using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace URLShortener.DAL.Components
{
    public static class Extensions
    {
        public static Guid ToGuid(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return Guid.Empty;

            if (Guid.TryParse(s, out Guid g))
                return g;
            return Guid.Empty;
        }

        public static string ToPhone(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            // strip to just numbers
            return Regex.Replace(str, "[^0-9]", "");
        }
    }
}
