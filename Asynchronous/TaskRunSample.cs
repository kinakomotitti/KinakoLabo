namespace Asynchronous
{
    #region using
    using System;
    using System.Threading.Tasks;
    #endregion

    class TaskRunSample
    {
        public void Main()
        {
            Task.Run(() =>
            {
                //ここではawaitできない
            }).Wait();

            Task.Run(async () =>
            {
                //voidは待機できない。Taskの実行完了を知る術がない。
                this.AsynchronousMethod01();

                //待機できるが、戻り値はvoidとして扱われる
                await this.AsynchronousMethod02();

                //Task<T>はT型の戻り値を取得できる
                string taskResult = await this.AsynchronousMethod03();
            });
        }

        private  async void AsynchronousMethod01()
        {
            string methodName = "AsynchronousMethod01";
            Console.WriteLine($"{methodName} start");
            await Task.Delay(1000);
            Console.WriteLine($"{methodName} end");
        }
        private  async Task AsynchronousMethod02()
        {
            string methodName = "AsynchronousMethod02";
            Console.WriteLine($"{methodName} start");
            await Task.Delay(1000);
            Console.WriteLine($"{methodName} end");
        }
        private  async Task<string> AsynchronousMethod03()
        {
            string methodName = "AsynchronousMethod03";
            Console.WriteLine($"{methodName} start");
            await Task.Delay(1000);
            Console.WriteLine($"{methodName} end");
            return string.Empty;
        }
    }
}
