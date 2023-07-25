using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using UnityEngine;

namespace ZGH.Core
{
    public class LogToFile : Singleton<LogToFile>
    {
        private string m_fullPath;
        private StreamWriter m_FileWriter;
        private int m_OutputCount;

        private Thread m_WriteThread;
        private ManualResetEvent m_ManualResetEvent;

        private ConcurrentQueue<string> m_LogQueue1 = new();
        private ConcurrentQueue<string> m_LogQueue2 = new();
        private ConcurrentQueue<string> m_LogQueue3 = new();
        private int m_QueueCount = 0;

        public int outputCount => m_OutputCount;
        public string fileFullPath => m_fullPath;
        public int queueCount => m_QueueCount;

        public override void Init()
        {
            StartWriter();
        }

        public override void Dispose()
        {
            EndWriter();
            base.Dispose();
        }

        private void StartWriter()
        {
            m_OutputCount = 0;

            m_LogQueue1.Clear();
            m_LogQueue2.Clear();
            m_LogQueue3.Clear();

            var fileName = $"Log_{DateTime.Now:yyyymmddhhmmssffff}.txt";
            m_fullPath = Path.Combine(Application.persistentDataPath, fileName);
            m_FileWriter = new StreamWriter(m_fullPath, false);
            m_FileWriter.AutoFlush = true;

            m_ManualResetEvent = new ManualResetEvent(false);
            m_WriteThread = new Thread(WriteToFileThread) {
                Name = "LogToFile",
                IsBackground = true
            };
            m_WriteThread.Start();

            Application.logMessageReceivedThreaded += HandleLogThreaded;
        }

        private void EndWriter()
        {
            Application.logMessageReceivedThreaded -= HandleLogThreaded;

            m_ManualResetEvent.Dispose();
            m_ManualResetEvent = null;
            m_WriteThread.Abort();
            m_WriteThread = null;

            m_FileWriter.Close();
            m_FileWriter = null;

            m_LogQueue1.Clear();
            m_LogQueue2.Clear();
            m_LogQueue3.Clear();
        }

        private void HandleLogThreaded(string logString, string stackTrace, LogType type)
        {
            m_LogQueue1.Enqueue($"frameCount: {Time.frameCount}");
            m_LogQueue2.Enqueue($"{type}: {logString}");
            m_LogQueue3.Enqueue(stackTrace);
            m_QueueCount++;
            m_ManualResetEvent.Set();
        }

        private void WriteToFileThread()
        {
            while (m_ManualResetEvent != null) {
                m_ManualResetEvent.WaitOne();
                try {
                    while (m_QueueCount > 0) {
                        if (m_LogQueue1.TryDequeue(out var str1)) {
                            m_FileWriter.WriteLine(str1);
                        }
                        if (m_LogQueue2.TryDequeue(out var str2)) {
                            m_FileWriter.WriteLine(str2);
                        }
                        if (m_LogQueue3.TryDequeue(out var str3)) {
                            m_FileWriter.WriteLine(str3);
                        }

                        m_QueueCount--;
                        m_OutputCount++;
                    }
                } catch (Exception e) {
                    throw e;
                } finally {
                    m_ManualResetEvent.Reset();
                }
            }
        }
    }
}