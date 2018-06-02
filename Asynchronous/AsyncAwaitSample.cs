namespace Asynchronous
{
    #region using
    using Asynchronous.Util;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    class AsyncAwaitSample
    {
        public void Main()
        {
            //Step1)Thread ID =1(仮)で実行
            AwaitJob();　//Awaitのため、一部処理が実行されない。

            //Step３)Thread ID =1(仮)で実行
            NoAwaitJob(); //すべて処理が実行される

            //結果を見るため、待機する。
            Thread.Sleep(10000);

            //出力結果
            //Thread ID :1 ,AwaitJob Start　　←同期処理と同じように実行された
            //Thread ID :1 ,NoAwaitJob Start　←同期処理と同じように実行された
            //Thread ID :1 ,NoAwaitJob End　　←同期処理と同じように実行された
            //Thread ID :4 ,AwaitJob End　　　←別スレッドで実行された
        }

        private async Task AwaitJob()
        {
            MessageManager.WriteStart();
            //Step２)Thread ID =1(仮)で実行するが、Delay待ちのため、処理を待機する
            await Task.Delay(1000);

            //Delay後、ThreadIDが１ではないスレッドで処理を実行
            MessageManager.WriteEnd();
        }

        private async Task NoAwaitJob()
        {
            //Step３)Thread ID =1(仮)で実行
            MessageManager.WriteStart();
            Thread.Sleep(1000);
            MessageManager.WriteEnd();
        }
    }
}
