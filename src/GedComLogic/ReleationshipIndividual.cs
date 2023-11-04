using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    /// <summary>
    /// Der Proband erhält unabhängig vom Geschlecht die Nummer 1, sein Vater die Nummer 2, die Mutter die Nummer 3. 
    /// Hat eine Person die Nummer n (z. B. 2 für den Vater), dann erhält deren Vater die Nummer 2·n (also Verdopplung 
    /// und damit die Nummer 4 für den Großvater väterlicherseits des Probanden), die Mutter 2·n + 1 (hier also die 5). 
    /// Der Vater von 10 erhält die Nummer 20, die Mutter erhält die Nummer 21.
    /// Alle männlichen Vorfahren haben demzufolge gerade Zahlen, alle weiblichen ungerade.
    /// </summary>
    public class ReleationshipIndividual
    {
        private int predecessorId = 1;
        private readonly IndividualRecord _record;

        public IndividualRecord Individual
        {
            get
            {
                return _record;
            }
        }

        /// <summary>
        /// Construcor from IndividualRecord 
        /// </summary>
        /// <param name="record"></param>
        public ReleationshipIndividual(IndividualRecord record)
        {
            _record = record;
        }


        public ReleationshipIndividual GetAsFather(IndividualRecord father)
        {
            var result = new ReleationshipIndividual(father)
            {
                predecessorId = predecessorId * 2
            };
            return result;
        }

        public ReleationshipIndividual GetAsMother(IndividualRecord mother)
        {
            var result = new ReleationshipIndividual(mother)
            {
                predecessorId = predecessorId * 2 + 1
            };
            return result;
        }
        public ReleationshipIndividual GetAsSpouse(IndividualRecord spouse)
        {
            var result = new ReleationshipIndividual(spouse);
            return result;
        }

        public ReleationshipIndividual GetAsChild(IndividualRecord child)
        {
            var result = new ReleationshipIndividual(child);
            return result;
        }

        /// <summary>
        /// Get the String representation of the 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return predecessorId.ToString();    
        }
    }
}
