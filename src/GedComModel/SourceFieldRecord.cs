using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// This is a record, to describe the events in a report. This is a RootsMagic record
    /// </summary>
    [GedComRecord("FIELD")]
    public class SourceFieldRecord : TypedBaseRecord
    {

        [GedComTag("NAME")]
        public string Name{ get; set; }

        [GedComTag("VALUE")]
        public ExtendedTextRecord FieldValue { get; set; }



    }
}
