using System;

namespace Bedroom
{
    [Flags]
    public enum DayTime
    {
        None = 0,
        Day = 1 << 0,
        Night = 1 << 1
    }
}