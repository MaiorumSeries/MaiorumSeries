using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    /// <summary>
    /// One event item as descriptive item direved from teh GedCom Model 
    /// </summary>
    public class EventItem
    {
        public string Tag { get; set; }

        /// <summary>
        /// Best known Date Time representation 
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Date text representation 
        /// </summary>
        public string DateString { get; set; }

        /// <summary>
        /// Description of the event 
        /// </summary>
        public string Description { get; set; }
    }
}
