using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// The name value is formed in the manner the name is normally spoken, with the given name 
    /// and family name (surname) separated by slashes (/). 
    /// </summary>
    [GedComRecord("NOTE")]
    public class NoteRecord : TypedBaseRecord
    {
        [GedComTag("CONC,CONT")]
        public List<NoteLineRecord> Lines { get; set; } = new List<NoteLineRecord>();
        
    }
}
