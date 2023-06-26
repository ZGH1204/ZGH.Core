using UnityEngine;

namespace ZGH.Core
{
    public partial class Utils
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
    }
}