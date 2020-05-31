using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoBentleyOttman
{
    public class RepairExclusion : IBaseRule
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Offset { get; set; }
        public string OffsetUom { get; set; }

        public RepairExclusion(DateTime start, DateTime end, int offset, string offsetUom)
        {
            this.Start = start;
            this.End = end;
            this.Offset = offset;
            this.OffsetUom = offsetUom;
        }
    }
}
