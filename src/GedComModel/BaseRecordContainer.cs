using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComModel
{
    public class BaseRecordContainer : List<BaseRecord>
    {
        public void AddBaseRecord(BaseRecord baseRecord)
        {
            Add(baseRecord);
        }
    }
}
