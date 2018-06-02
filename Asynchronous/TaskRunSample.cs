namespace Asynchronous
{
    #region using
    using Asynchronous.Util;
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

        private async void AsynchronousMethod01()
        {
            MessageManager.WriteStart();
            await Task.Delay(1000);
            MessageManager.WriteEnd();
        }
        private async Task AsynchronousMethod02()
        {
            MessageManager.WriteStart();
            await Task.Delay(1000);
            MessageManager.WriteEnd();
        }
        private async Task<string> AsynchronousMethod03()
        {
            MessageManager.WriteStart();
            await Task.Delay(1000);
            MessageManager.WriteEnd();
            return string.Empty;
        }
    }
}
