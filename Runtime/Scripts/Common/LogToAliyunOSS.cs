using Aliyun.OSS;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using ZGH.Core;

public class LogToAliyunOSS : MonoBehaviour
{
    public static string Endpoint = "oss-cn-qingdao.aliyuncs.com";
    public static string BucketName = "ar-scenic-game";
    public static string AccessKeyId = "LTAI5tNd9jm3pyhzfDXFrjuv";
    public static string AccessKeySecret = "j8rn6DBJ8z1M1wKHiWN919xtsGFT0k";

    public static OssClient m_Client;
    public static long m_LastPos = 0;
    public static string objectName;
    public int count;

    private void OnEnable()
    {
        count = 0;
        var fileName = $"Log_{DateTime.Now:yyyymmddhhmmss}.txt";
        objectName = $"Log/{fileName}";
        LogMessageReceivedMgr.Instance.onWriteEvent += AppendObjectWithString;
    }

    private void OnDisable()
    {
        LogMessageReceivedMgr.Instance.onWriteEvent -= AppendObjectWithString;
    }

    private void AppendObjectWithString(string fileContent)
    {
        try
        {
            m_Client ??= new OssClient(Endpoint, AccessKeyId, AccessKeySecret);
            var b = Encoding.UTF8.GetBytes(fileContent);
            using (Stream fs = new MemoryStream(b))
            {
                var request = new AppendObjectRequest(BucketName, objectName)
                {
                    ObjectMetadata = new(),
                    Content = fs,
                    Position = m_LastPos
                };
                var result = m_Client.AppendObject(request);
                m_LastPos = result.NextAppendPosition;
                count++;
            }
        } catch (Exception e) { throw e; }
    }
}