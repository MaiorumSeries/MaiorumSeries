using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    public static class ExtendedTextHelper
    {
        public static string GetText(this ExtendedTextRecord text, CultureInfo cultureInfo)
        {
            var str = new StringBuilder();


            str.Append(text.Value);

            foreach (var l in text.Lines)
            {
                if (l.Tag == "CONT")
                {
                    str.Append("\\n");
                }
                str.Append(l.Value);
            }

            var s = str.ToString();
            s = s.Replace("<i>", "");
            s = s.Replace("</i>", "");
            return s;
        }
    }
}
