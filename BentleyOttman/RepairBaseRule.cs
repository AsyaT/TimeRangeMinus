using System;
using System.Collections.Generic;
using System.Text;
using BentleyOttman;

namespace AlgoBentleyOttman
{
    public class RepairRule : IBaseRule
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Offset { get; set; }
        public TimeMeasure OffsetUom { get; set; }

        public RepairRule(DateTime start, DateTime end, int offset, TimeMeasure offsetUom)
        {
            this.Start = start;
            this.End = end;
            this.Offset = offset;
            this.OffsetUom = offsetUom;
        }
    }
}
