using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComParser
{
    /// <summary>
    /// Interface for the integration aspects during parsing operations.
    /// </summary>
    public interface IParserContext
    {
        /// <summary>
        /// Define if debug messages dring the GEDCOM parsing shall be issued
        /// </summary>
        bool Debug { get; set; }

        /// <summary>
        /// Define if verbose out shall be performed
        /// </summary>
        bool Verbose { get; set; }

        /// <summary>
        /// Parsed line number of the GedCom File
        /// </summary>
        int Line { get; set; }

        /// <summary>
        /// Number of lines read from the input filel
        /// </summary>
        int ReadLines { get; set; }

        /// <summary>
        /// Number of lines processed by the parser during transformation to typed model
        /// </summary>
        int ProcessedLine { get; set; }

        void WriteWarning(string id, string message, params string[] args);
        void WriteError(string id, string message, params string[] args);

    }
}
