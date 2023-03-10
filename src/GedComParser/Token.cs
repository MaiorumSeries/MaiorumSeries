using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComParser
{
    public class Token
    {
        /// <summary>
        /// The beginning of a new logical record is designated by a line whose level number is 0 (zero).
        /// Level numbers  must be between 0 to 99 and must not contain leading zeroes, for example, level one must be 1, not 01.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The cross-reference ID has a maximum of 22 characters, including the enclosing ‘at’ signs (@), 
        /// and it must be unique within the GEDCOM transmission.
        /// </summary>
        public string XrefId { get; set; }

        /// <summary>
        /// The length of the GEDCOM TAG is a maximum of 31 characters, with the first 15 characters being unique
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The total length of a GEDCOM line, including level number, cross-reference number, tag, value, 
        /// delimiters, and terminator, must not exceed 255 (wide) characters
        /// </summary>
        public string Value { get; set; }


        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append(Level.ToString());
            if (!string.IsNullOrEmpty (XrefId))
            {
                buffer.Append(" ");
                buffer.Append(XrefId);
            }
            buffer.Append(" ");
            buffer.Append(Tag);
            if (!string.IsNullOrEmpty(Value))
            {
                buffer.Append(" ");
                buffer.Append(Value);
            }

            return buffer.ToString();
        }

        //public TagedValue GetTagedValue ()
        //{
        //    var tagedValue = new TagedValue()
        //    {
        //        Tag = Tag,
        //        Value = Value
        //    };
        //    return tagedValue; 
        //}
    }
}
