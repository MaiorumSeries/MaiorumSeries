using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    public static class DateHelper
    {

        private class DateData
        {
            internal int? Year;
            internal int? Month;
            internal int? Day;
        }


        private static int MapStringToMonth(string str)
        {
            switch (str.ToUpper())
            {
                case "JAN":
                    return 1;
                case "FEB":
                    return 2;
                case "MAR":
                    return 3;
                case "MÄRZ":
                    return 3;
                case "MÄR":
                    return 3;
                case "APR":
                    return 4;
                case "MAY":
                    return 5;
                case "JUN":
                    return 6;
                case "JUL":
                    return 7;
                case "AUG":
                    return 8;
                case "SEP":
                    return 9;
                case "OCT":
                    return 10;
                case "NOV":
                    return 11;
                case "DEC":
                    return 12;
            }
            return -1;
        }


        private static  bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }



        private static DateData ParseString (string stringRepresentation)
        {
            var data = new DateData();
            string[] parts = stringRepresentation.Split(' ');
            if (parts.Length == 3)
            {
                if (IsDigitsOnly(parts[0]))
                {
                    int dayValue;
                    if (int.TryParse (parts[0], out dayValue))
                    {
                        if (dayValue >= 1 && dayValue <= 31)
                        {
                            data.Day = dayValue;
                        }
                    }
                }
                if (IsDigitsOnly(parts[1]))
                {
                    int monthValue;
                    if (int.TryParse(parts[1], out monthValue))
                    {
                        if (monthValue >= 1 && monthValue <= 12)
                        {
                            data.Month = monthValue;
                        }
                    }
                }
                else if (MapStringToMonth (parts[1]) > 0)
                {
                    data.Month = MapStringToMonth(parts[1]);
                }
                if (IsDigitsOnly(parts[2]))
                {
                    int yearValue;
                    if (int.TryParse(parts[2], out yearValue))
                    {
                        data.Year = yearValue;
                    }
                }

            }
            if (parts.Length == 2)
            {
                if (IsDigitsOnly(parts[0]))
                {
                    int monthValue;
                    if (int.TryParse(parts[0], out monthValue))
                    {
                        if (monthValue >= 1 && monthValue <= 12)
                        {
                            data.Month = monthValue;
                        }
                    }
                }
                else if (MapStringToMonth(parts[0]) > 0)
                {
                    data.Month = MapStringToMonth(parts[0]);
                }
                if (IsDigitsOnly(parts[1]))
                {
                    int yearValue;
                    if (int.TryParse(parts[1], out yearValue))
                    {
                        data.Year = yearValue;
                    }
                }

            }

            if (parts.Length == 1)
            {
                if (IsDigitsOnly(parts[0]))
                {
                    int yearValue;
                    if (int.TryParse(parts[0], out yearValue))
                    {
                        data.Year = yearValue;
                    }
                }

            }
            return data;
        }

        private static Random s_random = new Random();
        private static string[] s_months =
        {
            "JAN",
            "FEB",
            "MAR",
            "APR",
            "MAY",
            "JUN",
            "JUL",
            "AUG",
            "SEP",
            "OCT",
            "NOV",
            "DEC",
        };

        public static DateRecord GetRandomDateRecord ()
        {
            var date = new DateRecord();
            var str = new StringBuilder();

            str.Append(s_random.Next(1, 30).ToString());
            str.Append(" ");
            str.Append(s_months[s_random.Next(11)]);
            str.Append(" ");
            str.Append(s_random.Next(1750, 2010).ToString());
            date.Value = str.ToString();
            return date;
        }

        public static string GetDisplayDate(this DateRecord dateDetail, CultureInfo cultureInfo)
        {
            // TODO 
            var str = new StringBuilder();
            var data = ParseString(dateDetail.Value);

            if (data != null)
            {
                if (data.Day.HasValue && data.Month.HasValue && data.Year.HasValue)
                {
                    if (cultureInfo.Name == "de-DE")
                    {
                        str.Append(data.Day.ToString());
                        str.Append("-");
                        str.Append(data.Month.ToString());
                        str.Append("-");
                        str.Append(data.Year.ToString());
                    }
                    else
                    {
                        str.Append(data.Month.ToString());
                        str.Append("/");
                        str.Append(data.Day.ToString());
                        str.Append("/");
                        str.Append(data.Year.ToString());

                    }
                }
                else if (data.Month.HasValue && data.Year.HasValue)
                {
                    if (cultureInfo.Name == "de-DE")
                    {
                        str.Append(data.Month.ToString());
                        str.Append("-");
                        str.Append(data.Year.ToString());
                    }
                    else
                    {
                        str.Append(data.Month.ToString());
                        str.Append("/");
                        str.Append(data.Year.ToString());

                    }
                }
                else if (data.Year.HasValue)
                {
                    str.Append(data.Year.ToString());
                }
            }
            else
            {
                return dateDetail.Value;
            }

            return str.ToString();
        }

        public static DateTime GetDateTime(this DateRecord dateDetail)
        {
            int year = 1700;
            int month = 1;
            int day = 12;

            var data = ParseString(dateDetail.Value);
            if (data != null)
            {
                if (data.Year.HasValue)
                {
                    year = data.Year.Value;
                }
                if (data.Month.HasValue)
                {
                    month = data.Month.Value;
                }
                if (data.Day.HasValue)
                {
                    day = data.Day.Value;
                }
            }
            var date = new DateTime(year, month, day);
            return date;
        }
    }
}
