using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("INDI")]
    public class IndividualRecord : TypedBaseRecord
    {
        /// <summary>
        /// This is a flag indicating if this individual is a direct ancestor for a given individual. 
        /// This is used to create the book structure and will be calculated during processing.
        /// </summary>
        public bool DirectAncestor { get; set; } = false;

        /// <summary>
        /// A unique record identification number assigned to the record by the source system.
        /// This number is intended to serve as a more sure means of identification of a record for reconciling 
        /// differences in data between two interfacing systems.
        /// </summary>
        [GedComTag("RIN")]
        public string RecordId { get; set; }


        /// <summary>
        /// [confidential | locked | privacy ] 
        /// The restriction notice is defined for Ancestral File usage.  Ancestral File download GEDCOM files may 
        /// contain this data. Where: confidential = This data was marked as confidential by the user.  
        /// In some systems data marked as confidential will be treated differently, for example, there might be 
        /// an option that would stop confidential data from appearing on printed reports or would prevent that 
        /// information from being exported.
        /// locked = Some records in Ancestral File have been satisfactorily proven by evidence, but because 
        /// of source conflicts or incorrect traditions, there are repeated attempts to change this record.
        /// By arrangement, the Ancestral File Custodian can lock a record so that it cannot be changed without 
        /// an agreement from the person assigned as the steward of such a record.The assigned steward is either 
        /// the submitter listed for the record or Family History Support when no submitter is listed.
        /// privacy = Indicate that information concerning this record is not present due to rights of or an 
        /// approved request for privacy.For example, data from requested downloads of the
        /// Ancestral File may have individuals marked with ‘privacy’ if they are assumed living, 
        /// that is they were born within the last 110 years and there isn’t a death date.  
        /// In certain cases family records may also be marked with the RESN tag of privacy if either 
        /// individual acting in the role of HUSB or WIFE is assumed living.
        /// </summary>
        [GedComTag("RESN")]
        public string RestrictionNote { get; set; }

        /// <summary>
        /// A code that indicates the sex of the individual: 
        /// M = Male 
        /// F = Female 
        /// U = Undetermined from available records and quite sure that it can’t be.
        /// </summary>
        [GedComTag("SEX")]
        public string Sex { get; set; }

        [GedComTag("NAME")]
        public List<NameStructureData> Names { get; set; } = new List<NameStructureData>();

        [GedComTag("BIRT,CHR,DEAT,BURI,CHREM,ADOP,EVEN,BAPM,BARM,BASM,BLES,RESI,TITL,FACT,OCCU,CONF")]
        public List<EventDetailRecord> Events { get; set; } = new List<EventDetailRecord>();


        [GedComTag("OBJE")]
        public List<MultimediaRecord> Media { get; set; } = new List<MultimediaRecord>();

        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

        [GedComTag("FAMS")]
        public List<SpouseToFamilyLinkRecord> SpouseTo { get; set; } = new List<SpouseToFamilyLinkRecord>();

        [GedComTag("FAMC")]
        public ChildToFamilyLinkRecord ChildFrom { get; set; }

        [GedComTag("SOUR")]
        public List<SourceCitationStructure> Sources { get; set; } = new List<SourceCitationStructure>();

        [GedComTag("WWW")]
        public List<string> HomePages { get; set; } = new List<string>();

        /// <summary>
        /// ID Tag from RootsMagic
        /// </summary>
        [GedComTag("_AMTID")]
        public string AMTID { get; set; }

        [GedComTag("_UID")]
        public string UID { get; set; }

        [GedComTag("_UPD")]
        public string UPD { get; set; }

        /// <summary>
        /// ID Tag from Family Search Tree ID
        /// </summary>
        [GedComTag("_FSFTID")]
        public string FSFTID { get; set; }

        [GedComTag("CHAN")]
        public ChangeRecord ChangeRecord { get; set; }

    }
}
