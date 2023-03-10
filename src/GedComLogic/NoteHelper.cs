using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    public static class NoteHelper
    {


        public static List<string> GetNoteTextList(this NoteRecord noteDetail, Model model, CultureInfo cultureInfo)
        {


            List<string> list = new List<string>();
            NoteRecord note = noteDetail;
            if (!string.IsNullOrEmpty (noteDetail.XrefId))
            {
                var n = model.Notes.Find(x => x.Value == noteDetail.XrefId);
                if (n != null)
                {
                    note = n;
                }
                else
                {
                    return list;
                }
            }

            var str = new StringBuilder(note.Value);
            foreach (var line in note.Lines)
            {
                if (line.Tag == "CONC")
                {
                    str.Append(line.Value);
                }
                if (line.Tag == "CONT")
                {
                    str.Append("\n\n");

                    list.Add(str.ToString());
                    str = new StringBuilder();
                    str.Append(line.Value);
                }
            }
            list.Add(str.ToString());
            return list;
        }
    }
}
