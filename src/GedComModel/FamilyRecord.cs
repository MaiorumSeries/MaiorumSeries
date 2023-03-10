using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("FAM")]
    public class FamilyRecord : TypedBaseRecord
    {

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

       

        //[GedComTag("NAME")]
        //public List<NameStructureData> Names { get; set; } = new List<NameStructureData>();

        [GedComTag("MARR,EVEN,DIV,FACT,RESI")]
        public List<EventDetailRecord> Events { get; set; } = new List<EventDetailRecord>();

        [GedComTag("OBJE")]
        public List<MultimediaRecord> Media { get; set; } = new List<MultimediaRecord>();

        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

        [GedComTag("HUSB")]
        public HusbandToIndividualLinkRecord Husband { get; set; }

        [GedComTag("WIFE")]
        public WifeToIndividualLinkRecord Wife { get; set; }

        [GedComTag("CHIL")]
        public List<ChildToIndividualLinkRecord> Children { get; set; } = new List<ChildToIndividualLinkRecord>();

        [GedComTag("SOUR")]
        public List<SourceCitationStructure> Sources { get; set; } = new List<SourceCitationStructure>();

        [GedComTag("CHAN")]
        public ChangeRecord ChangeRecord { get; set; }


        [GedComTag("_UID")]
        public string UID { get; set; }

        [GedComTag("_UPD")]
        public string _UPD { get; set; }
    }
}
