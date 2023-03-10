using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{

    /// <summary>
    /// This attribute is indicating which GEDCOM Tags are related to a property in the model
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class GedComTagAttribute : Attribute
    {
        public List<string> Tags { get; private set; } = new List<string>();

        public GedComTagAttribute(string tag)
        {
            string[] tags = tag.Split(',');

            foreach (var t in tags)
            {
                Tags.Add(t);
            }
        }
    }
}
