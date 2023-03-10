using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace MaiorumSeries.LaTeXExport
{
    /// <summary>
    /// Class IoHelper for persisting data model to XML and back .
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class IoHelper<T>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        /// <summary>
        /// Writes the specified serialize class.
        /// </summary>
        /// <param name="report">The report to serialize.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Write(T report, string fileName)
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            using (stream)
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, report);
            }
            return true;
        }

        /// <summary>
        /// Reads the specified file name and loads XML as a class structure.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>T.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static T Read(string fileName)
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            T obj = default;
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using (stream)
            {
                var serializer = new XmlSerializer(typeof(T));
                using (XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings() { XmlResolver = null }))
                {
                    obj = (T)serializer.Deserialize(reader);

                }
            }
            return obj;
        }
    }
}
