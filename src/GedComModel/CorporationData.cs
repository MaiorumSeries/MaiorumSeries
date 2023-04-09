using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    [GedComRecord("CORP")]
    public class CorporationData : TypedBaseRecord
    {
        /// <summary>
        /// The address structure should be formed as it would appear on a mailing label using the ADDR and
        /// the CONT lines to form the address structure.The ADDR and CONT lines are required for any
        /// address. The additional subordinate address tags such as STAE and CTRY are provided to be used
        /// by systems that have structured their addresses for indexing and sorting.For backward compatibility
        /// these lines are not to be used in lieu of the required ADDR.and CONT line structure..
        /// </summary>
        [GedComTag("ADDR")]
        public AddressStructure Address { get; set; }

        /// <summary>
        /// A unique number assigned to access a specific telephone.
        /// </summary>
        [GedComTag("PHON")]
        public string PhoneNumber{ get; set; }

        /// <summary>
        /// World Wide Web home page.
        /// </summary>
        [GedComTag("WWW")]
        public string WebPage{ get; set; }
    }
}
