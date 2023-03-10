using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// As a general rule, events are things that happen on a specific date. Use the date form ‘BET date
    /// AND date’ to indicate that an event took place at some time between two dates.Resist the
    /// temptation to use a ‘FROM date TO date’ form in an event structure.If the subject of your
    /// recording occurred over a period of time, then it is probably not an event, but rather an attribute or
    /// fact.
    /// The EVEN tag in this structure is for recording general events that are not shown in the above
    /// <<INDIVIDUAL_EVENT_STRUCTURE>>. The event indicated by this general EVEN tag is
    /// defined by the value of the subordinate TYPE tag.
    /// </summary>
    /// 
    [GedComRecord("TEXT,_SUBQ,_BIBL,VALUE")]
    public class ExtendedTextRecord : TypedBaseRecord
    {
        [GedComTag("CONC,CONT")]
        public List<NoteLineRecord> Lines { get; set; } = new List<NoteLineRecord>();

    }
}
