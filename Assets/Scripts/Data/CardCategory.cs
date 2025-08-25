using System;

[System.Flags]
public enum CardCategory
{
    None = 0,
    Attack = 1 << 0,   // 0001
    Defense = 1 << 1,   // 0010
    Special = 1 << 2    // 0100
}
