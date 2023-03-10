using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("DATE")]
    public class DateRecord : TypedBaseRecord
    {
        /// <summary>
        /// </summary>
        [GedComTag("TIME")]
        public string Time { get; set; }


        /// <summary>
        /// </summary>
        [GedComTag("_TIMEZONE")]
        public string TimeZone { get; set; }

        
    }
}
