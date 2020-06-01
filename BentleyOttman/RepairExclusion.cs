using System;
using BentleyOttman;

namespace AlgoBentleyOttman
{
    public class RepairExclusion : IBaseRule
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Offset { get; set; }
        public TimeMeasure OffsetUom { get; set; }

        public RepairExclusion(DateTime start, DateTime end, int offset, TimeMeasure offsetUom)
        {
            this.Start = start;
            this.End = end;
            this.Offset = offset;
            this.OffsetUom = offsetUom;
        }
    }
}
