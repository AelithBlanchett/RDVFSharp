using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Helpers
{
    public static class Util
    {
        public static string FormatCharacterName(this string character)
        {
            return $"[icon]{character}[/icon][user]{character}[/user]";
        }
    }
}
