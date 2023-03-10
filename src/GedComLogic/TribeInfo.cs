using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    public class TribeInfo
    {
        private IndividualRecord _record;

        public IndividualRecord Individual
        {
            get
            {
                return _record;
            }
        }

        /// <summary>
        /// Number of total Descendants
        /// </summary>
        public int Descendants { get; set; } = 0;

        /// <summary>
        /// Construcor from IndividualRecord 
        /// </summary>
        /// <param name="record"></param>
        public TribeInfo(IndividualRecord record)
        {
            _record = record;
        }
    }
}
