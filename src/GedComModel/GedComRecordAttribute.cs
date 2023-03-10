using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// This attribute is indicating if a class is a record in the model related to a property of another class. 
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GedComRecordAttribute : Attribute
    {
        public List<string> Tags { get; private set; } = new List<string>();

        /// <summary>
        /// The tags are not unique in the GEDCOM Description. We need a alias for some records 
        /// to map it to the proper type. These are hanlded in a separate list in the mapping config
        /// </summary>
        public string Alias { get; private set; }

        public GedComRecordAttribute(string tag)
        {
            string[] tags = tag.Split(',');

            foreach (var t in tags)
            {
                if (t.Contains ("."))
                {
                    // This is an alias an no valid tag value
                    if (string.IsNullOrEmpty (Alias))
                    {
                        Alias = t;
                    }
                    else
                    {
                        throw new ApplicationException($"The GedComRecord Attribute with description {tag} does not contain unique alias definition");
                    }
                }
                else
                {
                    Tags.Add(t);
                }
            }
        }
    }
}
