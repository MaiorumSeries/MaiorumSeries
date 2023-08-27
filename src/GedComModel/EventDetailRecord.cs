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
    [GedComRecord("BIRT,CHR,DEAT,BURI,CHREM,ADOP,EVEN,BAPM,BASM,BLES,RESI,MARR,DIV,TITL,FACT,OCCU,CONF")]
    public class EventDetailRecord : TypedBaseRecord
    {

        /// <summary>
        /// A unique record identification number assigned to the record by the source system.
        /// This number is intended to serve as a more sure means of identification of a record for reconciling 
        /// differences in data between two interfacing systems.
        /// </summary>
        [GedComTag("RIN")]
        public string RecordId { get; set; }

        [GedComTag("CONC,CONT")]
        public List<NoteLineRecord> Lines { get; set; } = new List<NoteLineRecord>();

        [GedComTag("DATE")]
        public DateRecord Date { get; set; }

        /// <summary>
        /// A jurisdictional name to identify the place or location of an event
        /// </summary>
        [GedComTag("PLAC")]
        public PlaceRecord Place { get; set; }

        /// <summary>
        /// A description of the cause of the associated event or fact, such as the cause of death.
        /// </summary>
        [GedComTag("CAUS")]
        public string Cause { get; set; }

        /// <summary>
        /// A further qualification to the meaning of the associated superior tag. The value does not have any
        /// computer processing reliability.It is more in the form of a short one or two word note that should be
        /// displayed any time the associated data is displayed.
        /// </summary>
        [GedComTag("TYPE")]
        public string Type { get; set; }

        /// <summary>
        /// Additional information provided by the submitter for understanding the enclosing data.
        /// </summary>
        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

        [GedComTag("SOUR")]
        public List<SourceCitationStructure> Sources { get; set; } = new List<SourceCitationStructure>();


        [GedComTag("OBJE")]
        public List<MultimediaRecord> Media { get; set; } = new List<MultimediaRecord>();

        /// <summary>
        /// The address structure should be formed as it would appear on a mailing label using the ADDR and
        /// the CONT lines to form the address structure.The ADDR and CONT lines are required for any
        /// address. The additional subordinate address tags such as STAE and CTRY are provided to be used
        /// by systems that have structured their addresses for indexing and sorting.For backward compatibility
        /// these lines are not to be used in lieu of the required ADDR.and CONT line structure..
        /// </summary>
        [GedComTag("ADDR")]
        public AddressStructure Address { get; set; }

        [GedComTag("_UID")]
        public string UID { get; set; }
    }
}
