using System;
using System.Collections.Generic;
using System.Reflection;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// Internal Model of the application
    /// </summary>
    public class Model : TypedBaseRecord
    {
        /// <summary>
        /// Get alist of types which have a GedComRecordAttribute attached to it
        /// </summary>
        /// <param name="assembly">The container assembly</param>
        /// <returns></returns>

        private static IEnumerable<Type> GetTypesWithRecordAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(GedComRecordAttribute), true).Length > 0)
                {
                    yield return type; 
                }
            }
        }

        /// <summary>
        /// Get a mapping list of GEDCOM Tags attached to class types for the record attribute
        /// </summary>
        /// <returns>Mapping lsit</returns>
        public static MappingConfig GetRecordMappingConfig ()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var config = new MappingConfig();
            config.KeyWordMapping = new Dictionary<string, Type>();
            config.AliasWordMapping = new Dictionary<string, Type>();

            var inputList = GetTypesWithRecordAttribute(assembly);
            foreach (var type in inputList)
            {
                GedComRecordAttribute attribute = (GedComRecordAttribute) type.GetCustomAttribute(typeof(GedComRecordAttribute));

                if (attribute != null)
                {
                    foreach (var tag in attribute.Tags)
                    {
                        config.KeyWordMapping.Add(tag, type);
                    }
                    if (!string.IsNullOrEmpty(attribute.Alias))
                    {
                        config.AliasWordMapping.Add(attribute.Alias, type);
                    }
                }
            }
            return config;
        }

      

        [GedComTag("HEAD")]
        public HeadData Head { get; set; }

        [GedComTag("INDI")]

        public List<IndividualRecord> Individuals { get; set; } = new List<IndividualRecord>();

        [GedComTag("FAM")]
        public List<FamilyRecord> Families { get; set; } = new List<FamilyRecord>();

        [GedComTag("OBJE")]
        public List<MultimediaRecord> Media { get; set; } = new List<MultimediaRecord>();

        [GedComTag("NOTE")]
        public List<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

        [GedComTag("_PLAC")]
        public List<PlaceRecord> Places { get; set; } = new List<PlaceRecord>();

        [GedComTag("SOUR")]
        public List<SourceCitationStructure> Sources { get; set; } = new List<SourceCitationStructure>();

        /// <summary>
        /// The submitter record identifies an individual or organization that contributed information contained
        /// in the GEDCOM transmission.All records in the transmission are assumed to be submitted by the
        /// SUBMITTER referenced in the HEADer, unless a SUBMitter reference inside a specific record
        /// points at a different SUBMITTER record.
        /// </summary>
        [GedComTag("SUBM")]
        public List<SubmitterRecord> Submitter { get; set; } = new List<SubmitterRecord>();


        [GedComTag("_EVDEF")]
        public List<EventDefinitionRecord> EventDefinitions { get; set; } = new List<EventDefinitionRecord>();

        [GedComTag("REPO")]
        public List<RepositoryRecord> Repositories { get; set; } = new List<RepositoryRecord>();


        
        [GedComTag("TRLR")]
        public string Trailer { get; set; }
    }
}
