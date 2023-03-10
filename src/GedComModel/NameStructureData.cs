using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// The name value is formed in the manner the name is normally spoken, with the given name 
    /// and family name (surname) separated by slashes (/). 
    /// </summary>
    [GedComRecord("NAME")]
    public class NameStructureData : TypedBaseRecord
    {
        /// <summary>
        ///  Based on the dynamic nature or unknown compositions of naming conventions, it is difficult to provide 
        ///  more detailed name piece structure to handle every case. The NPFX, GIVN, NICK, SPFX, SURN, and NSFX tags 
        ///  are provided optionally for systems that cannot operate effectively with less structured information. 
        ///  For current future compatibility, all systems must construct their names based on the <NAME_PERSONAL> structure. 
        ///  Those using the optional name pieces should assume that few systems will process them, and most will not provide
        ///  the name pieces.
        /// </summary>
        [GedComTag("GIVN")]
        public string GivenName { get; set; }

        /// <summary>
        ///  Based on the dynamic nature or unknown compositions of naming conventions, it is difficult to provide 
        ///  more detailed name piece structure to handle every case. The NPFX, GIVN, NICK, SPFX, SURN, and NSFX tags 
        ///  are provided optionally for systems that cannot operate effectively with less structured information. 
        ///  For current future compatibility, all systems must construct their names based on the <NAME_PERSONAL> structure. 
        ///  Those using the optional name pieces should assume that few systems will process them, and most will not provide
        ///  the name pieces.
        /// </summary>
        [GedComTag("SURN")]
        public string SurName { get; set; }

        [GedComTag("NICK")]
        public string NickName { get; set; }

        /// <summary>
        ///  The NPFX, GIVN, NICK, SPFX, SURN, and
        /// NSFX tags are provided optionally for systems that cannot operate effectively with less structured information.
        /// </summary>
        [GedComTag("NSFX")]
        public string NamePieceSuffix { get; set; }

        [GedComTag("NPFX")]
        public string NameaPieceSuffix { get; set; }

        [GedComTag("SOUR")]
        public List<SourceCitationStructure> Sources { get; set; } = new List<SourceCitationStructure>();
    }
}
