namespace Asynchronous
{
    using Asynchronous.Util;
    #region using
    using System;
    using System.Threading;
    #endregion

    class QueueUserWorkItemCancelSample
    {
        public object QueueUserWorkItem { get; private set; }

        public void Main()
        {
            //キャンセルできない
            ThreadPool.QueueUserWorkItem(o => AsynchronousMethod01(o));

            //キャンセルできる
            var source = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(O => AsynchronousMethod01(O, source.Token));

            Thread.Sleep(1000);//タスク実行を待つため、１秒待機
            source.Cancel();

            MessageManager.WriteJobStatus("Cancel");
            Thread.Sleep(2000);//キャンセルできなかったタスクの実行を確認するため待機。
        }

        //キャンセル処理が実装されていないため、キャンセル前に実行がスケジュールされると
        //キャンセルすることができない。
        private void AsynchronousMethod01(object state)
        {
            MessageManager.WriteStart();
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(100);
                MessageManager.WriteJobStatus($"LoopCount: { i}");
            }
            MessageManager.WriteEnd();

        }

        //タスクをキャンセルするためには、CancellationTokenを受け取るようにしなければならない。
        private void AsynchronousMethod01(object state, CancellationToken token)
        {
            MessageManager.WriteStart();

            //以下のようにすると呼び出し元からのキャンセル通知を認識し、
            //キャンセルすることができる
            //※今回は、Dlayメソッドにtokenを渡してキャンセルできるようにしている。
            //token.ThrowIfCancellationRequested();
            for (int i = 0; i < 20; i++)
            {
                if (token.IsCancellationRequested) return;
                Thread.Sleep(100);
                MessageManager.WriteJobStatus($"LoopCount: { i}");
            }
            MessageManager.WriteEnd();
        }
    }
}
