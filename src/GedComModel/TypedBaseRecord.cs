using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    public class TypedBaseRecord
    {
        /// <summary>
        /// The length of the GEDCOM TAG is a maximum of 31 characters, with the first 15 characters being unique
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The cross-reference ID has a maximum of 22 characters, including the enclosing ‘at’ signs (@), 
        /// and it must be unique within the GEDCOM transmission.
        /// </summary>
        public string XrefId { get; set; }

        /// <summary>
        /// This is the value on the record level
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// Container for all the records which are not in the typed model
        /// </summary>
        public BaseRecordContainer Records { get; set; } = new BaseRecordContainer();

       
    }
}
