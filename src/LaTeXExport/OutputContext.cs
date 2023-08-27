using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.LaTeXExport
{
    /// <summary>
    /// Storing some context, of the already outputed content
    /// </summary>
    public  class OutputContext
    {
        public List<string> PlaceNames { get; set; } = new List<string>();
    }
}
