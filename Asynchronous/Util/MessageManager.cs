namespace Asynchronous.Util
{
    #region using
    using System;
    using System.Threading;
    #endregion

    public static class MessageManager
    {
        #region public

        public static void WriteStart([System.Runtime.CompilerServices.CallerMemberName]string methodName = "")
        {
            ConsoleWriteLineCore(methodName,"Start");

        }

        public static void WriteEnd([System.Runtime.CompilerServices.CallerMemberName]string methodName = "")
        {
            ConsoleWriteLineCore(methodName,"End");
        }

        public static void WriteJobStatus(string message,[System.Runtime.CompilerServices.CallerMemberName]string methodName = "")
        {
            ConsoleWriteLineCore(methodName, message);
        }

        #endregion

        private static void ConsoleWriteLineCore(string methodName,string message)
        {
            Console.WriteLine($"Thread ID :{Thread.CurrentThread.ManagedThreadId} ,{methodName} {message}");
        }
    }
}
