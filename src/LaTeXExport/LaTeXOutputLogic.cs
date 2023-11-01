// ***********************************************************************
// <copyright file="LaTeXOutputLogic.cs" company="Cord Burmeister">
//     Copyright © Cord Burmeister 2016
// </copyright>
// <summary></summary>
// ***********************************************************************


using MaiorumSeries.GedComModel;
using MaiorumSeries.LaTeXExport;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Diagnostics;
using MaiorumSeries.GedComLogic;
using System.Globalization;

namespace MaiorumSeries.LaTeXExport
{

    /// <summary>
    /// Class LaTeXOutputLogic conaining all the logic to generate a book in latex format from the GEDCOM Model .
    /// </summary>
    public static class LaTeXOutputLogic
    {

        private static readonly CultureInfo germanCultureInfo = new CultureInfo("de-DE");

        private static string CalculateOutputPath(ILaTeXExportContext context, string fileName)
        {
            if (!Directory.Exists(context.OutputPath))
            {
                Directory.CreateDirectory(context.OutputPath);
            }

            var path = Path.Combine(context.OutputPath, fileName);
            return path;
        }

        /// <summary>
        /// Map the eventtype to a symbol based on the documentation udner http://wiki-de.genealogy.net/Genealogische_Symbole_und_Zeichen
        /// See also http://detexify.kirelabs.org/classify.html for finding Latex symbols.
        /// See also a comprehensive list of LaTeX symbol sources under http://mirrors.ibiblio.org/CTAN/info/symbols/comprehensive/symbols-a4.pdf
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private static string MapEventTypeToSymbol(string tag)
        {
            switch (tag)
            {
                case "BIRT":
                    return @"$\star$ \  ";
                case "CHR":
                    return @"$\approx$ \  ";
                case "DEAT":
                    return @"$\dagger$ \  ";
                case "BURI":
                    return @"$\square$ \  ";
                case "CHREM":
                    return @"$\sqcup$ \  ";
                case "ADOP":
                    return @"$\wedge$ \  ";
                case "EVEN":
                    return @"$\bullet \  ";
                case "BAPM":
                    return @"$\approx$ \  ";
                case "BLES":
                    return @"$\approx$ \  ";
                case "RESI":
                    return @"$\odot$ \  ";
                case "MARR":
                    return @"$\infty$ \  ";
                case "DIV":
                    return @"$\circ | \circ$ \  ";
                case "TITL":
                    return @"$ > $ \  ";
                case "FACT":
                    return @"$\otimes$ \  ";
                case "OCCU":
                    return @"$\oplus$ \  ";
                case "CONF":
                    return @"$\times \  ";

            }
            return "?";
        }

        private static void WriteTimeLineHeader(ILaTeXExportContext context, LaTexWriter writer)
        {
            writer.RawOutput(@"\begin{longtable}{@{\,}r <{\hskip 2pt} !{\foo} >{\raggedright\arraybackslash}p{11cm}}");
            writer.RawOutput(@" %\caption{Timeline} \\[-1.5ex]");
            writer.RawOutput(@"\toprule");
            writer.RawOutput(@"\addlinespace[1.5ex]");
            writer.RawOutput(@"\multicolumn{1}{c!{\tfoo}}{}& \\[-2.3ex]");
            writer.RawOutput(@"%\renewcommand\arraystretch{1.4}\arrayrulecolor{ LightSteelBlue3}");
            writer.RawOutput(@"%\captionsetup{font=blue,labelfont=sc,labelsep=quad}");
        }

        private static void WriteTimeLineFooter(ILaTeXExportContext context, LaTexWriter writer)
        {
            writer.RawOutput(@"\multicolumn{1}{c!{\bfoo}}{}&");
            writer.RawOutput(@"\end{longtable}");
        }

        private static void WriteTimeLineItem(ILaTeXExportContext context, LaTexWriter writer, EventItem eventItem)
        {
            writer.RawOutput(eventItem.DateString + @" & " + eventItem.Description + @"\\");
        }


        private static bool HasTimeHeaderImage(ILaTeXExportContext context, IndividualRecord individualRecord)
        {
            var mr = individualRecord.Media.FirstOrDefault<MultimediaRecord>();

            return (mr != null);
        }


        /// <summary>
        /// Write and header Image to the document, if approbiate image in available in this indiviudal record
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <param name="individualRecord"></param>
        private static void WriteTimeHeaderImage(ILaTeXExportContext context, LaTexWriter writer, Model model, IndividualRecord individualRecord)
        {
            var mr = individualRecord.Media.FirstOrDefault<MultimediaRecord>();

            if (mr != null)
            {
                // Check if this is a refernce 
                if (!string.IsNullOrEmpty(mr.Value))
                {
                    mr = model.Media.Find(x => x.XrefId == mr.Value);
                }

                if (mr != null)
                {
                    var path = mr.FileName.Value;

                    path = EnsureTeXableImage(path);

                    if (!string.IsNullOrEmpty(path))
                    {
                        if (File.Exists(path))
                        {
                            path = path.Replace(@"\", "/");
                            writer.RawOutput(@"\begin{wrapfigure}{r}{0.2\textwidth}");
                            writer.RawOutput(@"\begin{center}");
                            writer.RawOutput(@"\vspace{-90pt}");
                            //writer.RawOutput(@"\includegraphics[width=0.25\textwidth]{C:/Users/CordB/OneDrive/Chronik/Bilder/Burmeister/Cord/Cord.jpg}");
                            writer.RawOutput(@"\includegraphics[width=0.2\textwidth]{" + path + "}");
                            writer.RawOutput(@"\end{center}");
                            writer.RawOutput(@"\end{wrapfigure}");
                        }
                        else
                        {
                            context.Error(@"The Media record has a path reference which can not be found :" + path);
                        }
                    }
                }
            }
        }

        private static void WriteIndividualWhenNotWritten(ILaTeXExportContext context, OutputContext outputContext, Model model, LaTexWriter writer, IndividualRecord individualRecord)
        {
            if (!context.WrittenIndividuals.Contains(individualRecord.XrefId))
            {
                WriteIndividual(context, outputContext, model, writer, individualRecord);
            }
        }

        /// <summary>
        /// This is the logic to write the personal data for one Individual. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="writer"></param>
        /// <param name="individualRecord"></param>
        private static void WriteIndividual(ILaTeXExportContext context, OutputContext outputContext, Model model, LaTexWriter writer, IndividualRecord individualRecord)
        {
            // Write the heading 
            writer.Header3(writer.ToTeX(individualRecord.GetDisplayName(context.Culture)));
            writer.Label(individualRecord.XrefId);

            // Remember, that this indiviual has been written
            if (!context.WrittenIndividuals.Contains(individualRecord.XrefId))
            {
                context.WrittenIndividuals.Add(individualRecord.XrefId);
            }

            // Put all name variations into the index
            var list = individualRecord.GetNameIndexList();
            foreach (var s in list)
            {
                writer.RawOutput(@"\index[persons]{" + s + "}\n");
            }

            // Get a short summary of the individual
            string summary = individualRecord.GetSummary(model, context.Culture);
            if (HasTimeHeaderImage(context, individualRecord)
                || !string.IsNullOrEmpty(summary))
            {
                // Write summary header
                writer.Header4(Strings.ResourceManager.GetString("Summary", context.Culture));

                // Show an image when available
                WriteTimeHeaderImage(context, writer, model, individualRecord);

                // Write summary
                writer.Text(summary);
            }

            #region Now check if there are enough events for the time line chapter
            // Now check if there are enough events for the time line chapter
            var eventList = individualRecord.GetEventList(model, context.Culture);
            if (eventList.Count > 6)
            {
                writer.Header4(Strings.ResourceManager.GetString("PersonalEvents", context.Culture));
                WriteTimeLineHeader(context, writer);
                foreach (var e in eventList)
                {
                    WriteTimeLineItem(context, writer, e);
                }
                WriteTimeLineFooter(context, writer);
            }
            #endregion 

            #region Write notes chapter when there are some
            // Write notes chapter when there are some
            if (individualRecord.Notes.Count > 0)
            {
                writer.Header4(Strings.ResourceManager.GetString("History", context.Culture));
                foreach (var n in individualRecord.Notes)
                {
                    var tl = (n.GetNoteTextList(model, context.Culture));
                    foreach (var tn in tl)
                    {
                        writer.Text(tn);
                    }
                }
            }
            #endregion 

            #region Show picture, when there is more than one picture
            // Show picture, when there is more than one picture
            if (individualRecord.Media.Count > 1)
            {
                // Images are floating away writer.Header4(Strings.ResourceManager.GetString("Media", context.Culture));

                writer.RawOutput(@"\begin{figure}[!ht]");
                writer.RawOutput(@"\centering");
                MultimediaRecord mmr = null;
                bool pendingHeader = false;
                bool pendingFooter = true;
                int imageCount = 0;
                int sectionCount = 1;
                foreach (var mr in individualRecord.Media)
                {
                    if (mmr != null)
                    {
                        mmr = mr;
                        // Check if this is a refernce 
                        if (!string.IsNullOrEmpty(mmr.Value))
                        {
                            mmr = model.Media.Find(x => x.XrefId == mmr.Value);
                        }

                        if (mmr != null)
                        {
                            var path = mmr.FileName.Value;
                            string title = individualRecord.GetDisplayName(context.Culture);
                            if (!string.IsNullOrEmpty(mmr.Title))
                            {
                                title = mmr.Title;
                                title = title.Replace("_", " ");
                            }
                            if (!string.IsNullOrEmpty(path))
                            {
                                path = EnsureTeXableImage(path);

                                if (File.Exists(path))
                                {
                                    if (pendingHeader)
                                    {
                                        writer.RawOutput(@"\begin{figure}[!ht]");
                                        writer.RawOutput(@"\centering");
                                        pendingFooter = true;
                                        pendingHeader = false;
                                    }

                                    path = path.Replace(@"\", "/");
                                    writer.RawOutput(@"\begin{subfigure}[b]{0.45\linewidth}");
                                    //writer.RawOutput(@"\includegraphics[width=0.25\textwidth]{C:/Users/CordB/OneDrive/Chronik/Bilder/Burmeister/Cord/Cord.jpg}");
                                    writer.RawOutput(@"\includegraphics[width=0.95\textwidth]{" + path + "}");
                                    writer.RawOutput(@"\caption{" + title + "}");
                                    writer.RawOutput(@"\end{subfigure}");

                                    imageCount++;

                                    if ((imageCount % 4) == 0)
                                    {
                                        pendingHeader = true;
                                        writer.RawOutput(@"\caption{" + Strings.ResourceManager.GetString("MediaFor", context.Culture) + individualRecord.GetDisplayName(context.Culture) + " " + sectionCount++ + "}");
                                        writer.RawOutput(@"\centering");
                                        writer.RawOutput(@"\end{figure}");
                                        //                                        writer.RawOutput(@"\afterpage{\clearpage}");
                                        pendingFooter = false;
                                    }
                                }
                                else
                                {
                                    context.Error(@"The Media record has a path reference which can not be found: " + path);
                                }
                            }
                        }
                    }
                    mmr = mr;
                }
                if (pendingFooter)
                {
                    writer.RawOutput(@"\caption{" + Strings.ResourceManager.GetString("MediaFor", context.Culture) + individualRecord.GetDisplayName(context.Culture) + " " + sectionCount++ + "}");
                    writer.RawOutput(@"\centering");
                    writer.RawOutput(@"\end{figure}");
                }
                //writer.RawOutput(@"\afterpage{\clearpage}");
            }
            #endregion

            WriteFamily(context, writer, model, individualRecord);


            #region Now check if there are enough non common events for a detailed chapter
            // Now check if there are enough events for the time line chapter
            var commonEventList = individualRecord.GetNonCommonEventList(model, context.Culture);
            if (commonEventList.Count > 0)
            {
                writer.Header4(Strings.ResourceManager.GetString("PersonalEvents", context.Culture));
                foreach (var e in commonEventList)
                {
                    WriteCommonEvent(context, outputContext, model, writer, e, individualRecord.GetDisplayName(context.Culture));
                }
            }
            #endregion 

            if (context.WriteSources)
            {
                WriteSources(context, writer, model, individualRecord);
            }
        }

        private static void WriteNoteList(ILaTeXExportContext context, Model model, LaTexWriter writer, List<NoteRecord> notes)
        {
            foreach (var n in notes)
            {
                var tl = (n.GetNoteTextList(model, context.Culture));
                foreach (var tn in tl)
                {
                    writer.Text(tn);
                }
            }
        }

        private static void WriteMediaList(ILaTeXExportContext context, Model model, LaTexWriter writer, List<MultimediaRecord> media)
        {
            if (media.Count <= 0) return;

            //writer.RawOutput(@"\begin{figure}[!ht]");
            //writer.RawOutput(@"\centering");
            MultimediaRecord mmr = null;
            foreach (var mr in media)
            {
                mmr = mr;
                if (mmr != null)
                {
                    // Check if this is a refernce 
                    if (!string.IsNullOrEmpty(mmr.Value))
                    {
                        mmr = model.Media.Find(x => x.XrefId == mmr.Value);
                    }

                    if (mmr != null)
                    {
                        var path = mmr.FileName.Value;
                        string title = string.Empty;
                        if (!string.IsNullOrEmpty(mmr.Title))
                        {
                            title = mmr.Title;
                            title = title.Replace("_", " ");
                        }
                        if (!string.IsNullOrEmpty(path))
                        {
                            path = EnsureTeXableImage(path);

                            if (File.Exists(path))
                            {
                                path = path.Replace(@"\", "/");
                                writer.RawOutput(@"\begin{figure}[!ht]");
                                writer.RawOutput(@"\centering");

                                //writer.RawOutput(@"\begin{subfigure}[b]{0.45\linewidth}");
                                ////writer.RawOutput(@"\includegraphics[width=0.25\textwidth]{C:/Users/CordB/OneDrive/Chronik/Bilder/Burmeister/Cord/Cord.jpg}");
                                writer.RawOutput(@"\includegraphics[width=0.95\textwidth]{" + path + "}");
                                //writer.RawOutput(@"\caption{" + title + "}");
                                //writer.RawOutput(@"\end{subfigure}");
                                writer.RawOutput(@"\caption{" + title + "}");
                                writer.RawOutput(@"\centering");
                                writer.RawOutput(@"\end{figure}");

                            }
                            else
                            {
                                context.Error(@"The Media record has a path reference which can not be found: " + path);
                            }
                        }
                    }
                }
            }
            //writer.RawOutput(@"\caption{" + Strings.ResourceManager.GetString("MediaFor", context.Culture) + "}");
            //writer.RawOutput(@"\centering");
            //writer.RawOutput(@"\end{figure}");
        }


        private static void WriteCommonEvent(ILaTeXExportContext context, OutputContext outputContext, Model model, LaTexWriter writer, EventDetailRecord eventItem, string name)
        {

            if (eventItem.Tag != "RESI") return;
            writer.Header5(Strings.ResourceManager.GetString("History", context.Culture));

            if (eventItem.Notes.Count > 0)
            {
                //    writer.Header4(Strings.ResourceManager.GetString("History", context.Culture));
                WriteNoteList(context, model, writer, eventItem.Notes);
            }

            if (eventItem.Address != null)
            {
                //writer.Header4(Strings.ResourceManager.GetString("History", context.Culture));
                WriteNoteList(context, model, writer, eventItem.Address.Notes);


                WriteMediaList(context, model, writer, eventItem.Address.Media);
            }

            if (eventItem.Place != null)
            {
                if (!outputContext.PlaceNames.Contains (eventItem.Place.Value))
                {
                    writer.RawOutput(@"\index[places]{" + writer.ToTeX (eventItem.Place.Value) + "}\n");
                    writer.Label("PLACE:" + eventItem.Place.Value);
                    outputContext.PlaceNames.Add(eventItem.Place.Value);
                    WriteNoteList(context, model, writer, eventItem.Place.Notes);
                    WriteMediaList(context, model, writer, eventItem.Place.Media);
                }
                else
                {
                    writer.HyperReference("PLACE:" + eventItem.Place.Value, eventItem.Place.Value);
                }



            }

            if (eventItem.Media.Count > 0)
            {
                //writer.Header4(Strings.ResourceManager.GetString("History", context.Culture));
                // Images are floating away writer.Header4(Strings.ResourceManager.GetString("Media", context.Culture));
                WriteMediaList(context, model, writer, eventItem.Media);
            }

            
        }

        enum SourceEnum
        {
            Individual,
            Event,
            Name,
            Family,
        }

        class SourceCitationSource
        {
            internal SourceEnum SourceEnum { get; set; }
            internal SourceCitationStructure Source { get; set; }
        }


        private static List<SourceCitationSource> CollectSources(ILaTeXExportContext context, Model model, IndividualRecord individualRecord)
        {
            List<SourceCitationSource> result = new List<SourceCitationSource>();
            if (individualRecord.Sources != null && individualRecord.Sources.Count > 0)
            {
                foreach (var s in individualRecord.Sources)
                {
                    result.Add(new SourceCitationSource()
                    {
                        Source = s,
                        SourceEnum = SourceEnum.Individual
                    });
                }
            }
            if (individualRecord.Names != null && individualRecord.Names.Count > 0)
            {
                foreach (var n in individualRecord.Names)
                {

                    if (n.Sources != null && n.Sources.Count > 0)
                    {
                        foreach (var s in n.Sources)
                        {
                            result.Add(new SourceCitationSource()
                            {
                                Source = s,
                                SourceEnum = SourceEnum.Name
                            });
                        }
                    }

                }
            }

            if (individualRecord.Events != null && individualRecord.Events.Count > 0)
            {
                foreach (var e in individualRecord.Events)
                {

                    if (e.Sources != null && e.Sources.Count > 0)
                    {
                        foreach (var s in e.Sources)
                        {
                            result.Add(new SourceCitationSource()
                            {
                                Source = s,
                                SourceEnum = SourceEnum.Event
                            });
                        }
                    }

                }
            }

            return result;
        }


        private static Dictionary<SourceCitationStructure, List<SourceCitationSource>> CollectSourcesDictionary(ILaTeXExportContext context, Model model, List<SourceCitationSource> sourceCitations)
        {
            var result = new Dictionary<SourceCitationStructure, List<SourceCitationSource>>();

            foreach (var c in sourceCitations)
            {
                var s = model.Sources.Find(x => x.XrefId == c.Source.Value);
                if (s != null)
                {
                    if (!result.ContainsKey(s))
                    {
                        result.Add(s, new List<SourceCitationSource>());

                    }

                    bool foundSource = false;
                    foreach (var s2 in result[s])
                    {
                        if (s2.Source.IsSame(c.Source))
                        {
                            foundSource = true;
                            break;
                        }
                    }
                    if (!foundSource)
                    {
                        result[s].Add(c);
                    }
                }
            }

            return result;
        }

        private static void WriteSources(ILaTeXExportContext context, LaTexWriter writer, Model model, IndividualRecord individualRecord)
        {
            var list = CollectSources(context, model, individualRecord);


            if (list != null && list.Count > 0)
            {
                // First convert list to dictionary
                var dict = CollectSourcesDictionary(context, model, list);

                // Write Source section
                writer.Header4(Strings.ResourceManager.GetString("Sources", context.Culture));


                writer.RawOutput(@"\begin{itemize}");

                foreach (var k in dict.Keys)
                {
                    writer.RawOutput(@"\item ");
                    writer.Text(k.GetTitle());
                    writer.RawOutput("");

                    if (dict[k].Count > 0)
                    {
                        writer.RawOutput(@"\begin{itemize}");

                        foreach (var s in dict[k])
                        {
                            writer.RawOutput(@"\item ");
                            writer.Text(s.Source.Page);
                            writer.RawOutput("");

                            if (s.Source.Media != null && s.Source.Media.Count > 0)
                            {
                                foreach (var m in s.Source.Media)
                                {
                                    if (Path.GetExtension(m.FileName.Value).ToLower() == ".htm")
                                    {
                                        continue;
                                    }
                                    string path = EnsureTeXableImage(m.FileName.Value);

                                    if (context.CopySources)
                                    {
                                        string targetFolder = Path.Combine(context.OutputPath, "media");
                                        if (!Directory.Exists(targetFolder))
                                        {
                                            Directory.CreateDirectory(targetFolder);
                                        }
                                        string targetPath = Path.Combine(context.OutputPath, "media", Path.GetFileName(path));
                                        context.VerboseMessage(" copy media file from '" + path + "' -> '" + targetPath);
                                        File.Copy(path, targetPath, true);
                                        path = Path.Combine(".", "media", Path.GetFileName(path)); ;
                                    }

                                    writer.RawOutput(@"\href{" + path.Replace("\\", "/") + "}{" + Strings.ResourceManager.GetString("MediaFile", context.Culture) + "}");
                                    //writer.RawOutput(@"\href{" + m.FileName.Value.Replace("\\", "/") + "}{$" + Path.GetFileName(m.FileName.Value) + "$}");
                                }
                            }

                        }

                        writer.RawOutput(@"\end{itemize}");

                    }
                }

                writer.RawOutput(@"\end{itemize}");
            }
        }

        /// <summary>
        /// Make sure, that the image can be processed by the LaTeX system. graphichs packge does not process bmp imgaes.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string EnsureTeXableImage(string path)
        {
            if (File.Exists(path))
            {
                string extension = Path.GetExtension(path);
                if ((extension.ToLower() == ".docx"))
                {
                    return null;
                }
                if ((extension.ToLower() == ".htm"))
                {
                    return null;
                }
                if ((extension.ToLower() == ".html"))
                {
                    return null;
                }
                if ((extension.ToLower() == ".bmp") || (extension.ToLower() == ".tif"))
                {
                    string newFile = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".png");
                    if (!File.Exists(newFile))
                    {
                        var bitmap = Bitmap.FromFile(path);
                        bitmap.Save(newFile, ImageFormat.Png);
                    }
                    return newFile;
                }

                string fileName = Path.GetFileNameWithoutExtension(path);
                string[] illegalChars =
                {
                "^",
                ".",
                "(",
                ")",
                " "
            };
                bool hasIllegalChars = false;
                foreach (var s in illegalChars)
                {
                    if (fileName.Contains(s))
                    {
                        fileName = fileName.Replace(s, "");
                        hasIllegalChars = true;
                    }
                }
                if (hasIllegalChars)
                {
                    string newPath = Path.Combine(Path.GetDirectoryName(path), fileName + Path.GetExtension(path));
                    File.Copy(path, newPath, true);
                    return newPath;
                }

            }
            return path;
        }

        private static void WriteFamily(ILaTeXExportContext context, LaTexWriter writer, Model model, IndividualRecord individualRecord)
        {
            IndividualRecord father = null;
            IndividualRecord mother = null;
            bool writtenHeader = false;
            if (individualRecord.ChildFrom != null)
            {
                var family = model.Families.Find(x => x.XrefId == individualRecord.ChildFrom.Value);

                if (family != null)
                {

                    if (family.Husband != null)
                    {
                        father = model.Individuals.Find(x => x.XrefId == family.Husband.Value);
                    }
                    if (family.Wife != null)
                    {
                        mother = model.Individuals.Find(x => x.XrefId == family.Wife.Value);
                    }
                    if ((father != null) || (mother != null))
                    {
                        if (!writtenHeader)
                        {
                            writer.Header4(Strings.ResourceManager.GetString("Family", context.Culture));
                            writtenHeader = true;
                        }

                        if (father != null)
                        {
                            WriteIndividualReference(context, writer, model, Strings.ResourceManager.GetString("Father", context.Culture), father);
                        }
                        if (mother != null)
                        {
                            WriteIndividualReference(context, writer, model, Strings.ResourceManager.GetString("Mother", context.Culture), mother);
                        }
                    }
                }
                foreach (var spouseTo in individualRecord.SpouseTo)
                {
                    var relationShip = model.Families.Find(x => x.XrefId == spouseTo.Value);

                    if (relationShip != null)
                    {
                        IndividualRecord spouse = null;
                        string role = string.Empty;

                        if ((relationShip.Wife != null) && (relationShip.Wife.Value != individualRecord.XrefId))
                        {
                            role = "Wife";
                            spouse = model.Individuals.Find(x => x.XrefId == relationShip.Wife.Value);
                        }
                        if ((relationShip.Husband != null) && (relationShip.Husband.Value != individualRecord.XrefId))
                        {
                            role = "Husband";
                            spouse = model.Individuals.Find(x => x.XrefId == relationShip.Husband.Value);
                        }

                        if ((role != string.Empty) && (spouse != null))
                        {
                            WriteIndividualReference(context, writer, model, Strings.ResourceManager.GetString(role, context.Culture), spouse);
                        }

                        if (!context.WrittenFamilies.Contains(relationShip.XrefId))
                        {
                            context.WrittenFamilies.Add(relationShip.XrefId);

                            foreach (var childReference in relationShip.Children)
                            {
                                var child = model.Individuals.Find(x => x.XrefId == childReference.Value);
                                if (child != null)
                                {
                                    WriteIndividualReference(context, writer, model, Strings.ResourceManager.GetString("Child", context.Culture), child);
                                }
                            }


                        }
                    }
                }
                if (!writtenHeader)
                {
                    writer.Header4(Strings.ResourceManager.GetString("Family", context.Culture));
                    writtenHeader = true;
                }
            }
        }

        private static void WriteIndividualReference(ILaTeXExportContext context, LaTexWriter writer, Model model, string role, IndividualRecord record)
        {
            writer.RawOutput(@"\emph{" + role + ": " + "} ");


            //writer.Text(record.GetDisplayName());
            writer.HyperReference(record.XrefId, record.GetDisplayName(context.Culture));

            writer.RawOutput(@"\newline ");

        }

        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        public static void WriteLaTexBook(ILaTeXExportContext context, Model model, BookMetaInformation bookMetaInformation, string personId)
        {
            string path = CalculateOutputPath(context, context.OutputName + ".tex");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var outputContext = new OutputContext();

            bookMetaInformation = EnsureMetaDataFile(context, bookMetaInformation);


            var record = model.Individuals.Find(x => x.XrefId == personId);
            using (var laWriter = new LaTexWriter(path))
            {


                #region Prepare the book structure with latex setup

                laWriter.RawOutput(@"% Body text font is Palatino!");

                if (context.Culture.Equals (germanCultureInfo))
                {
                    laWriter.RawOutput(@"\documentclass[a4paper]{scrbook}");
                    laWriter.RawOutput(@"\usepackage{ trajan}");
                    laWriter.RawOutput(@"\usepackage[ngerman]{babel}");
                }
                else
                {
                    laWriter.RawOutput(@"\documentclass{scrbook}");
                    laWriter.RawOutput(@"\usepackage{trajan}");
                    laWriter.RawOutput(@"\usepackage{babel}");
                }

                // erfasst man seine Dokumente schon in UTF-8 oder UTF-16 und teilt dies über das inputenc-Paket LaTeX auch mit, 
                // kann man sowohl deutsche als auch französische Anführungszeichen direkt eingeben. Wie man die Satzzeichen direkt 
                // über die Tastatur eingibt, ist im Artikel Anführungszeichen in der Wikipedia beschrieben.
                laWriter.RawOutput(@"\usepackage[utf8]{ inputenc}");
                laWriter.RawOutput(@"\usepackage[T1]{ fontenc}");
                // Will man die Anführungszeichen im gesamten Dokument einheitlich gestalten, sich aber die Möglichkeit offen halten, 
                // das Aussehen der Anführungszeichen global zu ändern, empfiehlt sich das Paket "csquotes". Um die Anführungszeichen 
                // dann zu ändern sind nur die Optionen beim Aufruf des Paketes anzupassen.
                laWriter.RawOutput(@"\usepackage[babel, german = guillemets]{csquotes}");

                laWriter.RawOutput(@"\usepackage[sc]{mathpazo}");
                //laWriter.RawOutput(@"\usepackage{textcomp} % for symbols ");
                laWriter.RawOutput(@"\usepackage{booktabs}");


                laWriter.RawOutput(@"\linespread{ 1.05}");

                laWriter.RawOutput(@"\usepackage{ verbatim} % for comments");
                laWriter.RawOutput(@"\usepackage{ listings} % for comments");

                laWriter.RawOutput(@"\usepackage{fourier,erewhon}");
                laWriter.RawOutput(@"\usepackage{amssymb, amsbsy}");
                laWriter.RawOutput(@"\usepackage{array, booktabs, longtable}");
                laWriter.RawOutput(@"\usepackage[x11names, table]{xcolor}");
                laWriter.RawOutput(@"\usepackage{caption}");
                laWriter.RawOutput(@"\usepackage{tocbasic} % To adjust the format of the content list");


                laWriter.RawOutput(@"\DeclareCaptionFont{blue}{\color{LightSteelBlue3}}");

                laWriter.RawOutput(@"\usepackage[pdftex]{graphicx}");
                laWriter.RawOutput(@"\usepackage{wrapfig}");
                laWriter.RawOutput(@"\usepackage{subcaption}");
                laWriter.RawOutput(@"\usepackage{afterpage}");
                laWriter.RawOutput(@"\usepackage[section]{placeins}");
                laWriter.RawOutput(@"\usepackage{wasysym}");



                laWriter.RawOutput(@"\usepackage{makeidx} % for index generation ");
                laWriter.RawOutput(@"\usepackage{imakeidx} % for several index chapters");

                laWriter.RawOutput(@"\usepackage[colorlinks, linkcolor = black, citecolor = black, filecolor = black, urlcolor=blue]{hyperref}"); // This shall be the last package refernence 
                laWriter.RawOutput(@"\linespread{ 1.05}");
                laWriter.RawOutput(@"\linespread{ 1.05}");
                laWriter.RawOutput(@"\linespread{ 1.05}");

                laWriter.RawOutput(@"\title{A book title}");
                laWriter.RawOutput(@"\author{Author Name}");
                laWriter.RawOutput(@"\date{\today}");


                laWriter.RawOutput(@"\hypersetup{");
                laWriter.RawOutput(@"pdftitle = {" + bookMetaInformation.Title + "},");
                laWriter.RawOutput(@"pdfsubject = {" + bookMetaInformation.Subject + "},");
                laWriter.RawOutput(@"pdfauthor = {" + bookMetaInformation.Author + "},");
                laWriter.RawOutput(@"pdfkeywords = { " + bookMetaInformation.Keywords + "} ,");
                laWriter.RawOutput(@"pdfcreator = { gedcom2book },");
                laWriter.RawOutput(@"pdfproducer = { LaTeX with hyperref}");
                laWriter.RawOutput(@"}");
                laWriter.RawOutput(@"\DeclareTOCStyleEntries[");
                laWriter.RawOutput(@"raggedentrytext,");
                laWriter.RawOutput(@"linefill =\hfill, ");
                laWriter.RawOutput(@"numwidth = 0pt,");
                laWriter.RawOutput(@"numsep = 1ex,");
                laWriter.RawOutput(@"dynnumwidth");
                laWriter.RawOutput(@"]{tocline}{chapter,section,subsection,subsubsection,paragraph,subparagraph}");

                laWriter.RawOutput(@"\newcommand{\foo}{\color{LightSteelBlue3}\makebox[0pt]{\tiny\textbullet}\hskip-0.5pt\vrule width 1pt\hspace{\labelsep}}");
                laWriter.RawOutput(@"\newcommand{\bfoo}{\raisebox{ 2.1ex}[0pt]{\makebox[\dimexpr2\tabcolsep]%");
                laWriter.RawOutput(@"{\color{LightSteelBlue3}\tiny\textbullet}}}%");
                laWriter.RawOutput(@"\newcommand{\tfoo}{\makebox[\dimexpr2\tabcolsep]%");
                laWriter.RawOutput(@"{\color{LightSteelBlue3}$\boldsymbol \uparrow $}}%");

                #endregion

                laWriter.RawOutput(@"\makeindex[title = " + Strings.ResourceManager.GetString("GeneralIndex", context.Culture) + "] % Create the default index");
                laWriter.RawOutput(@"\makeindex[name = persons, title = " + Strings.ResourceManager.GetString("IndexOfNames", context.Culture) + ", columns = 3] % Create an index named 'persons'");
                laWriter.RawOutput(@"\makeindex[name = places, title = " + Strings.ResourceManager.GetString("IndexOfPlaces", context.Culture) + ", columns = 3] % Create an index named 'places'");

                laWriter.RawOutput(@"\setcounter{tocdepth}{1}"); // Restrict to one level which are the names

                laWriter.RawOutput(@"\begin{document}");

                WriteLaTexBookStucture(context, outputContext, model, laWriter, bookMetaInformation, record);

                laWriter.RawOutput(@"\end{document}");
            }
        }
        private static string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                               .Replace("\\", ".")
                                                               .Replace("/", ".");
        }
        public static string GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            resourceName = FormatResourceName(assembly, resourceName);
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    return null;

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static void EnsureLogoFile(ILaTeXExportContext context)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(LaTeXOutputLogic));
            if (!File.Exists(Path.Combine(context.OutputPath, "logo.png")))
            {
                using (Stream stream = assembly.GetManifestResourceStream("MaiorumSeries.LaTeXExport.res.logo.png"))
                {
                    var bitmap = Bitmap.FromStream(stream);
                    bitmap.Save(Path.Combine(context.OutputPath, "logo.png"), ImageFormat.Png);
                }
            }
        }

        private static BookMetaInformation EnsureMetaDataFile(ILaTeXExportContext context, BookMetaInformation bookMetaInformation)
        {
            string fileName = "BookMetaInformation.txt";
            string path = Path.Combine(context.OutputPath, fileName);
            if (!File.Exists(path))
            {
                IoHelper<BookMetaInformation>.Write(bookMetaInformation, path);
            }
            bookMetaInformation = IoHelper<BookMetaInformation>.Read(path);
            return bookMetaInformation;
        }

        public static void WriteLaTexBookTitlePage(ILaTeXExportContext context, Model model, BookMetaInformation bookMetaInformation, LaTexWriter writer)
        {
            //#region title page 

            //writer.RawOutput(@"\begin{titlepage}");
            //writer.RawOutput(@"\centering{");
            //writer.RawOutput(@"{\fontsize{ 40}");
            //writer.RawOutput(@"{48}\selectfont");
            //writer.RawOutput(@"A book title}");
            //writer.RawOutput(@"}\\");
            //writer.RawOutput(@"\vspace{ 10mm}");
            //writer.RawOutput(@"\centering{\Large{ Author Name} }\\");
            //writer.RawOutput(@"\vspace{\fill}");
            //writer.RawOutput(@"\centering \large{ 2011}");
            //writer.RawOutput(@"\end{titlepage}");

            //#endregion

            writer.RawOutput(@"\begin{titlepage}");
            writer.RawOutput(@"\newcommand{\HRule}{\rule{\linewidth}{0.5mm}} % Defines a new command for the horizontal lines, change thickness here");
            writer.RawOutput(@"\center % Center everything on the page");
            writer.RawOutput(@"\textsc{\LARGE " + bookMetaInformation.Subject + @"}\\[1.5cm] % Name of your university/college");
            //writer.RawOutput(@"\textsc{\Large Major Heading}\\[0.5cm] % Major heading such as course name");
            //writer.RawOutput(@"\textsc{\large Minor Heading}\\[0.5cm] % Minor heading such as course title");
            writer.RawOutput(@"%	TITLE SECTION");
            writer.RawOutput(@"\HRule \\[0.4cm]");
            writer.RawOutput(@"{ \huge \bfseries " + bookMetaInformation.Title + @"}\\[0.4cm] % Title of your document");
            writer.RawOutput(@"\HRule \\[1.5cm]");
            //writer.RawOutput(@"%	AUTHOR SECTION");
            //writer.RawOutput(@"\begin{minipage}{0.4\textwidth}");
            //writer.RawOutput(@"\begin{flushleft} \large");
            //writer.RawOutput(@"\emph{" + Strings.ResourceManager.GetString("Author", context.Culture) + ":}\\");
            //writer.RawOutput(@"John \textsc{Smith} % Your name");
            //writer.RawOutput(@"\end{flushleft}");
            //writer.RawOutput(@"\end{minipage}");
            //writer.RawOutput(@"~");
            //writer.RawOutput(@"\begin{minipage}{0.4\textwidth}");
            //writer.RawOutput(@"\begin{flushright} \large");
            //writer.RawOutput(@"\emph{Supervisor:} \\");
            //writer.RawOutput(@"Dr.James \textsc{Smith} % Supervisor's Name");
            //writer.RawOutput(@"\end{flushright}");
            //writer.RawOutput(@"\end{minipage}\\[2cm]");

            writer.RawOutput(@"% If you don't want a supervisor, uncomment the two lines below and remove the section above");
            writer.RawOutput(@"\Large \emph{" + Strings.ResourceManager.GetString("Author", context.Culture) + @":}\\");
            writer.RawOutput(@"\textsc{" + bookMetaInformation.Author + @"}\\[1cm] % Your name");
            writer.RawOutput(@" %	LOGO SECTION");
            writer.RawOutput(@"\includegraphics[height=11cm]{logo.png}\\[1cm] % Include a department/university logo - this will require the graphicx package");
            writer.RawOutput(@"%	DATE SECTION");
            writer.RawOutput(@"{\large \today}\\[2cm] % Date, change the \today to a set date if you want to be precise");
            writer.RawOutput(@"\vfill % Fill the rest of the page with whitespace");
            writer.RawOutput(@"\end{titlepage}");
            EnsureLogoFile(context);
        }

        public static void WriteLaTexBookCitationPage(ILaTeXExportContext context, Model model, LaTexWriter writer)
        {
            writer.RawOutput(@"\newpage{}");

            #region Citation Page 
            writer.RawOutput(@"\thispagestyle {empty}");
            writer.RawOutput(@"\vspace*{2cm}");
            writer.RawOutput(@"\begin{center}");
            writer.RawOutput(@"\Large{\parbox{10cm}{");
            writer.RawOutput(@"\begin{raggedright}");
            writer.RawOutput(@"{\Large ");
            writer.RawOutput(@"\textit{" + Strings.ResourceManager.GetString("CiteText", context.Culture) + "}");
            writer.RawOutput(@"}\\");
            writer.RawOutput(@"\vspace{.5cm}\hfill{---" + Strings.ResourceManager.GetString("CiteAuthor", context.Culture) + "}");
            writer.RawOutput(@"\end{raggedright}");
            writer.RawOutput(@"}");
            writer.RawOutput(@"}");
            writer.RawOutput(@"\end{center}");

            #endregion

        }

        public static void WriteLaTexBookPrefacePage(ILaTeXExportContext context, Model model, LaTexWriter writer)
        {
            #region Preface

            writer.RawOutput(@"\newpage{}");
            writer.RawOutput(@"\subsection *{" + Strings.ResourceManager.GetString("PrefaceTitle", context.Culture) + "}");
            writer.RawOutput(@"\addcontentsline{toc}{subsection}{" + Strings.ResourceManager.GetString("PrefaceTitle", context.Culture) + "}");
            writer.Text(GetPrefaceText(context));

            #endregion

        }



        //            . Einleitung(thematisch gegliederter Fließtext einschließlich nötiger Erklärungen, damit auch Laien den Aufbau verstehen, ca. 40 Seiten)
        //2.Grundliste mit mir als Proband bis zu den Urgroßeltern, nach Kekulé numeriert,
        //3.Ahnenstämme in alphabetischer Reihenfolge der Familiennamen, so dass sich die Einzelfamilien wie in einem Lexikon nachschlagen lassen; nach jedem Ahnenstamm folgt ein Quellen-und Literaturverzeichnis(benutzte Archivalien mit exakter Signatur, Bücher, Aufsätze, Verlinkungen usw.)
        //4.Anhang mit
        //+ Verzeichnis der Abbildungen von Vorfahren(Fotos, Gemälde, Epitaphfiguren usw.)
        //+Leichenpredigtenverzeichnis
        //+ Ahnen graphisch(1 Übersichtstafel und 16 Anschlusstafeln bis Generation 11)
        //+ Stammfolge zu meinem Familiennamen.

        public static void WriteLaTexBookStucture(ILaTeXExportContext context, OutputContext outputContext, Model model, LaTexWriter writer, BookMetaInformation bookMetaInformation, IndividualRecord individualRecord)
        {
            writer.RawOutput(@"\frontmatter");

            WriteLaTexBookTitlePage(context, model, bookMetaInformation, writer);

            WriteLaTexBookMetaInformationPage(context, model, writer);

            WriteLaTexBookCitationPage(context, model, writer);

            WriteLaTexBookPrefacePage(context, model, writer);

            writer.RawOutput(@"\newpage{}");
            writer.RawOutput(@"\tableofcontents");
            writer.RawOutput(@"\mainmatter");

            IncludeChapters(context, model, writer);

            writer.Header1(Strings.ResourceManager.GetString("MainPerson", context.Culture));

            writer.RawOutput(@"\newpage{}");
            writer.Header2(Strings.ResourceManager.GetString("MainPersonAndFamily", context.Culture));
            
            WriteIndividual(context, outputContext, model, writer, individualRecord);
            WriteDescendantsGenerations (context, outputContext, model, writer, individualRecord, true);

            writer.RawOutput(@"\newpage{}");

            if (HasDescendantsGenerations (context, model, individualRecord))
            {
                writer.Header1(Strings.ResourceManager.GetString("Descendants", context.Culture));
                WriteDescendantsGenerations(context, outputContext, model, writer, individualRecord, false);
            }
            writer.Header1(Strings.ResourceManager.GetString("Ancestors", context.Culture));

            WriteAncestorsGenerations(context, outputContext, model, writer, individualRecord);

            writer.RawOutput(@"\appendix");
            writer.RawOutput(@"\listoffigures");
            writer.RawOutput(@"\listoftables");
            writer.RawOutput(@"\printindex");
            writer.RawOutput(@"\printindex[persons] % Output the 'persons' index");
            writer.RawOutput(@"\printindex[places] % Output the 'persons' index");

            //writer.RawOutput(@"\backmatter");
            //writer.RawOutput(@"\chapter{ Last note}");
        }

        private static void IncludeChapters(ILaTeXExportContext context, Model model, LaTexWriter writer)
        {
            var listOfFiles = new List<string>()
            {
                "chapter.tex",
                 "background.tex",
            };


            foreach (var file in listOfFiles)
            {
                if (File.Exists(Path.Combine(context.OutputPath, file)))
                {
                    writer.RawOutput(@"\input{" + Path.GetFileNameWithoutExtension(file) + "}");
                }
            }

        }

        private static void WriteLaTexBookMetaInformationPage(ILaTeXExportContext context, Model model, LaTexWriter writer)
        {

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            string name = assembly.GetName().Name;
            writer.RawOutput(@"\newpage{}");

            #region MetaInformation Page 
            writer.RawOutput(@"\thispagestyle {empty}");
            writer.RawOutput(@"\vspace*{\fill}");
            writer.RawOutput(@"\begin{raggedright}");
            writer.RawOutput(@"{\Large ");

            writer.RawOutput(@"\textit{" + "GedCom Date   " + " " + model.Head.Date.Value.ToString() + @"}\\");
            writer.RawOutput(@"\textit{" + "GedCom File   " + " " + model.Head.FileName + @"}\\");

            if (model.Head.GedComVersion != null && !string.IsNullOrEmpty (model.Head.GedComVersion.Version))
            {
                writer.RawOutput(@"\textit{" + "GedCom Version" + " " + model.Head.GedComVersion.Version + @"}\\");
            }
            if (model.Head.GedComSource != null && !string.IsNullOrEmpty(model.Head.GedComSource.Name))
            {
                writer.RawOutput(@"\textit{" + "GedCom Source " + " " + model.Head.GedComSource.Name + @"}\\");
            }

            writer.RawOutput(@"\textit{" + name + " " + version + @"}\\");
            writer.RawOutput(@"\textit{" + Strings.ResourceManager.GetString("Generated", context.Culture) + " " + DateTime.Now.ToShortDateString() + @"}\\");
            writer.RawOutput(@"}");
            writer.RawOutput(@"\end{raggedright}");

            #endregion
        }

        private static void CollectTribeInfos(ILaTeXExportContext context, Model model, IndividualRecord individualRecord, ref TribeInfo tribeInfo)
        {

            foreach (var spouseTo in individualRecord.SpouseTo)
            {
                var family = model.Families.Find(x => x.XrefId == spouseTo.Value);
                if (family != null)
                {
                    foreach (var childReference in family.Children)
                    {
                        var child = model.Individuals.Find(x => x.XrefId == childReference.Value);
                        if (child != null)
                        {
                            tribeInfo.Descendants++;
                            CollectTribeInfos(context, model, child, ref tribeInfo);
                        }
                    }
                    if (family.Wife != null)
                    {
                        var mother = model.Individuals.Find(x => x.XrefId == family.Wife.Value);
                        if (mother != null)
                        {
                            tribeInfo.Descendants++;
                        }
                    }
                }
            }
        }

        private static void WriteTribeChunkItem(ILaTeXExportContext context, Model model, LaTexWriter writer, IndividualRecord from, bool partner)
        {
            StringBuilder builder = new StringBuilder(@"\item ");
            if (partner)
            {
                builder.Append(@" $\infty$ ");
            }
            writer.RawOutput(builder.ToString());

            writer.HyperReference(from.XrefId, from.GetDisplayName(context.Culture));

            builder = new StringBuilder();
            builder.Append(@" ");
            builder.Append(from.Sex == "M" ? @"\mars" : @"\venus");
            var eventList = from.GetEventList(model, context.Culture);
            foreach (var e in eventList)
            {
                switch (e.Tag)
                {
                    case "BIRT":
                        builder.Append(" ");
                        builder.Append(MapEventTypeToSymbol(e.Tag));
                        builder.Append(" ");
                        builder.Append(e.DateString);
                        break;

                    case "DEAT":
                        builder.Append(" ");
                        builder.Append(MapEventTypeToSymbol(e.Tag));
                        builder.Append(" ");
                        builder.Append(e.DateString);
                        break;
                }
            }


            writer.RawOutput(builder.ToString());
        }

        private static IndividualRecord WriteTribeChunk(ILaTeXExportContext context, Model model, LaTexWriter writer,
            List<IndividualRecord> descendants, IndividualRecord from, int level = 5)
        {
            int newLevel = level - 1;
            IndividualRecord ancestor = null;

            if (newLevel <= 0)
            {
                if (from.DirectAncestor)
                {
                    return from;
                }
                return null;
            }

            writer.RawOutput(@"\begin{itemize}");
            descendants.Add(from);
            WriteTribeChunkItem(context, model, writer, from, false);

            foreach (var spouseTo in from.SpouseTo)
            {
                var relationShip = model.Families.Find(x => x.XrefId == spouseTo.Value);

                if (relationShip != null)
                {
                    #region Write the spouse if available 
                    IndividualRecord spouse = null;

                    if ((relationShip.Wife != null) && (relationShip.Wife.Value != from.XrefId))
                    {
                        spouse = model.Individuals.Find(x => x.XrefId == relationShip.Wife.Value);
                    }
                    if ((relationShip.Husband != null) && (relationShip.Husband.Value != from.XrefId))
                    {
                        spouse = model.Individuals.Find(x => x.XrefId == relationShip.Husband.Value);
                    }

                    if (spouse != null)
                    {
                        descendants.Add(spouse);
                        WriteTribeChunkItem(context, model, writer, spouse, true);

                    }
                    #endregion

                    // Dive into children

                    foreach (var childReference in relationShip.Children)
                    {
                        var child = model.Individuals.Find(x => x.XrefId == childReference.Value);
                        if (child != null)
                        {
                            //writer.RawOutput(@"\item " + child.GetDisplayName() + " " + (child.Sex == "M" ? @"\mars" : @"\venus"));
                            IndividualRecord indi = null;

                            if (newLevel <= 1)
                            {
                                // Repeat with parent
                                indi = WriteTribeChunk(context, model, writer, descendants, from, newLevel);
                            }
                            else
                            {
                                // Continue with child
                                indi = WriteTribeChunk(context, model, writer, descendants, child, newLevel);
                            }
                            if (indi != null)
                            {
                                ancestor = indi;
                            }
                        }
                    }
                }
            }

            writer.RawOutput(@"\end{itemize}");

            //if (from.ChildFrom != null)
            //{
            //    var family = model.Families.Find(x => x.XrefId == from.Individual.ChildFrom.Value);

            //    if (family != null)
            //    {
            //        if (family.Husband == null && family.Wife == null)
            //        {
            //            isTribeCandidate = true;
            //        }
            //        if (family.Husband != null)
            //        {
            //            father = model.Individuals.Find(x => x.XrefId == family.Husband.Value);
            //        }
            //        if (family.Wife != null)
            //        {
            //            mother = model.Individuals.Find(x => x.XrefId == family.Wife.Value);
            //        }

            //        if (father != null)
            //        {
            //            var fatherRecord = from.GetAsFather(father);
            //            AddInGeneration(generation + 1, fatherRecord);
            //            Build(model, fatherRecord, generation + 1);
            //        }
            //        if (mother != null)
            //        {
            //            var motherRecord = from.GetAsMother(mother);
            //            AddInGeneration(generation + 1, motherRecord);
            //            Build(model, motherRecord, generation + 1);
            //        }
            //    }

            //}

            return ancestor;
        }


        private static void WriteTribe(ILaTeXExportContext context, OutputContext outputContext, Model model, LaTexWriter writer, TribeInfo tribe)
        {
            writer.Header2(Strings.ResourceManager.GetString("ClanFrom", context.Culture)  + tribe.Individual.GetDisplayName(context.Culture));

            IndividualRecord current = tribe.Individual;

            List<IndividualRecord> descendants = new List<IndividualRecord>();

            do
            {
                writer.Header3(Strings.ResourceManager.GetString("DescendantsFrom", context.Culture) + current.GetDisplayName(context.Culture));
                current = WriteTribeChunk(context, model, writer, descendants, current);
            } while (current != null);


            bool foundNewDetails = false;

            foreach (var indi in descendants)
            {
                if (!context.WrittenIndividuals.Contains(indi.XrefId))
                {
                    foundNewDetails = false;
                    break;
                }
            }

            if (foundNewDetails)
            {
                writer.Header2(Strings.ResourceManager.GetString("ClanDescendantsFrom ", context.Culture) + tribe.Individual.GetDisplayName(context.Culture));

                foreach (var indi in descendants)
                {
                    WriteIndividualWhenNotWritten(context, outputContext, model, writer, indi);
                }
            }
        }

        private static void WriteAncestorsGenerations(ILaTeXExportContext context, OutputContext outputContext, Model model, LaTexWriter writer, IndividualRecord individualRecord)
        {
            var generations = new ReleationShipStructure(new ReleationshipIndividual(individualRecord));

            generations.BuildAncestors(model);

            foreach (var key in generations.Generations.Keys)
            {
                writer.Header2(Strings.ResourceManager.GetString("Generation", context.Culture) + " " + key.ToString());

                foreach (var i in generations.Generations[key])
                {
                    WriteIndividual(context, outputContext, model, writer, i.Individual);
                }
            }

            if (context.WriteTribe)
            {
                writer.Header1(Strings.ResourceManager.GetString("Clans", context.Culture));

                List<TribeInfo> infos = new List<TribeInfo>();
                foreach (var tribe in generations.TribeCandidates)
                {
                    var info = new TribeInfo(tribe);
                    CollectTribeInfos(context, model, tribe, ref info);
                    infos.Add(info);
                }

                // Sort them by size of tree
                infos.Sort((a, b) => (b.Descendants.CompareTo(a.Descendants)));

                foreach (var t in infos)

                {
                    WriteTribe(context, outputContext, model, writer, t);
                }

            }

        }

        private static bool HasDescendantsGenerations(ILaTeXExportContext context, Model model, IndividualRecord individualRecord)
        {
            var generations = new ReleationShipStructure(new ReleationshipIndividual(individualRecord));
            generations.BuildDescendants(model);

            foreach (var key in generations.Generations.Keys)
            {
                if (key != 0) return true;
            }
            return false;
        }


        private static void WriteDescendantsGenerations(ILaTeXExportContext context, OutputContext outputContext, Model model, LaTexWriter writer, IndividualRecord individualRecord, bool onlyGenerationZero)
        {
            var generations = new ReleationShipStructure(new ReleationshipIndividual(individualRecord));

            generations.BuildDescendants(model);

            foreach (var key in generations.Generations.Keys)
            {

                if (onlyGenerationZero && key != 0)
                {
                    continue;
                }
                if (!onlyGenerationZero && key == 0)
                {
                    continue;
                }

                if (!onlyGenerationZero)
                {
                    writer.Header2(Strings.ResourceManager.GetString("Generation", context.Culture) + " " + key.ToString());
                }

                foreach (var i in generations.Generations[key])
                {
                    WriteIndividual(context, outputContext, model, writer, i.Individual);
                }
            }
        }

        private static string GetPrefaceText(ILaTeXExportContext context)
        {
            string preface = Strings.ResourceManager.GetString("PrefaceText", context.Culture);
            Assembly assembly = Assembly.GetAssembly(typeof(LaTeXOutputLogic));
            if (!File.Exists(Path.Combine(context.OutputPath, "preface.txt")))
            {
                using (Stream stream = assembly.GetManifestResourceStream("MaiorumSeries.LaTeXExport.res.preface.txt"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string result = reader.ReadToEnd();
                        File.WriteAllText(Path.Combine(context.OutputPath, "preface.txt"), result);
                    }
                }
            }
            if (File.Exists(Path.Combine(context.OutputPath, "preface.txt")))
            {
                preface = File.ReadAllText(Path.Combine(context.OutputPath, "preface.txt"));
            }
            return preface;
        }
    }

}
