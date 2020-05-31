using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoBentleyOttman
{
    public interface IBaseRule
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int Offset { get; set; }
        public string OffsetUom { get; set; }
    }
}
