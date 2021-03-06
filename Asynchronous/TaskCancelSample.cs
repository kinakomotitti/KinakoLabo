﻿namespace Asynchronous
{
    using Asynchronous.Util;
    #region using
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    class TaskCancelSample
    {
        /// <summary>
        /// Taskを使ってキャンセルする処理のサンプル
        /// </summary>
        public void Main()
        {
            var source = new CancellationTokenSource();
            var task = Task<string>.Run( () =>
            {
                //CancellationTokenを渡さないと、キャンセルができない。
                // return await AsynchronousMethod03();
                return  AsynchronousMethod03(source.Token);

            }, source.Token);

            Thread.Sleep(500);//タスクを実行中にするために呼び出し元スレッドを一時停止する

            //※キャンセルをコメントアウトすると、"SUCCESS"がコンソールに出力される
            source.Cancel();

            try
            {
                //キャンセルすると、System.Threading.CancellationTokenをスロー。
                Console.WriteLine(task.Result.ToString());
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //キャンセル処理が実装されていないため、キャンセル前に実行がスケジュールされると
        //キャンセルすることができない。
        private async Task<string> AsynchronousMethod03()
        {
            MessageManager.WriteStart();
            await Task.Delay(1000);
            MessageManager.WriteEnd();
            return "SUCCESS";
        }

        //タスクをキャンセルするためには、CancellationTokenを受け取るようにしなければならない。
        private async Task<string> AsynchronousMethod03(CancellationToken token)
        {
            MessageManager.WriteStart();
            //以下のようにすると呼び出し元からのキャンセル通知を認識し、
            //キャンセルすることができる。
            //tokenに対して取り消しが要求された場合、System.OperationCanceledException をスローする。
            token.ThrowIfCancellationRequested();

            //※今回は、Dlayメソッドにtokenを渡してキャンセルできるようにしている。
            await Task.Delay(1000,token);

            //（参考）MSDNのサンプルでは以下のように実装されていた。
            //if (token.IsCancellationRequested)
            //{
            //    MessageManager.WriteJobStatus("Task AsynchronousMethod03 cancelled");
            //    //※必ずしもThrowしないといけないわけではない。場合によってはreturnするだけでも良い。
            //    token.ThrowIfCancellationRequested();
            //}

            MessageManager.WriteEnd();
            return "SUCCESS";
        }
    }
}
