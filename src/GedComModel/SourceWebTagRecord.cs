using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// This is a record, to describe the events in a report. This is a RootsMagic record
    /// </summary>
    [GedComRecord("_WEBTAG")]
    public class SourceWebTagRecord : TypedBaseRecord
    {

        [GedComTag("URL")]
        public string Url { get; set; }

        [GedComTag("NAME")]
        public string Name { get; set; }

    }
}
