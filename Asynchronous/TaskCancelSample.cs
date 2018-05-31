namespace Asynchronous
{
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
            var task = Task<string>.Run(async () =>
            {
                //CancellationTokenを渡さないと、キャンセルができない。
                // return await AsynchronousMethod03();
                return await AsynchronousMethod03(source.Token);

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
            string methodName = "AsynchronousMethod03";
            Console.WriteLine($"{methodName} start");
            await Task.Delay(1000);
            Console.WriteLine($"{methodName} end");
            return "SUCCESS";
        }

        //タスクをキャンセルするためには、CancellationTokenを受け取るようにしなければならない。
        private async Task<string> AsynchronousMethod03(CancellationToken token)
        {
            string methodName = "AsynchronousMethod03";
            Console.WriteLine($"{methodName} start");
            //以下のようにすると呼び出し元からのキャンセル通知を認識し、
            //キャンセルすることができる
            //※今回は、Dlayメソッドにtokenを渡してキャンセルできるようにしている。
            //token.ThrowIfCancellationRequested();
            await Task.Delay(1000);
            Console.WriteLine($"{methodName} end");
            return "SUCCESS";
        }
    }
}
