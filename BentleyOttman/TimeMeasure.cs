using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BentleyOttman
{
    public enum TimeMeasure
    {
        None = 0,
        [Description("минут")]
        Minutes = 1,
        [Description("часов")]
         Hours = 2,
         [Description("дней")]
         Days = 3
    }
}
