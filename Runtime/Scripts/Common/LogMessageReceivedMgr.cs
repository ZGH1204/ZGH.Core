using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ZGH.Core
{
    public class LogMessageReceivedMgr : Singleton<LogMessageReceivedMgr>
    {
        public Action<string> onWriteEvent;

        private ConcurrentQueue<LogOutput> m_OutputQueue = new();
        private Thread m_WriteThread;
        private ManualResetEvent m_ManualResetEvent;
        private StringBuilder m_sb = new();

        public LogMessageReceivedMgr() : base()
        {
            Init();
        }

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
            onWriteEvent = null;
            m_ManualResetEvent = new ManualResetEvent(false);
            m_WriteThread = new Thread(WriteToFileThread);
            m_WriteThread.Name = "LogMessageReceivedMgr";
            m_WriteThread.Start();

            Application.logMessageReceivedThreaded += HandleLogThreaded;
        }

        private void EndWriter()
        {
            onWriteEvent = null;
            Application.logMessageReceivedThreaded -= HandleLogThreaded;

            m_ManualResetEvent.Dispose();
            m_ManualResetEvent = null;
            m_WriteThread.Abort();
            m_WriteThread = null;
        }

        private void HandleLogThreaded(string logString, string stackTrace, LogType type)
        {
            var output = LogOutput.Get();
            output.stack = stackTrace;
            //output.time = $"[Frame:{Time.frameCount}] {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            output.time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            output.log = $"{logString}";
            output.type = type.ToString();
            m_OutputQueue.Enqueue(output);
            m_ManualResetEvent.Set();
        }

        private void WriteToFileThread()
        {
            while (m_ManualResetEvent != null)
            {
                m_ManualResetEvent.WaitOne();
                try
                {
                    while (m_OutputQueue.Count > 0 && m_OutputQueue.TryDequeue(out var output))
                    {
                        m_sb.AppendLine("Time: " + output.time);
                        m_sb.AppendLine($"{output.type}: {output.log}");
                        m_sb.AppendLine("Stack:\n" + output.stack);
                        var str = m_sb.ToString();
                        onWriteEvent?.Invoke(str);

                        m_sb.Clear();
                        LogOutput.Release(output);
                    }
                } catch (Exception e)
                {
                    throw e;
                } finally
                {
                    m_ManualResetEvent.Reset();
                    Thread.Sleep(1000);
                }
            }
        }

        public int GetOutputQueueCount()
        {
            return m_OutputQueue.Count;
        }

        [Serializable]
        private class LogOutput
        {
            public string type;
            public string time;
            public string log;
            public string stack;

            public static LogOutput Get()
            {
                var inst = UnityEngine.Pool.GenericPool<LogOutput>.Get();

                inst.type = null;
                inst.time = null;
                inst.log = null;
                inst.stack = null;
                return inst;
            }

            public static void Release(LogOutput inst)
            {
                UnityEngine.Pool.GenericPool<LogOutput>.Release(inst);
            }
        }
    }
}