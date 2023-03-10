using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    public static class SourceCitationHelper
    {
        public static string GetTitle(this SourceCitationStructure source)
        {
            if (source.Title  != null && !string.IsNullOrEmpty (source.Title.GetText()))
            {
                return source.Title.GetText();
            }
            return source.Abbreviation;
        }

        /// <summary>
        /// Compare 2 SourceCitationStructure is they are the same. This checks only the description and media links for equality
        /// </summary>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsSame(this SourceCitationStructure source, SourceCitationStructure other)
        {
            if (source.GetTitle () != other.GetTitle())
            {
                return false;
            }

            // Now check the list of the same media links
            if (source.Media == null && other.Media == null)
            {
                return true;
            }
            if (source.Media == null || other.Media == null)
            {
                return false;
            }
            if (source.Media.Count != other.Media.Count)
            {
                return false;
            }

            foreach (var m1 in source.Media)
            {
                bool found = false;

                foreach (var m2 in other.Media)
                {
                    if (m1.FileName.Value == m2.FileName.Value)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }
            foreach (var m1 in other.Media)
            {
                bool found = false;

                foreach (var m2 in source.Media)
                {
                    if (m1.FileName.Value == m2.FileName.Value)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }


            return true;
        }
    }
}
