namespace AsynchronousUWP.Util
{
    #region using
    using System;
    using System.Threading;
    #endregion

    public static class MessageManager
    {
        #region public

        public static string WriteStart([System.Runtime.CompilerServices.CallerMemberName]string methodName = "")
        {
            return ConsoleWriteLineCore(methodName, "Start");

        }

        public static string WriteEnd([System.Runtime.CompilerServices.CallerMemberName]string methodName = "")
        {
            return ConsoleWriteLineCore(methodName, "End");
        }

        public static string WriteJobStatus(string message, [System.Runtime.CompilerServices.CallerMemberName]string methodName = "")
        {
            return ConsoleWriteLineCore(methodName, message);
        }

        #endregion

        private static string ConsoleWriteLineCore(string methodName, string message)
        {
            return $"Thread ID :{Thread.CurrentThread.ManagedThreadId} ,{methodName} {message}";
        }
    }
}
