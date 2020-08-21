﻿using System;

namespace BentleyOttman
{
    public class RepairExclusion : IBaseRule
    {
        public Guid? Guid { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Offset { get; set; }
        public TimeMeasure OffsetUom { get; set; }

        public RepairExclusion(DateTime start, DateTime end, int? offset, TimeMeasure offsetUom)
        {
            Guid = null;
            this.Start = start;
            this.End = end;
            this.OffsetUom = offsetUom;
            this.Offset = offset.HasValue == false ? 0 : offset.Value;
        }
    }
}
