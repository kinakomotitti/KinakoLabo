using AsynchronousUWP.Util;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace AsynchronousUWP.Views
{
    public sealed partial class AsynchronousPage : Page, INotifyPropertyChanged
    {
        #region 自動生成

        public AsynchronousPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        private Task<int> SampleTask = null;
        private CancellationTokenSource source = null;
        private void Execute_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.source = new CancellationTokenSource();
            this.SampleTask = Task.Run<int>(() =>
           {
               return SampleMethod(this.source.Token);
           }, this.source.Token);

            //キャンセルのときだけ実行
            this.SampleTask.ContinueWith((task, status) =>
            {

                this.TaskJobStatus_TextBlock.Text = MessageManager.WriteJobStatus("OnlyOnCanceled");
            }, new Object(),
               CancellationToken.None,
               TaskContinuationOptions.OnlyOnCanceled,
               TaskScheduler.FromCurrentSynchronizationContext());

            //タスクが完了したときだけ実行
            this.SampleTask.ContinueWith((task, status) =>
            {
                this.TaskJobStatus_TextBlock.Text = MessageManager.WriteJobStatus("OnlyOnRanToCompletion");
                this.TaskJobStatus_TextBlock.Text += $"\r\nResult : {task.Result.ToString()}";
            }, new Object(),
               CancellationToken.None,
               TaskContinuationOptions.OnlyOnRanToCompletion,
               TaskScheduler.FromCurrentSynchronizationContext());

            ////TaskScheduler.Defaultを指定すると、ThradPoolのスレッドでタスクが実行されるため、以下の例外がスローされる。
            ////System.Exception: 'アプリケーションは、別のスレッドにマーシャリングされたインターフェイスを呼び出しました。
            //this.SampleTask.ContinueWith((task,status) =>
            //{
            //    this.TaskJobStatus_TextBlock.Text = "canceled";
            //}, new Object(),
            //   CancellationToken.None,
            //   TaskContinuationOptions.OnlyOnRanToCompletion,
            //   TaskScheduler.Default);
        }

        private void Cancel_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.source != null)
                this.source.Cancel();
        }
        private int SampleMethod(CancellationToken token)
        {
            int sum = 0;
            int length = 10;
            for (int i = 0; i < length; i++)
            {
                string message = MessageManager.WriteJobStatus($"計算中：{i + 1}/{length}");
                Task.Run(async () =>
                {
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        //ユーザーインターフェースを操作する
                        this.TaskStatus_TextBlock.Text = message;
                    });
                });
                Thread.Sleep(1000);
                sum += i;
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
            }
            return sum;
        }
    }
}

