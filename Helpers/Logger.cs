using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Helpers
{
    public enum LogType
    {
        Trace,
        Warning,
        Error
    }

    public class CustomException : Exception
    {
        public CustomException() { }

        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public interface ILogger
    {
        void Error(string msg, CustomException ex, [CallerMemberName] string area = null, string additional = null);
        void Error(string msg, Exception ex, [CallerMemberName] string area = null, string additional = null);
        void Error(string msg, string Details, [CallerMemberName] string area = null, string additional = null);
        void Trace(string msg, [CallerMemberName] string area = null, string additional = null);
        void LogException(Exception ex, [CallerMemberName] string area = null, string additional = null);
    }

    public static class Logger
    {
        public static ILogger Log { get; set; }

        public static void Error(string msg, CustomException ex, [CallerMemberName] string area = null, string additional = null)
        {
            try
            {
                Debug.WriteLine(msg);
                Debug.WriteLine(ex.Message);
                Log?.Error(msg, ex, area, additional);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public static void Error(string msg, Exception ex, [CallerMemberName] string area = null, string additional = null)
        {
            try
            {
                Debug.WriteLine(msg);
                Debug.WriteLine(ex.Message);
                Log?.Error(msg, ex, area, additional);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
        public static void Error(string msg, string Details, [CallerMemberName] string area = null, string additional = null)
        {
            try
            {
                Debug.WriteLine(msg);
                Debug.WriteLine(Details);
                Log?.Error(msg, Details, area, additional);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public static void Trace(string msg, [CallerMemberName] string area = null, string additional = null)
        {
            try
            {
                Debug.WriteLine($"{area} : {msg}");
                Log?.Trace(msg, area, additional);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public static void LogException(Exception ex, [CallerMemberName] string area = null, string additional = null)
        {
            try
            {
                Debug.WriteLine(area);
                Debug.WriteLine(ex.Message);
                Log?.Error(ex.Message, ex, area, additional);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }
}
