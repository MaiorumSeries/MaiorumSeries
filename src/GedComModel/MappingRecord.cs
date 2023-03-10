using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("MAP")]
    public class MappingRecord : TypedBaseRecord
    {
        /// <summary>
        /// The value specifying the latitudinal coordinate of the place name. The latitude coordinate is the
        /// direction North or South from the equator in degrees and fraction of degrees carried out to give the
        /// desired accuracy.For example: 18 degrees, 9 minutes, and 3.4 seconds North would be formatted as
        /// N18.150944. Minutes and seconds are converted by dividing the minutes value by 60 and the seconds
        /// value by 3600 and adding the results together.This sum becomes the fractional part of the degree’s
        /// value.
        /// </summary>
        [GedComTag("LATI")]
        public string Latitude { get; set; }

        /// The value specifying the longitudinal coordinate of the place name.The longitude coordinate is
        /// Degrees and fraction of degrees east or west of the zero or base meridian coordinate. For example:
        /// 168 degrees, 9 minutes, and 3.4 seconds East would be formatted as E168.150944
        [GedComTag("LONG")]
        public string Longitude { get; set; }

        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

    }
}
