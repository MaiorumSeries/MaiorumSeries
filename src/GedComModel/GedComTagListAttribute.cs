using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{

    /// <summary>
    /// This attribute is indicating which GEDCOM Tag is related to a property in the model
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class GedComTagListAttribute : Attribute
    {
        public string TagValue { get; private set; }

        public GedComTagListAttribute(string tag)
        {
            TagValue = tag;
        }
    }
}
