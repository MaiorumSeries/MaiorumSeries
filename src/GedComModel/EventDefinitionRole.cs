using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// This is a record, to describe the events in a report. This is a RootsMagic record
    /// </summary>
    [GedComRecord("ROLE")]
    public class EventDefinitionRole : TypedBaseRecord
    {




        [GedComTag("SENT")]
        public List<EventDefinitionSentRecord> Sents { get; set; } = new List<EventDefinitionSentRecord>();

    }
}
