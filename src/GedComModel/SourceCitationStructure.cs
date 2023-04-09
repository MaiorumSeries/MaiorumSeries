using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// The data provided in the <<SOURCE_CITATION>> structure is source-related information specific
    /// to the data being cited. (See GEDCOM examples starting on page 74.) Systems that do not use a
    /// (SOURCE_RECORD) must use the non-preferred second SOURce citation structure option.When
    /// systems that support the zero level source record format encounters a source citation that does not
    /// contain pointers to source records, then that system needs to create a SOURCE_RECORD format
    /// and store the source description information found in the non-structured source citation in the title
    /// area for the new source record.
    /// </summary>
    [GedComRecord("SOUR")]
    public class SourceCitationStructure : TypedBaseRecord
    {
        /// <summary>
        /// </summary>
        [GedComTag("PAGE")]
        public string Page { get; set; }

        /// <summary>
        /// </summary>
        [GedComTag("TITL")]
        public EventDetailRecord Title { get; set; }

        [GedComTag("TEXT")]
        public ExtendedTextRecord  Text { get; set; }

        [GedComTag("_SUBQ")]
        public ExtendedTextRecord SubQuote { get; set; }


        [GedComTag("_BIBL")]
        public ExtendedTextRecord Bibliothec { get; set; }

        [GedComTag("REPO")]
        public string Repository{ get; set; }

        [GedComTag("ABBR")]
        public string Abbreviation { get; set; }

        [GedComTag("PUBL")]
        public string Publication { get; set; }

        /// <summary>
        /// SOURCE_ORIGINATOR:= {Size=1:248}
        /// The person, agency, or entity who created the record.For a published work, this could be the author,
        /// compiler, transcriber, abstractor, or editor.For an unpublished source, this may be an individual, a
        /// government agency, church organization, or private organization, etc.
        /// </summary>
        [GedComTag("AUTH")]
        public string Authority { get; set; }

        /// <summary>
        /// The name property is not part of the GEDCOM 5.5.1 Spec, but written by RootMagic.
        /// </summary>
        [GedComTag("NAME")]
        public string Name { get; set; }

        [GedComTag("OBJE")]
        public List<MultimediaRecord> Media { get; set; } = new List<MultimediaRecord>();

        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

        [GedComTag("CHAN")]
        public ChangeRecord ChangeRecord { get; set; }


        [GedComTag("_TMPLT")]
        public List<SourceTemplateRecord> Templates { get; set; } = new List<SourceTemplateRecord>();

        [GedComTag("_WEBTAG")]
        public List<SourceWebTagRecord> WebTags { get; set; } = new List<SourceWebTagRecord>();
    }
}
