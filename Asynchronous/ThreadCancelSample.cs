namespace Asynchronous
{
    #region using
    using Asynchronous.Util;
    using System.Threading;
    #endregion

    class ThreadCancelSample
    {
        private static bool IsCanceled = false;

        public void Main()
        {
            #region 　パターン１

            //キャンセル処理　パターン１：その１
            Thread thread1 = new Thread(AsynchronousMethod01);
            thread1.Start();
            Thread.Sleep(1000); //動作確認のため一時停止
            MessageManager.WriteJobStatus("Call Cancel");

            //Static変数を利用してキャンセルを実行する
            IsCanceled = true;

            Thread.Sleep(1000); //動作確認のため一時停止
            //出力結果
            //Thread ID :3 ,AsynchronousMethod01 Start
            //Thread ID :3 ,AsynchronousMethod01 LoopCount: 0
            //Thread ID :1 ,Main Call Cancel
            //Thread ID: 3 ,AsynchronousMethod01 LoopCount: 1　←キャンセルしてすぐに終了されない。
            //Thread ID :3 ,AsynchronousMethod01 Canceled

            //キャンセル処理　パターン１：その２
            Thread thread2 = new Thread(AsynchronousMethod01);
            IsCanceled = false;
            thread2.Start();
            Thread.Sleep(1000); //動作確認のため一時停止
            MessageManager.WriteJobStatus("Call Cancel");

            //スレッドを強制終了
            thread2.Abort();

            Thread.Sleep(1000); //動作確認のため一時停止

            //出力結果
            //Thread ID :4 ,AsynchronousMethod01 Start
            //Thread ID :4 ,AsynchronousMethod01 LoopCount: 0　←強制終了されているので、即時停止する
            //Thread ID :1 ,Main Call Cancel

            #endregion

            #region パターン２

            //キャンセル処理　パターン２
            CancellationTokenSource source = new CancellationTokenSource();
            Thread thread3 = new Thread(() => AsynchronousMethod01(source.Token));
            thread3.Start();
            Thread.Sleep(1000); //動作確認のため一時停止
            MessageManager.WriteJobStatus("Call Cancel");

            try
            {
                //キャンセルする
                source.Cancel();
            }
            catch (System.Exception e)
            {
                MessageManager.WriteJobStatus(e.Message);
            }


            Thread.Sleep(1000); //動作確認のため一時停止
            //出力結果
            //Thread ID :5 ,AsynchronousMethod01 Start
            //Thread ID :5 ,AsynchronousMethod01 LoopCount: 0
            //Thread ID :5 ,AsynchronousMethod01 Canceled
            //Thread ID :1 ,Main Call Cancel

            #endregion
        }

        /// <summary>
        /// パターン１
        /// </summary>
        private void AsynchronousMethod01()
        {
            MessageManager.WriteStart();
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(500);
                MessageManager.WriteJobStatus($"LoopCount: { i}");
                //キャンセル処理を明示的に実装する
                if (IsCanceled)
                {
                    MessageManager.WriteJobStatus("Canceled");
                    return;
                }
            }
            MessageManager.WriteEnd();
        }

        /// <summary>
        /// パターン２
        /// </summary>
        /// <param name="token"></param>
        private void AsynchronousMethod01(CancellationToken token)
        {
            MessageManager.WriteStart();
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(500);
                MessageManager.WriteJobStatus($"LoopCount: { i}");

                //キャンセル処理を明示的に実装する
                if (token.CanBeCanceled)
                {
                    MessageManager.WriteJobStatus("Canceled");
                    //ハンドルできないエラーが発生するので、コメントアウト。
                    //token.ThrowIfCancellationRequested();
                    return;
                }
            }
            MessageManager.WriteEnd();
        }
    }
}