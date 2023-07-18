using System.Diagnostics;
using UnityEngine;

namespace ZGH.Core
{
    public static class Log
    {
        [Conditional("LogOn")]
        public static void Assert(object obj, string s)
        {
            if (obj == null) {
                LogE(s);
            }
        }

        [Conditional("LogOn")]
        public static void Dump(string title, object obj)
        {
            string json = JsonUtility.ToJson(obj);
            LogI($"{title}:{json}");
        }

        [Conditional("LogOn")]
        public static void I<T1>(T1 s)
        {
            LogI(s.ToString());
        }

        [Conditional("LogOn")]
        public static void I<T1, T2>(T1 s1, T2 s2)
        {
            LogI(s1.ToString(), s2.ToString());
        }

        [Conditional("LogOn")]
        public static void I<T1, T2, T3>(T1 s1, T2 s2, T3 s3)
        {
            LogI(s1.ToString(), s2.ToString(), s3.ToString());
        }

        [Conditional("LogOn")]
        public static void I<T1, T2, T3, T4>(T1 s1, T2 s2, T3 s3, T4 s4)
        {
            LogI(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString());
        }

        [Conditional("LogOn")]
        public static void I<T1, T2, T3, T4, T5>(T1 s1, T2 s2, T3 s3, T4 s4, T5 s5)
        {
            LogI(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString(), s5.ToString());
        }

        [Conditional("LogOn")]
        public static void W<T1>(T1 s)
        {
            LogW(s.ToString());
        }

        [Conditional("LogOn")]
        public static void W<T1, T2>(T1 s1, T2 s2)
        {
            LogW(s1.ToString(), s2.ToString());
        }

        [Conditional("LogOn")]
        public static void W<T1, T2, T3>(T1 s1, T2 s2, T3 s3)
        {
            LogW(s1.ToString(), s2.ToString(), s3.ToString());
        }

        [Conditional("LogOn")]
        public static void W<T1, T2, T3, T4>(T1 s1, T2 s2, T3 s3, T4 s4)
        {
            LogW(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString());
        }

        [Conditional("LogOn")]
        public static void W<T1, T2, T3, T4, T5>(T1 s1, T2 s2, T3 s3, T4 s4, T5 s5)
        {
            LogW(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString(), s5.ToString());
        }

        public static void E<T1>(T1 s)
        {
            LogE(s.ToString());
        }

        public static void E<T1, T2>(T1 s1, T2 s2)
        {
            LogE(s1.ToString(), s2.ToString());
        }

        public static void E<T1, T2, T3>(T1 s1, T2 s2, T3 s3)
        {
            LogE(s1.ToString(), s2.ToString(), s3.ToString());
        }

        public static void E<T1, T2, T3, T4>(T1 s1, T2 s2, T3 s3, T4 s4)
        {
            LogE(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString());
        }

        public static void E<T1, T2, T3, T4, T5>(T1 s1, T2 s2, T3 s3, T4 s4, T5 s5)
        {
            LogE(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString(), s5.ToString());
        }

        [Conditional("LogOn")]
        private static void LogI(params string[] args)
        {
            UnityEngine.Debug.Log($"[{Time.frameCount}]" + string.Join("|", args));
            // to do
        }

        [Conditional("LogOn")]
        private static void LogW(params string[] args)
        {
            UnityEngine.Debug.Log($"[{Time.frameCount}]" + string.Join("|", args));
            // to do
        }

        private static void LogE(params string[] args)
        {
            UnityEngine.Debug.Log($"[{Time.frameCount}]" + string.Join("|", args));
            // to do
        }
    }
}