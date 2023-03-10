using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// This is a record, to describe the events in a report. This is a RootsMagic record
    /// </summary>
    [GedComRecord("_EVDEF")]
    public class EventDefinitionRecord : TypedBaseRecord
    {


      /// <summary>
      /// Type
      /// </summary>
        [GedComTag("TYPE")]
        public string Type { get; set; }

        /// <summary>
        /// Titel
        /// </summary>
        [GedComTag("TITL")]
        public string Titel { get; set; }


        /// <summary>
        /// Abbreviation
        /// </summary>
        [GedComTag("ABBR")]
        public string Abbreviation { get; set; }

        [GedComTag("SENT")]
        public List<EventDefinitionSentRecord> Sents { get; set; } = new List<EventDefinitionSentRecord>();

        [GedComTag("PLAC")]
        public string Place { get; set; }

        [GedComTag("DATE")]
        public string Date { get; set; }

        [GedComTag("DESC")]
        public string Description { get; set; }

        [GedComTag("ROLE")]
        public List<EventDefinitionRole> EventDefinitions { get; set; } = new List<EventDefinitionRole>();

    }
}
