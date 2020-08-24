using System;

namespace BentleyOttman
{
    public class Rule : IBaseRule
    {
        public Guid? Guid { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Offset { get; set; }
        public TimeMeasure OffsetUom { get; set; }

        public Rule(Guid guid, DateTime start, DateTime end, int? offset, TimeMeasure offsetUom)
        {
            this.Guid = guid;
            this.Start = start;
            this.End = end;
            this.OffsetUom = offsetUom;
            this.Offset = offset.HasValue == false ? 0 : offset.Value;
        }
    }
}
