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

        private Task<int> SampleTask = null;
        private CancellationTokenSource source = null;
        private void Execute_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.source = new CancellationTokenSource();
            this.SampleTask =  Task.Run<int>(() =>
            {
                return SampleMethod(this.source.Token);
            });


            this.SampleTask.ContinueWith((Task,param) =>
            {
                
                this.TaskJobStatus_TextBlock.Text = MessageManager.WriteJobStatus("OnlyOnFaulted");
            }, this.source.Token, TaskScheduler.FromCurrentSynchronizationContext());

            //TaskScheduler.Defaultを指定すると、ThradPoolのスレッドでタスクが実行されるため、以下の例外がスローされる。
            //System.Exception: 'アプリケーションは、別のスレッドにマーシャリングされたインターフェイスを呼び出しました。
            //this.SampleTask.ContinueWith((Task) =>
            //{
            //    this.TaskJobStatus_TextBlock.Text = "canceled";
            //},this.source.Token, TaskContinuationOptions.OnlyOnCanceled, TaskScheduler.Default);

            this.SampleTask.ContinueWith((Task) =>
            {
                this.TaskJobStatus_TextBlock.Text = MessageManager.WriteJobStatus("RanToCompletion");
            }, this.source.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void Cancel_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.source.Cancel();
        }
        private int SampleMethod(CancellationToken token)
        {
            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                Task.Run(async () =>
                {
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        //ユーザーインターフェースを操作する
                        this.TaskStatus_TextBlock.Text = $"ほげほげ{i}";
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

