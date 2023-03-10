using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MaiorumSeries.LaTeXExport
{
    public interface ILaTeXExportContext
    {
     

        /// <summary>
        /// The path, where all files will be stored to build the book.
        /// </summary>
        string OutputPath { get; set; }

        /// <summary>
        /// Name of the output files. This will be the final name of the PDF.
        /// </summary>
        string OutputName { get; set; }

        /// <summary>
        /// Flag to write source chapter for individuals
        /// </summary>
        bool WriteSources { get; set; }


        /// <summary>
        /// Flag to write tribe chapter 
        /// </summary>
        bool WriteTribe { get; set; }

        /// <summary>
        /// Flag to copy sources media to output folder
        /// </summary>
        bool CopySources { get; set; }

        /// <summary>
        /// This is the culture, in which languge the output will be generated.  
        /// </summary>
        CultureInfo Culture { get; set; }

        /// <summary>
        /// Remember the list of indivuduals which have been written.  
        /// </summary>
        List<string> WrittenIndividuals { get; set; } 

        /// <summary>
        /// Remember the list of families which have been written.  
        /// </summary>
        List<string> WrittenFamilies { get; set; } 



        void Error(string message);

        void VerboseMessage(string message);

    }
}
