﻿using System;

namespace UAssetAPI.UnrealTypes.Enums
{
    [Flags]
    public enum ETextFlag
    {
        Transient = 1 << 0,
        CultureInvariant = 1 << 1,
        ConvertedProperty = 1 << 2,
        Immutable = 1 << 3,
        InitializedFromString = 1 << 4
    }
}
