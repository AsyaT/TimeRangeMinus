using System;
using BentleyOttman;

namespace AlgoBentleyOttman
{
    public interface IBaseRule
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int Offset { get; set; }
        public TimeMeasure OffsetUom { get; set; }
    }
}
