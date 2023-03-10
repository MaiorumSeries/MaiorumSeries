using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.LaTeXExport
{
    [Serializable]
    public class BookMetaInformation
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Keywords { get; set; }
    }
}
