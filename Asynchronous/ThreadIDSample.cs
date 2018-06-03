namespace Asynchronous
{
    #region using
    using Asynchronous.Util;
    using System.Threading;
    #endregion

    class ThreadIDSample
    {
        public void Main()
        {
            MessageManager.WriteJobStatus("Called");
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread((O) =>
                {
                    MessageManager.WriteJobStatus("Called");
                    Thread.Sleep(500);
                });
                thread.Start();
            }
            MessageManager.WriteJobStatus("Called");

            //出力結果
            //Thread ID っ２はないのか(´・ω・`)
            //Thread ID :1 ,Main Called
            //Thread ID :3 ,Main Called
            //Thread ID :4 ,Main Called
            //Thread ID :5 ,Main Called
            //Thread ID :6 ,Main Called
            //Thread ID :7 ,Main Called
            //Thread ID :8 ,Main Called
            //Thread ID :9 ,Main Called
            //Thread ID :10 ,Main Called
            //Thread ID :11 ,Main Called
            //Thread ID :1 ,Main Called
            //Thread ID :12 ,Main Called
        }
    }
}
