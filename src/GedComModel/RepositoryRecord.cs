using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("REPO")]
    public class RepositoryRecord : TypedBaseRecord
    {
        [GedComTag("NAME")]
        public string Name { get; set; }

        [GedComTag("ADDR")]
        public string Address { get; set; }

    }
}
