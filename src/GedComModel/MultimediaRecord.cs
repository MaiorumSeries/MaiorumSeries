using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("OBJE")]
    public class MultimediaRecord : TypedBaseRecord
    {
        /// <summary>
        /// </summary>
        [GedComTag("FILE")]
        public FileRecord FileName { get; set; }

        [GedComTag("TITL")]
        public string Title { get; set; }

        /// <summary>
        /// MULTIMEDIA_FORMAT:= {Size=3:4}
        /// [bmp | gif | jpg | ole | pcx | tif | wav]
        /// Indicates the format of the multimedia data associated with the specific GEDCOM context.This
        /// allows processors to determine whether they can process the data object. Any linked files should
        /// contain the data required, in the indicated format, to process the file data.
        /// </summary>
        [GedComTag("FORM")]
        public string Format { get; set; }

        /// <summary>
        /// </summary>
        [GedComTag("_PHOTO_RIN")]
        public string PHOTO_RIN { get; set; }

        /// <summary>
        /// Type Tag from Rootsmagic
        /// </summary>
        [GedComTag("_TYPE")]
        public string TYPE { get; set; }

        /// <summary>
        /// Unknown Tag from Rootsmagic
        /// </summary>
        [GedComTag("_SCBK")]
        public string SCBK { get; set; }

        /// <summary>
        /// Primary Tag from Rootsmagic
        /// </summary>
        [GedComTag("_PRIM")]
        public string Primary { get; set; }

        /// <summary>
        /// Type Tag from Rootsmagic
        /// </summary>
        [GedComTag("_DATE")]
        public string DATE { get; set; }

        [GedComTag("CHAN")]
        public ChangeRecord ChangeRecord { get; set; }
    }
}
