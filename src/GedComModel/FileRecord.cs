using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("FILE")]
    public class FileRecord : TypedBaseRecord
    {
        /// <summary>
        /// [ bmp | gif | jpg | ole | pcx | tif | wav ]
        /// Indicates the format of the multimedia data associated with the specific GEDCOM context.This
        /// allows processors to determine whether they can process the data object. Any
        /// linked files should contain the data required, in the indicated format, to process the file data.
        /// </summary>
        [GedComTag("FORM")]
        public string MultimediaFormat { get; set; }

        /// <summary>
        /// </summary>
        [GedComTag("TITL")]
        public string Title { get; set; }
    }
}
