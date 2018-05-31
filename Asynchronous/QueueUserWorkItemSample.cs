namespace Asynchronous
{
    #region using
    using System;
    using System.Threading;
    #endregion

    class QueueUserWorkItemSample
    {
        public object QueueUserWorkItem { get; private set; }

        public void Main()
        {
            //QueueUserWorkItemを使うことで、シンプルなコードでThreadPoolを利用することができる。
            //しかし、戻り値を取得するのは難しい。
            ThreadPool.QueueUserWorkItem(AsynchronousMethod01);
            Thread.Sleep(2000);
        }

        private void AsynchronousMethod01(object state)
        {
            string methodName = "AsynchronousMethod01";
            Console.WriteLine($"{methodName} start");
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine($"{methodName}  : {i}");
            }
            Console.WriteLine($"{methodName} end");
        }
    }
}