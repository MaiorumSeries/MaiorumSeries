using MaiorumSeries.GedComModel;
using System;
using DuoVia.FuzzyStrings;
using System.Globalization;

namespace MaiorumSeries.GedComLogic
{
    public static class ModelHelper
    {

        public static CultureInfo GetCultureInfo (this Model model)
        {
            var cultureInfo = new CultureInfo("en-US");
            string language = model?.Head?.Language;
            if (!string.IsNullOrEmpty(language))
            {
                switch (language.ToLower())
                {
                    case "german":
                        cultureInfo = new CultureInfo("de-DE");
                        break;
                }
            }
            return cultureInfo;
        }

        public static string FindBestNameMatch(this Model model, string inputName)
        {
            string bestMatch = "";
            double lastCoeficence = 0.0;

            foreach (var indi in model.Individuals)
            {
                // Check if there is any name for the individual 
                if (indi.Names.Count > 0)
                {
                    // Just take the first name; no alternatives.
                    if (!string.IsNullOrEmpty( indi.Names[0].Value))
                    {
                        double check = indi.Names[0].Value.DiceCoefficient(inputName);
                        if (string.IsNullOrEmpty (bestMatch))
                        {
                            bestMatch = indi.XrefId;
                            lastCoeficence = check;
                        }
                        else
                        {
                            if (check > lastCoeficence)
                            {
                                bestMatch = indi.XrefId;
                                lastCoeficence = check;
                            }
                        }
                    }
                }
            }
            return bestMatch;
        }
    }
}
