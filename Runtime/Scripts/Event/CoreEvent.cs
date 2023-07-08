using System;

namespace ZGH.Core
{
    public enum CoreEvent : int
    {
        MonoUpdate = -1,
        MonoFixUpdate = -2,
        FifthSecondUpdate = -3,
        PerSecondUpdate = -4,
    }
}