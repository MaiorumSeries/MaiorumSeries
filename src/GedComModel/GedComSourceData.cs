using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("HEAD.SOUR")]
    public class GedComSourceData : TypedBaseRecord
    {
        /// <summary>
        /// An identifier that represents the version level assigned to the associated product. 
        /// It is defined and changed by the creators of the product.
        /// <remarks>
        /// In this Case it is the version of the Programm generating this Gedcom file
        /// </remarks>
        /// </summary>
        [GedComTag("VERS")]
        public string Version { get; set; }

        /// <summary>
        /// The name of the software product that produced this transmission.
        /// </summary>
        [GedComTag("NAME")]
        public string Name { get; set; }


        /// <summary>
        /// Name of the business, corporation, or person that produced or commissioned the product
        /// </summary>
        [GedComTag("CORP")]
        public CorporationData Corporation { get; set; }

        /// <summary>
        /// Unknown tag from MyHeritage Family Tree Builder
        /// </summary>
        [GedComTag("_RTLSAVE")]
        public string RTLSAVE { get; set; }
    }
}
