using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    public class MappingConfig
    {
        public IDictionary<string, Type> KeyWordMapping { get; set; }
        public IDictionary<string, Type> AliasWordMapping { get; set; }

        public bool ContainsKey(string recordTag, string tag)
        {
            if (!string.IsNullOrEmpty(recordTag))
            {
                string alias = recordTag + "." + tag;
                if (AliasWordMapping.ContainsKey(alias))
                {
                    return true;
                }
            }
            if (KeyWordMapping.ContainsKey(tag)) return true;
            return false;
        }


        public Type GetType(string recordTag, string tag)
        {
            if (!string.IsNullOrEmpty (recordTag))
            {
                string alias = recordTag + "." + tag;
                if (AliasWordMapping.ContainsKey(alias))
                {
                    return AliasWordMapping[alias];
                }
            }
            return KeyWordMapping[tag];
        }
    }
}
