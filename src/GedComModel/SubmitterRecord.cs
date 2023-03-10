using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    /// <summary>
    /// The submitter record identifies an individual or organization that contributed information contained
    /// in the GEDCOM transmission.All records in the transmission are assumed to be submitted by the
    /// SUBMITTER referenced in the HEADer, unless a SUBMitter reference inside a specific record
    /// points at a different SUBMITTER record.
    /// </summary>
    [GedComRecord("SUBM")]
    public class SubmitterRecord : TypedBaseRecord
    {
        ///// <summary>
        ///// An identifier that represents the version level assigned to the associated product. 
        ///// It is defined and changed by the creators of the product.
        ///// <remarks>
        ///// In this Case it is the version of the Programm generating this Gedcom file
        ///// </remarks>
        ///// </summary>
        //[GedComTag("VERS")]
        //public string Version { get; set; }


        /// <summary>
        /// A unique record identification number assigned to the record by the source system.
        /// This number is intended to serve as a more sure means of identification of a record for reconciling 
        /// differences in data between two interfacing systems.
        /// </summary>
        [GedComTag("RIN")]
        public string RecordId { get; set; }

        /// <summary>
        /// The name of the submitter formatted for display and address generation.
        /// </summary>
        [GedComTag("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// The address structure should be formed as it would appear on a mailing label using the ADDR and
        /// the CONT lines to form the address structure.The ADDR and CONT lines are required for any
        /// address. The additional subordinate address tags such as STAE and CTRY are provided to be used
        /// by systems that have structured their addresses for indexing and sorting.For backward compatibility
        /// these lines are not to be used in lieu of the required ADDR.and CONT line structure..
        /// </summary>
        [GedComTag("ADDR")]
        public AddressStructure Address { get; set; }

    }
}
