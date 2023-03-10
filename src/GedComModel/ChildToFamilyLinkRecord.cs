using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("FAMC")]
    public class ChildToFamilyLinkRecord : TypedBaseRecord
    {


        /// <summary>
        /// [adopted | birth | foster | sealing]
        /// A code used to indicate the child to family relationship for pedigree navigation purposes.
        /// Where:
        /// adopted = indicates adoptive parents.
        /// birth = indicates birth parents.
        /// foster = indicates child was included in a foster or guardian family.
        /// sealing = indicates child was sealed to parents other than birth parents.
        /// </summary>
        [GedComTag("PEDI")]
        public string PedigreeLinkageType { get; set; }
    }
}
