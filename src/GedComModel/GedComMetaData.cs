using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("GEDC")]
    public class GedComMetaData : TypedBaseRecord
    {
        /// <summary>
        /// An identifier that represents the version level assigned to the associated product. 
        /// It is defined and changed by the creators of the product.
        /// <remarks>
        /// In this Case it is the version of the GEDCOM Specification. The parser is designed for GEDCOM Version 5.5.1
        /// </remarks>
        /// </summary>
        [GedComTag("VERS")]
        public string Version { get; set; }

        /// <summary>
        /// [ LINEAGE-LINKED ] The GEDCOM form used to construct this transmission. There maybe other forms used such as 
        /// CommSoft's "EVENT_LINEAGE_LINKED" but these specifications define only the LINEAGELINKED Form.  
        /// Systems will use this value to specify GEDCOM compatible with these specification
        /// </summary>
        [GedComTag("FORM")]
        public string Format { get; set; }
    }
}
