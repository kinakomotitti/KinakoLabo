namespace Asynchronous
{
    #region using
    using Asynchronous.Util;
    using System.Threading;
    #endregion

    class ThreadStartSample
    {
        public void Main()
        {
            //Threadクラスのインスタンスを生成
            //実行する処理を登録
            Thread thread = new Thread(AsynchronousMethod01);
            MessageManager.WriteStart();

            //Threadをフォアグラウンドで実行(デフォルトはバックグラウンド)
            thread.IsBackground = false;

            //処理実行
            thread.Start();

            MessageManager.WriteEnd();
        }

        private void AsynchronousMethod01()
        {
            MessageManager.WriteStart();
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(1000);
                MessageManager.WriteJobStatus($"LoopCount: { i}");
            }
            MessageManager.WriteEnd();
        }
    }
}
