using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

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

        public static string GetSystemInfo(bool isReflect = false)
        {
            var sb = new StringBuilder();
            sb.AppendLine("设备信息");

            if (isReflect) {
                var type = typeof(SystemInfo);
                var propertyInfos = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
                for (var i = 0; i < propertyInfos.Length; i++) {
                    var name = propertyInfos[i].Name;
                    var str = type.InvokeMember(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, null).ToString();
                    sb.AppendLine($"{name}:\t\t{str}");
                }
            } else {
                sb.AppendLine("操作系统名称: " + SystemInfo.operatingSystem);
                sb.AppendLine("处理器名称: " + SystemInfo.processorType);
                sb.AppendLine("处理器数量: " + SystemInfo.processorCount);
                sb.AppendLine("当前系统内存大小: " + SystemInfo.systemMemorySize + "MB");
                sb.AppendLine("当前显存大小: " + SystemInfo.graphicsMemorySize + "MB");
                sb.AppendLine("显卡名字: " + SystemInfo.graphicsDeviceName);
                sb.AppendLine("显卡厂商: " + SystemInfo.graphicsDeviceVendor);
                sb.AppendLine("显卡的标识符代码: " + SystemInfo.graphicsDeviceID);
                sb.AppendLine("显卡厂商的标识符代码: " + SystemInfo.graphicsDeviceVendorID);
                sb.AppendLine("该显卡所支持的图形API版本: " + SystemInfo.graphicsDeviceVersion);
                sb.AppendLine("图形设备着色器性能级别: " + SystemInfo.graphicsShaderLevel);
                sb.AppendLine("是否支持内置阴影: " + SystemInfo.supportsShadows);
                sb.AppendLine("设备唯一标识符: " + SystemInfo.deviceUniqueIdentifier);
                sb.AppendLine("用户定义的设备的名称: " + SystemInfo.deviceName);
                sb.AppendLine("设备的模型: " + SystemInfo.deviceModel);
            }
            sb.AppendLine();

            return sb.ToString();
        }

        public static int GetTimeStamp(DateTime dt)
        {
            var dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32((dt - dateStart).TotalSeconds);
        }

        public static DateTime GetDateTime(int timeStamp)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var lTime = (long)timeStamp * 10000000;
            var toNow = new TimeSpan(lTime);
            var targetDt = dtStart.Add(toNow);
            return targetDt;
        }
    }
}