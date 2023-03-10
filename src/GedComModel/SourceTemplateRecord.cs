using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// This is a record, to describe the events in a report. This is a RootsMagic record
    /// </summary>
    [GedComRecord("_TMPLT")]
    public class SourceTemplateRecord : TypedBaseRecord
    {

        [GedComTag("TID")]
        public string TID { get; set; }


        [GedComTag("FIELD")]
        public List<SourceFieldRecord> Sents { get; set; } = new List<SourceFieldRecord>();

    }
}
