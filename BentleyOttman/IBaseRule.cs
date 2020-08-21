using System;

namespace BentleyOttman
{
    public interface IBaseRule
    {
        public Guid? Guid { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int Offset { get; set; }
        public TimeMeasure OffsetUom { get; set; }
    }
}
