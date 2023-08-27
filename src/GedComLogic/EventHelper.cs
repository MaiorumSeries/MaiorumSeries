using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    public static class EventHelper
    {
        public static bool IsCommonEventItem(this EventDetailRecord eventDetail)
        {
            switch (eventDetail.Tag)
            {
                case "BIRT":
                    return true;
                case "CHR":
                    return true;
                case "BAPM":
                    return true;
                case "DEAT":
                    return true;
                case "BURI":
                    return true;
                case "RESI":
                    if (eventDetail.Notes.Count > 0) return false;
                    if (eventDetail.Media.Count > 0) return false;
                    return true;
            }
            return false;
        }
        public static bool IsPlaceReference (this PlaceRecord place)
        {
            if (place == null) return false;

            if (place.Media?.Count > 0) return false;
            if (place.Notes?.Count > 0) return false;
            return true;
        }


        public static bool IsNonCommonEventItem(this EventDetailRecord eventDetail, Model model)
        {
            if (eventDetail.Address != null) return true;
            if (eventDetail.Notes?.Count > 0) return true;
            if (eventDetail.Media?.Count > 0) return true;
            if (eventDetail.Place != null && !eventDetail.Place.IsPlaceReference()) return true;
            return false;
        }

        /// <summary>
        /// Get the event information from an event record 
        /// </summary>
        /// <param name="eventDetail">Details from an event record</param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static EventItem GetEventItem(this EventDetailRecord eventDetail, CultureInfo cultureInfo)
        {
            EventItem item = new EventItem();

            var dateRecord = new DateRecord()
            {
                Value = eventDetail.Date.Value
            };
            item.Tag = eventDetail.Tag;
            item.Date = dateRecord.GetDateTime();
            item.DateString = eventDetail.Date.GetDisplayDate(cultureInfo);

            var place = eventDetail.GetDisplayPlace();

            switch (eventDetail.Tag)
            {
                case "BIRT":
                    item.Description = Strings.ResourceManager.GetString("Born", cultureInfo);
                    break;
                case "CHR":
                    item.Description = Strings.ResourceManager.GetString("Born", cultureInfo);
                    break;
                case "BAPM":
                    item.Description = Strings.ResourceManager.GetString("Baptized", cultureInfo);
                    break;
                case "MARR":
                    item.Description = Strings.ResourceManager.GetString("Married", cultureInfo);
                    break;
                case "RESI":
                    item.Description = Strings.ResourceManager.GetString("Resident", cultureInfo);
                    break;
                case "DEAT":
                    item.Description = Strings.ResourceManager.GetString("Died", cultureInfo);
                    break;
                case "BURI":
                    item.Description = Strings.ResourceManager.GetString("Buried", cultureInfo);
                    break;
                default:
                    item.Description = eventDetail.Tag;
                    break;
            }
            if (!string.IsNullOrEmpty(place))
            {
                item.Description += " in ";
                item.Description += place;
            }

            return item;
        }

        public static string GetDisplayDate(this EventDetailRecord eventDetail, CultureInfo cultureInfo)
        {
            // TODO 
            var str = new StringBuilder();

            if (eventDetail.Date != null)
            {
                if (!string.IsNullOrEmpty(eventDetail.Date.Value)) 
                {
                    str.Append(eventDetail.Date.Value);
                }
                else
                {
                    str.Append("");
                }
            }
            else
            {
                str.Append("");
            }
            return str.ToString();
        }
        public static string GetDisplayPlace(this EventDetailRecord eventDetail)
        {
            var str = new StringBuilder();

            if (eventDetail.Place != null)
            {
                if (!string.IsNullOrEmpty(eventDetail.Place.Value))
                {
                    str.Append(eventDetail.Place.Value.Replace(" ", @" \ "));
                }
                else
                {
                    str.Append("");
                }
            }
            else
            {
                str.Append("");
            }
            return str.ToString();
        }

        public static string GetText(this EventDetailRecord eventDetail)
        {
            var str = new StringBuilder();


            str.Append(eventDetail.Value);

            foreach (var l in eventDetail.Lines)
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
