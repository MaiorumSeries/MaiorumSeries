using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// The header structure provides information about the entire transmission. The SOURce system name identifies 
    /// which system sent the data. The DESTination system name identifies the intended receiving system.
    /// Additional GEDCOM standards will be produced in the future to reflect GEDCOM expansion and maturity.
    /// This requires the reading program to make sure it can read the GEDC.VERS and the GEDC.FORM values to insure
    /// proper readability. The CHAR tag is required.All character codes greater than 0x7F must be converted to ANSEL
    /// </summary>
    [GedComRecord("HEAD")]
    public class HeadData : TypedBaseRecord
    {

        /// <summary>
        /// The human language in which the data in the transmission is normally read or written.  
        /// It is used primarily by programs to select language-specific sorting sequences and phonetic 
        /// name matching algorithms.
        /// A table of valid latin language identification codes. [ Afrikaans | Albanian | Anglo-Saxon | Catalan | Catalan_Spn 
        /// | Czech | Danish | Dutch | English | Esperanto | Estonian | Faroese | Finnish | French | German | Hawaiian 
        /// | Hungarian | Icelandic | Indonesian | Italian | Latvian | Lithuanian | Navaho | Norwegian | Polish | Portuguese 
        /// | Romanian | Serbo_Croa | Slovak | Slovene | Spanish | Swedish | Turkish | Wendic ]
        /// Other languages not supported until UNICODE
        /// [Amharic | Arabic | Armenian | Assamese | Belorusian | Bengali | Braj | Bulgarian | Burmese | Cantonese 
        /// | Church-Slavic | Dogri | Georgian | Greek | Gujarati | Hebrew | Hindi | Japanese | Kannada | Khmer | Konkani 
        /// | Korean | Lahnda | Lao | Macedonian | Maithili | Malayalam | Mandrin | Manipuri | Marathi | Mewari | Nepali 
        /// | Oriya | Pahari | Pali | Panjabi | Persian | Prakrit | Pusto | Rajasthani | Russian | Sanskrit | Serb | Tagalog 
        /// | Tamil | Telugu | Thai | Tibetan | Ukrainian | Urdu | Vietnamese | Yiddish]
        /// </summary>
        [GedComTag("LANG")]
        public string Language { get; set; }

        /// <summary>
        /// [ ANSEL |UTF-8 | UNICODE | ASCII ] 
        /// A code value that represents the character set to be used to interpret this data. Currently, 
        /// the preferred character set is ANSEL, which includes ASCII as a subset. UNICODE is not widely supported 
        /// by most operating systems; therefore, GEDCOM produced using the UNICODE character set will be limited in 
        /// its interchangeability for a while but should eventually provide the international flexibility that is desired.
        /// </summary>
        [GedComTag("CHAR")]
        public string CharSet { get; set; }


        /// <summary>
        /// The name of the system expected to process the GEDCOM-compatible transmission. 
        /// The registered RECEIVING_SYSTEM_NAME for all GEDCOM submissions to the Family History Department must be 
        /// one of the following names:
        /// "ANSTFILE" when submitting to Ancestral File. 
        /// "TempleReady" when submitting for temple ordinance clearance.
        /// </summary>
        [GedComTag("DEST")]
        public string Destination { get; set; }

        /// <summary>
        /// The name of the GEDCOM transmission file. If the file name includes a file extension it must be
        /// shown in the form(filename.ext)
        /// </summary>
        [GedComTag("FILE")]
        public string FileName { get; set; }

        [GedComTag("DATE")]
        public DateRecord Date { get; set; }

        /// <summary>
        /// Record describing the GedCom file format and versions
        /// </summary>
        [GedComTag("GEDC")]
        public GedComMetaData GedComVersion { get; set; }

        /// <summary>
        /// A system identification name which was obtained through the GEDCOM registration process. This name must 
        /// be unique from any other product. Spaces within the name must be substituted with a 0x5F (underscore _) 
        /// so as to create one word
        /// </summary>
        [GedComTag("HEAD.SOUR")]
        public GedComSourceData GedComSource { get; set; }


        /// <summary>
        /// The submitter record identifies an individual or organization that contributed information contained
        /// in the GEDCOM transmission.All records in the transmission are assumed to be submitted by the
        /// SUBMITTER referenced in the HEADer, unless a SUBMitter reference inside a specific record
        /// points at a different SUBMITTER record.
        /// TODO Check if this a only a reference
        /// </summary>
       [GedComTag("SUBM")]
        public string Submitter { get; set; }

        /// <summary>
        /// A copyright statement needed to protect the copyrights of the submitter of this GEDCOM file
        /// </summary>
        [GedComTag("COPR")]
        public string CopyrightGedcomFile { get; set; }

        [GedComTag("_RINS")]
        public string RINS { get; set; }

        [GedComTag("_UID")]
        public string UID { get; set; }
        [GedComTag("_DESCRIPTION_AWARE")]
        public string DESCRIPTION_AWARE { get; set; }
        [GedComTag("_FACTS_DEFRAGGED")]
        public string FACTS_DEFRAGGED { get; set; }
    }
}
