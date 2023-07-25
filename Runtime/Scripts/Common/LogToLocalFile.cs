using System;
using System.IO;
using UnityEngine;

namespace ZGH.Core
{
    public class LogToLocalFile : MonoBehaviour
    {
        [HideInInspector]
        public string fileFullPath;

        public int count;
        public int QueueCount;
        private StreamWriter m_FileWriter;

        private void OnEnable()
        {
            count = 0;
            var fileName = $"Log_{DateTime.Now:yyyymmddhhmmss}.txt";
            fileFullPath = Path.Combine(Application.persistentDataPath, fileName);
            LogMessageReceivedMgr.Instance.onWriteEvent += AppendObjectWithString;
        }

        private void OnDisable()
        {
            LogMessageReceivedMgr.Instance.onWriteEvent -= AppendObjectWithString;
            m_FileWriter.Close();
            m_FileWriter = null;

            LogMessageReceivedMgr.Instance.Dispose();
        }

        private void AppendObjectWithString(string fileContent)
        {
            if (m_FileWriter == null)
            {
                m_FileWriter = new StreamWriter(fileFullPath, false);
                m_FileWriter.AutoFlush = true;
            }

            m_FileWriter.WriteLine(fileContent);
            count++;
            QueueCount = LogMessageReceivedMgr.Instance.GetOutputQueueCount();
        }
    }
}