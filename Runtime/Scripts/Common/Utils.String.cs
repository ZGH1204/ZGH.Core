using UnityEngine;

namespace ZGH.Core
{
    public static partial class Utils
    {
        public static string ToHourMinSec(int time)
        {
            var hour = Mathf.Floor(time / 3600);
            var min = Mathf.Floor((time - hour * 3600) / 60);
            var sec = Mathf.Floor(time % 60);
            return $"{hour:00}:{min:00}:{sec:00}";
        }

        public static string ToMinSec(int time)
        {
            var hour = Mathf.Floor(time / 3600);
            var min = Mathf.Floor((time - hour * 3600) / 60);
            var sec = Mathf.Floor(time % 60);
            return $"{min:00}:{sec:00}";
        }

        public static string UpperFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str)) {
                return null;
            }
            return char.ToUpper(str[0]) + str[1..];
        }

        public static string LowerFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str)) {
                return null;
            }
            return char.ToLower(str[0]) + str[1..];
        }
    }
}