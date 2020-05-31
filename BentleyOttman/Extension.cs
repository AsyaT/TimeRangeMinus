using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoBentleyOttman
{
    public static class Extension
    {
        public static void AddInterval(this Dictionary<DateTime, MainDicStructure> dicStructure, IBaseRule interval)
        {
            bool isRule = true;
            if (interval.GetType() == typeof(RepairRule))
                isRule = true;
            else if (interval.GetType() == typeof(RepairExclusion))
                isRule = false;

            dicStructure.Add(interval.Start, new MainDicStructure(true, isRule));
            dicStructure.Add(interval.End, new MainDicStructure(false, isRule));

            DateTime offset = new DateTime(0);

            while (offset.Ticks < new DateTime(0).AddYears(1).Ticks)
            {
                switch (interval.OffsetUom)
                {
                    case "день":
                        offset = offset.AddDays(interval.Offset);
                        break;
                    case "час":
                        offset = offset.AddHours(interval.Offset);
                        break;
                }

                dicStructure.Add(interval.Start.AddTicks(offset.Ticks), new MainDicStructure(true, isRule));
                dicStructure.Add(interval.End.AddTicks(offset.Ticks), new MainDicStructure(false, isRule));
            }
        }
    }
}
