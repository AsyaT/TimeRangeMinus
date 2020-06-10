using System;


namespace BentleyOttman
{
    public static class Extensions
    {
        internal static DateTime CalculateOffset(this DateTime currentOffset, int offset, TimeMeasure offsetUom)
        {
            switch (offsetUom)
            {
                case TimeMeasure.Minutes:
                    return currentOffset.AddMinutes(offset);

                case TimeMeasure.Days:
                    return currentOffset.AddDays(offset);

                case TimeMeasure.Hours:
                    return currentOffset.AddHours(offset);

            }

            return currentOffset;
        }
    }
}
