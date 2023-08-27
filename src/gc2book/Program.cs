using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MaiorumSeries.GedComParser;
using MaiorumSeries.LaTeXExport;
using MaiorumSeries.GedComLogic;

namespace gc2book
{
    class Program
    {

        internal class ConsoleParserContext : IParserContext
        {
            public bool Debug { get; set; }
            public bool Verbose { get; set; }
            public int Line { get; set; }
            public int ReadLines { get; set; }
            public int ProcessedLine { get; set; }

            public void WriteError(string id, string message, params string[] args)
            {
                Console.WriteLine("ERR (" + id + "): " + string.Format(message, args));
            }

            public void WriteWarning(string id, string message, params string[] args)
            {
                Console.WriteLine("WRN (" +id + "): " + string.Format(message, args));
            }
        }

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

            public void VerboseMessage(string message)
            {
                Console.WriteLine(string.Format(message));
            }

            public void Error(string message)
            {
                Console.WriteLine(string.Format(message));
            }

            public void WriteLine(string message, params string[] args)
            {
                Console.WriteLine(string.Format(message, args));
            }



            public ConsoleLaTeXExportContext()
            {
#if DEBUG
                WriteSources = true;
                WriteTribe = false;
                CopySources = false;
#endif
            }
        }


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

        static int Main(string[] args)
        {
            int exitCode = 0;
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                  .WithParsed<Options>(o =>
                  {
                      var parser = new MaiorumSeries.GedComParser.Parser();
                      var parserContext = new ConsoleParserContext();

                      parserContext.Verbose = o.Verbose;
                      parserContext.Debug = o.Debug;

                      if (string.IsNullOrEmpty (o.InputFileName))
                      {
                          Console.WriteLine("InputFileName is not set to a value");
                          exitCode = 2;
                          return;
                      }
                      string fileName = o.InputFileName;

                      var model = parser.Parse(parserContext, fileName);

                      ConsoleLaTeXExportContext laTeXExportContext = new ConsoleLaTeXExportContext();
//                      laTeXExportContext.Culture = new CultureInfo("de-DE");
                      laTeXExportContext.Culture = new CultureInfo(o.Culture);
                      laTeXExportContext.OutputName = o.OutputName;
                      laTeXExportContext.OutputPath = o.OutputPath;
                      //                      laTeXExportContext.Culture = model.GetCultureInfo();
                      laTeXExportContext.WriteSources = o.WriteSources;
                      laTeXExportContext.CopySources = o.CopySources;
                      laTeXExportContext.WriteTribe = o.WriteTribe;


                      string forPersonId = model.FindBestNameMatch(o.Name);

                      BookMetaInformation bookMetaInformation = new BookMetaInformation()
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
