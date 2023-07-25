using System.IO;
using UnityEngine;

namespace ZGH.Core
{
    [DisallowMultipleComponent]
    public class DebugToFile : MonoBehaviour
    {
        public static DebugToFile Inst;

        public string fileFullPath;

        [SerializeField]
        private int m_Count = 0;

        private FileType m_FileType = FileType.TXT;
        private StreamWriter m_FileWriter;
        private LogOutput m_Output = new();

        private void Awake()
        {
            Inst = this;
        }

        private void OnEnable()
        {
            if (!Application.isPlaying) {
                return;
            }

            StartWriter();
        }

        private void OnDisable()
        {
            if (!Application.isPlaying) {
                return;
            }

            EndWriter();
        }

        public bool StartWriter()
        {
            if (m_Count > 0) {
                return false;
            }

            fileFullPath = GetFullPath();
            m_Count = 0;
            m_FileWriter = new StreamWriter(fileFullPath, false);
            m_FileWriter.AutoFlush = true;
            switch (m_FileType) {
                case FileType.CSV:
                    m_FileWriter.WriteLine("type,time,log,stack");
                    break;

                case FileType.JSON:
                    m_FileWriter.WriteLine("[");
                    break;

                case FileType.TSV:
                    m_FileWriter.WriteLine("type\ttime\tlog\tstack");
                    break;
            }

            Application.logMessageReceived += HandleLog;
            return true;
        }

        public void EndWriter()
        {
            Application.logMessageReceived -= HandleLog;

            switch (m_FileType) {
                case FileType.JSON:
                    m_FileWriter.WriteLine("\n]");
                    break;

                case FileType.CSV:
                case FileType.TSV:
                case FileType.TXT:
                default:
                    break;
            }
            m_FileWriter.Close();
            m_Count = 0;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            m_Output.Reset();
            m_Output.stack = stackTrace;
            m_Output.time = $"[Frame:{Time.frameCount}] {System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
            m_Output.log = $"{logString}";
            m_Output.type = type.ToString();

            switch (m_FileType) {
                case FileType.CSV:
                    m_FileWriter.WriteLine(m_Output.type + "," + m_Output.time + "," + m_Output.log.Replace(",", " ").Replace("\n", "") + "," + m_Output.stack.Replace(",", " ").Replace("\n", ""));
                    break;

                case FileType.JSON:
                    m_Output.stack = m_Output.stack.Replace("\n", "\t");
                    m_FileWriter.Write((m_Count == 0 ? "" : ",\n") + JsonUtility.ToJson(m_Output, true));
                    m_FileWriter.WriteLine();
                    break;

                case FileType.TSV:
                    m_FileWriter.WriteLine(m_Output.type + "\type" + m_Output.time + "\type" + m_Output.log.Replace("\type", " ").Replace("\n", "") + "\type" + m_Output.stack.Replace("\type", " ").Replace("\n", ""));
                    break;

                case FileType.TXT:
                    // m_FileWriter.WriteLine("Type: " + m_Output.type);
                    m_FileWriter.WriteLine("Time: " + m_Output.time);
                    m_FileWriter.WriteLine($"{m_Output.type}: {m_Output.log}");
                    m_FileWriter.WriteLine("Stack:\n" + m_Output.stack);
                    break;
            }

            m_Output.Reset();
            m_Count++;
        }

        public string GetFullPath()
        {
            string ext;
            switch (m_FileType) {
                case FileType.JSON: ext = "json"; break;
                case FileType.CSV: ext = "csv"; break;
                case FileType.TSV: ext = "tsv"; break;
                case FileType.TXT: ext = "txt"; break;
                default: ext = "txt"; break;
            }
            var fileName = $"Log_{System.DateTime.Now:yyyymmddhhmmss}.{ext}";
            return Path.Combine(Application.persistentDataPath, fileName);
        }

        public enum FileType
        {
            CSV,
            JSON,
            TSV,
            TXT
        }

        [System.Serializable]
        public class LogOutput
        {
            public string type;
            public string time;
            public string log;
            public string stack;

            public void Reset()
            {
                type = null;
                time = null;
                log = null;
                stack = null;
            }
        }
    }
}