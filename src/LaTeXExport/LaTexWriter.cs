// ***********************************************************************
// Assembly         : ReleaseManagerBusinessLogic
// Author           : Cord Burmeister
// ***********************************************************************
// <copyright file="MarkdownWriter.cs" company="Cord Burmeister">
//     Copyright © Cord Burmeister 2016
// </copyright>
// <summary>Class to write Markdown files</summary>
// ***********************************************************************


using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MaiorumSeries.LaTeXExport
{
    /// <summary>
    /// Class for writing a LateX File. Created a new class, because the Processing API is only
    /// available as private assembly from the VS2010 Version.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    [ExcludeFromCodeCoverage] 
    public sealed class LaTexWriter : IDisposable
    {
        private readonly TextWriter _textWriter;


        /// <summary>
        /// Initializes a new instance of the <see cref="LaTexWriter" /> class.
        /// Creates a file and writes to it
        /// </summary>
        /// <param name="outputFile">The output file.</param>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
        public LaTexWriter(string outputFile)
        {
            _textWriter = File.CreateText (outputFile);
        }

        /// <summary>
        /// Closes this instance of the Mark down writer.
        /// </summary>
        private void Close()
        {
            _textWriter?.Close();
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
            // free native resources if there are any.
        }

        
        public void RawOutput(string message)
        {
            _textWriter.WriteLine(message);
        }

        public string ToTeX(string message)
        {
            string tex = message.Replace("_", @"\_");
            tex = tex.Replace("&", @"\&");
            tex = tex.Replace("#", @"\#");
            tex = tex.Replace("µ", "m ");
            return tex;
        }


        /// <summary>
        /// Writes a header 1 title.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void Header1 (string message)
        {
            _textWriter.WriteLine("\\part{" + message + "}");
            _textWriter.WriteLine("");

        }

        /// <summary>
        /// Writes a header 2 title..
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void Header2(string message)
        {
            _textWriter.WriteLine("\\chapter{" + message + "}");
            _textWriter.WriteLine("");
        }

        /// <summary>
        /// Writes a header 3 title.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void Header3(string message)
        {
            _textWriter.WriteLine("\\section{" + message + "}");
            _textWriter.WriteLine("");
        }

        /// <summary>
        /// Writes a header 4 title.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void Header4(string message)
        {
            _textWriter.WriteLine("\\subsection{" + message + "}");
            _textWriter.WriteLine("");
        }

        /// <summary>
        /// Writes a header 5 title.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void Header5(string message)
        {
            _textWriter.WriteLine("\\subsubsection{" +  message + "}");
            _textWriter.WriteLine("");
        }

        /// <summary>
        /// Writes a header 6 title.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void Header6(string message)
        {
            _textWriter.WriteLine("\\paragraph{" + message + "}");
            _textWriter.WriteLine("");
            //_textWriter.WriteLine("\\subparagraph{" + message + "}");
            //_textWriter.WriteLine("");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void Text(string message)
        {
            if (string.IsNullOrEmpty (message))
            {
                return;
            }
            _textWriter.Write(ToTeX( message));
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void ItalicText(string message)
        {
            _textWriter.Write(message);
        }
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void BoldText(string message)
        {
            _textWriter.Write(message);
        }
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void ItalicAndBoldText(string message)
        {
            _textWriter.Write(message);
        }


        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void FlushFloating ()
        {

            _textWriter.WriteLine("\\afterpage{\\clearpage}");
        }
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>

        public void FloatingImage(string description, string link)
        {


            _textWriter.WriteLine("\\begin{figure}[ht]");
            _textWriter.WriteLine("\\centering");
            _textWriter.WriteLine("\\includegraphics[width = 16cm]{" + link + "}");
            _textWriter.WriteLine("\\caption{ " +  ToTeX ( description ) + " }");
           // _textWriter.Write("\\label{fig:120px-Robot_omnidirectional_drive}");
            _textWriter.WriteLine("\\end{figure}");

        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void NonFloatingImage(string description, string link)
        {



            _textWriter.WriteLine("\\begin{figure}[ht]");
            _textWriter.WriteLine("\\centering");
            _textWriter.WriteLine("\\includegraphics[width=0.9\\textwidth]{" + link + "}");
            _textWriter.WriteLine("\\caption{ " + ToTeX(description) + " }");
            // _textWriter.Write("\\label{fig:120px-Robot_omnidirectional_drive}");
            _textWriter.WriteLine("\\end{figure}");


            //_textWriter.WriteLine("\\begin{center}");
            //_textWriter.WriteLine("\\includegraphics[width=0.9\\textwidth]{" + link + "}");
            //_textWriter.WriteLine("\\end{center}");

        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void IndexEntry(string index)
        {
            _textWriter.WriteLine("\\index{" + index + "} ");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void IndexEntry2(string index1, string index2)
        {
            _textWriter.WriteLine("\\index{" + index1 + "!" + index2 + "} ");
        }
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void IndexEntry3(string index1, string index2, string index3)
        {
            _textWriter.WriteLine("\\index{" + index1 + "!" + index2 + "!" + index3 + "} ");
        }




        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void EndParapharph()
        {
            _textWriter.WriteLine("");
            _textWriter.WriteLine("");
        }
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void HorizontalRuler()
        {
            _textWriter.WriteLine("---");
        }


        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void UnorderedListItem(string message)
        {
            _textWriter.WriteLine("* " + message);
            _textWriter.WriteLine("");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void UnorderedListParagraph(string message)
        {
            _textWriter.WriteLine("   " + message + "  ");
            _textWriter.WriteLine("");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void UnorderedSubListItem(string message)
        {
            _textWriter.WriteLine("  * " + message);
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void EndDescriptionEnvironment()
        {
            _textWriter.WriteLine("\\end{description}");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void StartDescriptionEnvironment()
        {
            _textWriter.WriteLine("\\begin{description}");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void EndEnumerateEnvironment()
        {
            _textWriter.WriteLine("\\end{enumerate}");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void StartEnumerateEnvironment()
        {
            _textWriter.WriteLine("\\begin{enumerate}");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void Item()
        {
            _textWriter.WriteLine("\\item");
        }
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void Item(string description)
        {
            _textWriter.WriteLine("\\item ["+ description + "]");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void Label(string label)
        {
            _textWriter.WriteLine("\\label{" + label + "}");
        }

        /// <summary>
        /// Writing a normal reference as chapter number to the label
        /// </summary>
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void Reference(string label)
        {
            _textWriter.WriteLine("\\ref{" + label + "}");
        }

        /// <summary>
        /// Writing a hyper reference as string to the label
        /// </summary>
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void HyperReference(string label, string description)
        {
            _textWriter.WriteLine("\\hyperref[" + label + "]{" + description + "}");
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void Hyperlink(string description, string link)
        {
            _textWriter.WriteLine("\\href{" + link + "}{" + description + "}");
        }


        //       \begin{tabular
        //   }{|c|c|}\hline
        //  Lehrstuhl & Professor \\ \hline
        //  BWL & Maier \\ \hline
        //  MB & M"uller \\ \hline
        //  Jura & Schmidt \\ \hline
        //\end{tabular}
        private int _columnCounter;
        private int _columnSize;


        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void StartTableEnvironment(int tabular, int width)
        {

            _textWriter.WriteLine("");
            _textWriter.Write("\\begin{tabular}{|");
            for (int i = 0; i < tabular; i++)
            {
                _textWriter.Write("p{" + width +"cm}|");
            }
            _textWriter.WriteLine("}");
            _textWriter.WriteLine("\\hline");
            _columnCounter = 0;
            _columnSize = tabular;
        }


        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void TableCell(string cell)
        {
            _columnCounter ++;
            if (_columnCounter == _columnSize)
            {
                _textWriter.Write(cell);
                _textWriter.WriteLine(" \\\\ \\hline");
            }
            else if (_columnCounter < _columnSize)
            {
                _textWriter.Write(cell);
                _textWriter.Write(" & ");
            }
        }

        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void TableCellAndReference(string cell, string label)
        {
            _columnCounter++;
            if (_columnCounter == _columnSize)
            {
                _textWriter.Write(cell);
                _textWriter.Write(" ");
                Reference(label);
                _textWriter.WriteLine(" \\\\ \\hline");
            }
            else if (_columnCounter < _columnSize)
            {
                _textWriter.Write(cell);
                _textWriter.Write(" ");
                Reference(label);
                _textWriter.Write(" & ");
            }
        }


        internal void RowEnd()
        {
            _columnCounter=0;
        }
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        internal void EndTableEnvironment()
        {
            _textWriter.WriteLine("\\end{tabular}");
        }

        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        public void EmailLink(string idContactPerson, string idContactPersonEmail)
        {
            _textWriter.WriteLine("\\href{ mailto:" + idContactPersonEmail + "}{" + idContactPerson + "}");
        }
    }
}
