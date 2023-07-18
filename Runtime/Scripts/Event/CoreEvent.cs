using System;

namespace ZGH.Core
{
    public enum CoreEvent : int
    {
        MonoUpdate = -100,
        MonoFixUpdate = -200,
        FifthSecondUpdate = -300,
        PerSecondUpdate = -400,
    }
}