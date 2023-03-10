using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// The name value is formed in the manner the name is normally spoken, with the given name 
    /// and family name (surname) separated by slashes (/). 
    /// </summary>
    [GedComRecord("ADDR")]
    public class AddressStructure : TypedBaseRecord
    {
        
        [GedComTag("CITY")]
        public string City { get; set; }

        [GedComTag("ADR1")]
        public string AddressDetail1 { get; set; }

        /// <summary>
        /// Additional information provided by the submitter for understanding the enclosing data.
        /// </summary>
        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

        [GedComTag("CONC,CONT")]
        public List<NoteLineRecord> Lines { get; set; } = new List<NoteLineRecord>();


        [GedComTag("OBJE")]
        public List<MultimediaRecord> Media { get; set; } = new List<MultimediaRecord>();
    }
}
