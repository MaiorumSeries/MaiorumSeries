using CommandLine;
using MaiorumSeries.GedComLogic;
using MaiorumSeries.GedComModel;
using MaiorumSeries.GedComParser;
using MaiorumSeries.LaTeXExport;
using System.Globalization;

namespace gc2book
{
    /// <summary>
    /// Main program class for the gc2book application.
    /// Handles command-line parsing, GEDCOM file processing, and LaTeX export.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Implementation of IParserContext for console-based error and warning output.
        /// Tracks parsing state and provides debug/verbose output.
        /// </summary>
        internal class ConsoleParserContext : IParserContext
        {
            public bool Debug { get; set; }
            public bool Verbose { get; set; }
            public int Line { get; set; }
            public int ReadLines { get; set; }
            public int ProcessedLine { get; set; }

            /// <summary>
            /// Writes an error message to the console.
            /// </summary>
            /// <param name="id">Error identifier.</param>
            /// <param name="message">Error message format string.</param>
            /// <param name="args">Arguments for the format string.</param>
            public void WriteError(string id, string message, params string[] args)
            {
                Console.WriteLine("ERR (" + id + "): " + string.Format(message, args));
            }

            /// <summary>
            /// Writes a warning message to the console.
            /// </summary>
            /// <param name="id">Warning identifier.</param>
            /// <param name="message">Warning message format string.</param>
            /// <param name="args">Arguments for the format string.</param>
            public void WriteWarning(string id, string message, params string[] args)
            {
                Console.WriteLine("WRN (" + id + "): " + string.Format(message, args));
            }
        }

        /// <summary>
        /// Implementation of ILaTeXExportContext for console-based LaTeX export configuration and output.
        /// </summary>
        internal class ConsoleLaTeXExportContext : ILaTeXExportContext
        {
            public string? OutputPath { get; set; }
            public string? OutputName { get; set; }
            public CultureInfo Culture { get; set; } = new CultureInfo("en-US");
            public List<string> WrittenFamilies { get; set; } = new List<string>();
            public List<string> WrittenIndividuals { get; set; } = new List<string>();
            public bool WriteSources { get; set; } = false;
            public bool WriteTribe { get; set; } = false;
            public bool CopySources { get; set; } = false;

            /// <summary>
            /// Writes a verbose message to the console.
            /// </summary>
            /// <param name="message">The message to write.</param>
            public void VerboseMessage(string message)
            {
                Console.WriteLine(string.Format(message));
            }

            /// <summary>
            /// Writes an error message to the console.
            /// </summary>
            /// <param name="message">The error message.</param>
            public void Error(string message)
            {
                Console.WriteLine(string.Format(message));
            }

            public void Error(IndividualRecord individual, string message)
            {
                Console.WriteLine(string.Format("Error for individual {0}: {1}", individual.GetDisplayName(Culture), message));
            }

            /// <summary>
            /// Writes a formatted line to the console.
            /// </summary>
            /// <param name="message">The message format string.</param>
            /// <param name="args">Arguments for the format string.</param>
            public void WriteLine(string message, params string[] args)
            {
                Console.WriteLine(string.Format(message, args));
            }

            /// <summary>
            /// Initializes a new instance of the ConsoleLaTeXExportContext class.
            /// Sets default values for export options in debug mode.
            /// </summary>
            public ConsoleLaTeXExportContext()
            {
#if DEBUG
                WriteSources = true;
                WriteTribe = false;
                CopySources = false;
#endif
            }
        }

        /// <summary>
        /// Command-line options for the gc2book application.
        /// </summary>
        public class Options
        {
            [Option('d', "debug", Required = false, HelpText = "Set processing GEDCOM messages to debug.")]
            public bool Debug { get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }

            [Option('p', "outputpath", Required = false, Default = "c:\\work\\books", HelpText = "Set the folder for the output folder.")]
            public string OutputPath { get; set; } = "c:\\work\\books";

            [Option('o', "outputname", Required = false, Default = "Output", HelpText = "Set the name for the output files.")]
            public string OutputName { get; set; } = "Output";

            [Option('n', "name", Required = true, Default = "John Doe", HelpText = "Set the name of the main person.")]
            public string Name { get; set; } = "John Doe";

            [Option('l', "language", Required = false, Default = "en-US", HelpText = "Set the language culture of output.")]
            public string Culture { get; set; } = "en-US";

            [Option('g', "gedcomfile", Required = false, HelpText = "Set gedcom input file name .")]
            public string? InputFileName { get; set; }

            [Option('s', "sources", Required = false, Default = false, HelpText = "Set the flag if source information will be written for individuals.")]
            public bool WriteSources { get; set; }

            [Option('c', "copysources", Required = false, Default = false, HelpText = "Set the flag if source information will be copied to output folder.")]
            public bool CopySources { get; set; }

            [Option('t', "tribe", Required = false, Default = false, HelpText = "Set the flag if tribe chapters will be written for individuals.")]
            public bool WriteTribe { get; set; }
        }

        /// <summary>
        /// Main entry point for the application.
        /// Parses command-line arguments, processes the GEDCOM file, and exports to LaTeX.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>Exit code (0 for success, non-zero for error).</returns>
        static int Main(string[] args)
        {
            int exitCode = 0;
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                  .WithParsed<Options>(o =>
                  {
                      var parser = new MaiorumSeries.GedComParser.Parser();
                      var parserContext = new ConsoleParserContext
                      {
                          Verbose = o.Verbose,
                          Debug = o.Debug
                      };

                      if (string.IsNullOrEmpty(o.InputFileName))
                      {
                          Console.WriteLine("InputFileName is not set to a value");
                          exitCode = 2;
                          return;
                      }
                      string fileName = o.InputFileName;

                      var model = parser.Parse(parserContext, fileName);

                      var laTeXExportContext = new ConsoleLaTeXExportContext
                      {
                          Culture = new CultureInfo(o.Culture),
                          OutputName = o.OutputName,
                          OutputPath = o.OutputPath,
                          WriteSources = o.WriteSources,
                          CopySources = o.CopySources,
                          WriteTribe = o.WriteTribe
                      };

                      string forPersonId = model.FindBestNameMatch(o.Name);

                      var bookMetaInformation = new BookMetaInformation()
                      {
                          Author = "Author Name",
                          Title = "Title of the Book",
                          Subject = "Subject or subtitle of the book",
                          Keywords = "Some key words for the PDF"
                      };

                      MaiorumSeries.LaTeXExport.LaTeXOutputLogic.WriteLaTexBook(laTeXExportContext, model, bookMetaInformation, forPersonId);

                      Console.WriteLine("Processed lines " + parserContext.ProcessedLine + "/" + parserContext.ReadLines);
                  });
            return exitCode;
        }
    }

}
