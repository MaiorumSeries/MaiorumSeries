using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("PLAC,_PLAC")]
    public class PlaceRecord : TypedBaseRecord
    {
        [GedComTag("MAP")]
        public MappingRecord Mapping { get; set; }

        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();


        [GedComTag("OBJE")]
        public List<MultimediaRecord> Media { get; set; } = new List<MultimediaRecord>();


        /// <summary>
        ///  Standard – GeoCode will insert the standardized place name here when it finds a match, or you select a match.
        /// </summary>
        [GedComTag("STND")]
        public string Standard { get; set; }
       
    }
}
