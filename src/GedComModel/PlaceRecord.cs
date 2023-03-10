using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("PLAC,_PLAC")]
    public class PlaceRecord : TypedBaseRecord
    {
        [GedComTag("MAP")]
        public MappingRecord Mapping { get; set; }

        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();
    }
}
