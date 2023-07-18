using System;
using System.Collections.Generic;

namespace ZGH.Core
{
    public static partial class Utils
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var t in source) {
                action(t);
            }
        }
    }
}