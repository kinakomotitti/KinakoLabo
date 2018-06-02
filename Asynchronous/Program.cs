namespace Asynchronous
{
    #region using
    using System;
    using System.Threading.Tasks;
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            //「Async/Await実装サンプル」の呼び出し
            var asaw = new AsyncAwaitSample();
            asaw.Main();

            //「Threadの実行サンプル」呼び出し
            //var ts = new ThreadStartSample();
            //ts.Main();

            //「ThreadPool.QueueUserWorkItemの実行サンプル」呼び出し
            //var qs = new QueueUserWorkItemSample();
            //qs.Main();

            //「ThreadPool.QueueUserWorkItemのキャンセルのサンプル」呼び出し
            //var qcs = new QueueUserWorkItemCancelSample();
            //qcs.Main();

            //「Taskのキャンセルのサンプル」呼び出し
            //var tcs = new TaskCancelSample();
            //tcs.Main();
            
            //「Taskの実行（Run）のサンプル」呼び出し
            //var trs = new TaskRunSample();
            //trs.Main();
#if DEBUG
            //Task、ThreadPoolはフォアグラウンドスレッドで動作するため、
            //ここに到達した時点でまだ処理が継続していても
            //強制的に処理が終了される。
            System.Diagnostics.Debugger.Break();
#endif
        }
    }
}