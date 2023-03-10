using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    public static class IndividualHelper
    {
        /// <summary>
        /// Get the display name in the book for the indiviual
        /// </summary>
        /// <param name="individualRecord"></param>
        /// <returns></returns>
        public static string GetDisplayName(this IndividualRecord individualRecord)
        {
            var strFormal = new StringBuilder();

            var nameRecord = individualRecord.Names.FirstOrDefault();
            if (nameRecord != null)
            {
                if (!string.IsNullOrEmpty(nameRecord.GivenName) || (!string.IsNullOrEmpty(nameRecord.SurName)))
                {
                    if (!string.IsNullOrEmpty(nameRecord.GivenName))
                    {
                        strFormal.Append(nameRecord.GivenName);
                    }
                    if (!string.IsNullOrEmpty(nameRecord.SurName))
                    {
                        strFormal.Append(" ");
                        strFormal.Append(nameRecord.SurName);
                    }
                }
                else
                {
                    strFormal.Append(nameRecord.Value.Replace("/", ""));
                }
            }
            else
            {
                strFormal.Append(Strings.NoDisplayNameForIndividual);
            }
            return strFormal.ToString();
        }

        /// <summary>
        /// Get a short summary text, very similar to a abstract. This should be possible with very little information. 
        /// </summary>
        /// <param name="individualRecord"></param>
        /// <returns></returns>
        public static string GetSummary(this IndividualRecord individualRecord, Model model, CultureInfo cultureInfo)
        {
            var str = new StringBuilder();

            var nameRecord = individualRecord.Names.FirstOrDefault();
            if (nameRecord != null)
            {
                var title = individualRecord.Events.Find(x => x.Tag == "TITL");
                if (title != null)
                {
                    str.Append(GetDisplayName(individualRecord));
                    str.Append(" ist von Beruf ");
                    str.Append(title.Value);
                    str.Append(". ");
                }
                var occu = individualRecord.Events.Find(x => x.Tag == "OCCU");
                if (occu != null)
                {
                    str.Append(GetDisplayName(individualRecord));
                    str.Append(" ist von Beruf ");
                    str.Append(occu.Value);
                    str.Append(". ");
                }

                var birth = individualRecord.Events.Find(x => x.Tag == "BIRT");
                var death = individualRecord.Events.Find(x => x.Tag == "DEAD");

                if ((birth != null) && (death != null))
                {
                    str.Append(GetDisplayName(individualRecord));
                    str.Append(" ist geboren am ");
                    str.Append(birth.GetDisplayDate(cultureInfo));
                    string place = birth.GetDisplayPlace();
                    if (!string.IsNullOrEmpty(place))
                    {
                        str.Append(" in ");
                        str.Append(place);
                    }
                    str.Append(" und gestorben am  ");
                    str.Append(death.GetDisplayDate(cultureInfo));
                    string place2 = death.GetDisplayPlace();
                    if (!string.IsNullOrEmpty(place2))
                    {
                        str.Append(" in ");
                        str.Append(place2);
                    }
                    str.Append(". ");

                }
                else if (birth != null)
                {
                    str.Append(GetDisplayName(individualRecord));
                    str.Append(" ist geboren am ");
                    str.Append(birth.GetDisplayDate (cultureInfo));
                    string place = birth.GetDisplayPlace();
                    if (!string.IsNullOrEmpty (place))
                    {
                        str.Append(" in ");
                        str.Append(place);
                    }
                    str.Append(". ");
                }
                else if (death != null)
                {
                    str.Append(GetDisplayName(individualRecord));
                    str.Append(" ist gestorben am ");
                    str.Append(death.GetDisplayDate(cultureInfo));
                    string place = death.GetDisplayPlace();
                    if (!string.IsNullOrEmpty(place))
                    {
                        str.Append(" in ");
                        str.Append(place);
                    }
                    str.Append(". ");
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// Get a list of event descriptions for that individual.
        /// </summary>
        /// <param name="individualRecord"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static List<EventItem> GetEventList(this IndividualRecord individualRecord, Model model, CultureInfo cultureInfo)
        {
            var list = new List<EventItem>();
            foreach (var e in individualRecord.Events)
            {
                if (e.Date == null) continue;
                var i = e.GetEventItem(cultureInfo);
                if (i != null)
                {
                    list.Add(i);
                }
            }
            foreach (var spouseTo in individualRecord.SpouseTo)
            {
                var family = model.Families.Find(x => x.XrefId == spouseTo.Value);
                if (family != null)
                {
                    foreach (var e in family.Events)
                    {
                        if (e.Date == null) continue;
                        var i = e.GetEventItem(cultureInfo);
                        if (i != null)
                        {
                            list.Add(i);
                        }
                    }
                }
            }
            var work = list.OrderBy(x => x.Date);

            var result = new List<EventItem>();
            foreach (var i in work)
            {
                result.Add(i);
            }
            return result;
        }




        /// <summary>
        /// Get a list of event descriptions for that individual.
        /// </summary>
        /// <param name="individualRecord"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static List<EventDetailRecord> GetNonCommonEventList(this IndividualRecord individualRecord, Model model, CultureInfo cultureInfo)
        {
            var list = new List<EventDetailRecord>();
            foreach (var e in individualRecord.Events)
            {
                if (e.IsNonCommonEventItem())
                {
                    list.Add(e);
                }
            }
            foreach (var spouseTo in individualRecord.SpouseTo)
            {
                var family = model.Families.Find(x => x.XrefId == spouseTo.Value);
                if (family != null)
                {
                    foreach (var e in family.Events)
                    {
                        if (e.IsNonCommonEventItem())
                        {
                            list.Add(e);
                        }
                    }
                }
            }
            return list;
            //var work = list.OrderBy(x => x.Date);

            //var result = new List<EventDetailRecord>();
            //foreach (var i in work)
            //{
            //    result.Add(i);
            //}
            //return result;
        }
        /// <summary>
        /// The list if names of this person, in latex index format
        /// </summary>
        /// <param name="individualRecord"></param>
        /// <returns></returns>
        public static List<string> GetNameIndexList(this IndividualRecord individualRecord)
        {
            var list = new List<string>();
            foreach (var nameRecord in individualRecord.Names)
            {
                var str = new StringBuilder();

                if (!string.IsNullOrEmpty(nameRecord.GivenName) || (!string.IsNullOrEmpty(nameRecord.SurName)))
                {
                    if (!string.IsNullOrEmpty(nameRecord.SurName))
                    {
                        str.Append(nameRecord.SurName);
                    }
                    if (!string.IsNullOrEmpty(nameRecord.GivenName))
                    {
                        str.Append("!");
                        str.Append(nameRecord.GivenName);
                    }
                }
                else
                {
                    str.Append(nameRecord.Value.Replace("/", ""));
                }
                list.Add(str.ToString());
            }
            return list;
        }
    }
}
