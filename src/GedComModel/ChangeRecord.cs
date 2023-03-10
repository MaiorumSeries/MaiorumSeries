using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{

    /// <summary>
    /// The change date is intended to only record the last change to a record. Some systems may want to
    ///  manage the change process with more detail,
    /// </summary>
    [GedComRecord("CHAN")]
    public class ChangeRecord : TypedBaseRecord
    {
        [GedComTag("DATE")]
        public DateRecord Date { get; set; }

        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();
    }
}
