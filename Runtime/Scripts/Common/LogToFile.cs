using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using UnityEngine;

namespace ZGH.Core
{
    public class LogToFile : Singleton<LogToFile>
    {
        public bool isCheck = false;

        private string m_fullPath;
        private StreamWriter m_FileWriter;
        private int m_OutputCount;
        private bool m_IsPause;

        private Thread m_WriteThread;
        private ManualResetEvent m_ResetEvent;

        private ConcurrentQueue<LogType> m_LogQueue1 = new();
        private ConcurrentQueue<string> m_LogQueue2 = new();
        private ConcurrentQueue<string> m_LogQueue3 = new();
        private int m_QueueCount = 0;

        public int outputCount => m_OutputCount;
        public string fileFullPath => m_fullPath;
        public int queueCount => m_QueueCount;
        public bool isPause => m_IsPause;

        public override void Init() {
            StartWriter();
        }

        public override void Dispose() {
            EndWriter();
            base.Dispose();
        }

        private void InitStreamWriter() {
            if (m_FileWriter != null) {
                CloseStreamWriter();
            }
            m_FileWriter = new StreamWriter(m_fullPath, File.Exists(m_fullPath), System.Text.Encoding.UTF8);
            m_FileWriter.AutoFlush = true;
        }

        private void CloseStreamWriter() {
            if (m_FileWriter == null) {
                return;
            }
            m_FileWriter.Close();
            m_FileWriter = null;
        }

        private void StartWriter() {
            m_OutputCount = 0;
            m_IsPause = false;
            m_LogQueue1.Clear();
            m_LogQueue2.Clear();
            m_LogQueue3.Clear();

            var fileName = $"Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            m_fullPath = Path.Combine(Application.persistentDataPath, fileName);
            InitStreamWriter();
            m_FileWriter.WriteLine(Utils.GetSystemInfo());

            m_ResetEvent = new ManualResetEvent(false);
            m_WriteThread = new Thread(WriteToFileThread) {
                Name = "LogToFile",
                IsBackground = true
            };
            m_WriteThread.Start();

            Application.logMessageReceivedThreaded += HandleLogThreaded;
        }

        private void EndWriter() {
            Application.logMessageReceivedThreaded -= HandleLogThreaded;

            m_ResetEvent.Dispose();
            m_ResetEvent = null;
            m_WriteThread.Abort();
            m_WriteThread = null;

            CloseStreamWriter();

            m_LogQueue1.Clear();
            m_LogQueue2.Clear();
            m_LogQueue3.Clear();
        }

        public void PauseWriter() {
            if (m_FileWriter == null || m_IsPause) {
                return;
            }

            m_IsPause = true;
            m_ResetEvent.Reset();
            CloseStreamWriter();
        }

        public void ResumeWriter() {
            if (m_FileWriter != null || !m_IsPause) {
                return;
            }

            InitStreamWriter();
            m_IsPause = false;
            m_ResetEvent.Set();
        }

        private void HandleLogThreaded(string logString, string stackTrace, LogType type) {
            m_LogQueue1.Enqueue(type);
            m_LogQueue2.Enqueue(logString);
            m_LogQueue3.Enqueue(stackTrace);
            m_QueueCount++;

            if (!m_IsPause) {
                m_ResetEvent.Set();
            }
        }

        private void WriteToFileThread() {
            while (m_ResetEvent != null) {
                m_ResetEvent.WaitOne();
                try {
                    while (m_QueueCount > 0 && !m_IsPause) {
                        if (m_FileWriter != null) {
                            if (m_LogQueue1.TryDequeue(out var str1)) {
                                m_FileWriter.WriteLine(str1);
                            }
                            if (m_LogQueue2.TryDequeue(out var str2)) {
                                m_FileWriter.WriteLine(str2);
                            }
                            if (m_LogQueue3.TryDequeue(out var str3)) {
                                m_FileWriter.WriteLine(str3);
                            }

                            if (isCheck) {
                                if (m_LogQueue1.Count != m_LogQueue2.Count ||
                                    m_LogQueue1.Count != m_LogQueue3.Count ||
                                    m_LogQueue2.Count != m_LogQueue3.Count) {
                                    Log.E("日志输入异常!!！");
                                }
                            }

                            m_QueueCount--;
                            m_OutputCount++;
                        }
                    }
                } catch (Exception e) {
                    throw e;
                } finally {
                    m_ResetEvent.Reset();
                }
            }
        }
    }
}