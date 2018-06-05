using Asynchronous.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Asynchronous
{
    class HowToUseTask
    {
        public void Main()
        {
            MessageManager.WriteStart();

            ParentAndChildTask();



            Thread.Sleep(50000);
            MessageManager.WriteEnd();
        }
        #region Run

        private void TaskRun()
        {
            //Task.Run
            Task.Run(() =>
            {
                MessageManager.WriteStart();
                for (int i = 0; i < 10; i++)
                {
                    MessageManager.WriteJobStatus($"Count : {i}");
                }
                MessageManager.WriteEnd();
            });
        }

        #endregion

        #region Start

        private void TaskStart()
        {
            //Task.Start
            Task task = new Task(() =>
            {
                MessageManager.WriteStart();
                for (int i = 0; i < 10; i++)
                {
                    MessageManager.WriteJobStatus($"Count : {i}");
                }
                MessageManager.WriteEnd();
            });


            //何かしら別の処理


            //タスクを実行（インスタンスを生成してから何かの処理を挟んでの実行ができる）
            task.Start();
        }

        #endregion

        #region ContinueWith

        private void TaskContinueWithPattern1()
        {
            MessageManager.WriteStart();
            //Task.Run
            Task task = Task.Run(() => Sample("Continue Task　(1)"));

            //ContinueWithの戻り値で、新しいTaskのインスタンスを受け取ることができる。
            //この戻り値で受け取ったTaskに対しても、完了後に実行するTaskを登録することが可能。
            //この場合、一つ目のtaskが完了した後、(2) -> (3) -> (4)という順番で実行される。
            var task_ = task.ContinueWith((t) => Sample("Continue Task　(2)"));
            var task__ = task_.ContinueWith((t) => Sample("Continue Task　(3)"));
            var task___ = task__.ContinueWith((t) => Sample("Continue Task　(4)"));

            //最後のTaskが完了するまで待機
            task___.Wait();
            MessageManager.WriteEnd();

            //出力例
            //Thread ID :1 ,TaskContinueWithPattern1 Start
            //Thread ID :3 ,Sample Start　                       ←task
            //Thread ID :3 ,Sample Continue Task(1), Count: 0
            //Thread ID :3 ,Sample Continue Task(1), Count: 1
            //Thread ID :3 ,Sample Continue Task(1), Count: 2
            //Thread ID :3 ,Sample Continue Task(1), Count: 3
            //Thread ID :3 ,Sample End
            //Thread ID :4 ,Sample Start　                       ←task_
            //Thread ID :4 ,Sample Continue Task(2), Count: 0
            //Thread ID :4 ,Sample Continue Task(2), Count: 1
            //Thread ID :4 ,Sample Continue Task(2), Count: 2
            //Thread ID :4 ,Sample Continue Task(2), Count: 3
            //Thread ID :4 ,Sample End
            //Thread ID :4 ,Sample Start　                       ←task__
            //Thread ID :4 ,Sample Continue Task(3), Count: 0
            //Thread ID :4 ,Sample Continue Task(3), Count: 1
            //Thread ID :4 ,Sample Continue Task(3), Count: 2
            //Thread ID :4 ,Sample Continue Task(3), Count: 3
            //Thread ID :4 ,Sample End
            //Thread ID :4 ,Sample Start　                       ←task___
            //Thread ID :4 ,Sample Continue Task(4), Count: 0
            //Thread ID :4 ,Sample Continue Task(4), Count: 1
            //Thread ID :4 ,Sample Continue Task(4), Count: 2
            //Thread ID :4 ,Sample Continue Task(4), Count: 3
            //Thread ID :4 ,Sample End
            //Thread ID :1 ,TaskContinueWithPattern1 End        ←task___が終わるのを待って完了
        }

        private void TaskContinueWithPattern2()
        {
            MessageManager.WriteStart();

            //Task.Run
            Task task = Task.Run(() => Sample("Continue Task　(1)"));

            //ContinueWithの戻り値で、新しいTaskのインスタンスを受け取ることができる。
            //このインスタンスを無視し、元のtaskに対してtaskを追加していくことも可能。
            //この場合、一つ目のtaskが完了したときに、ThreadPoolに対し、
            //登録されていたのこりのタスクがすべて登録される。
            task.ContinueWith((t) => Sample("Continue Task　(2)"));
            task.ContinueWith((t) => Sample("Continue Task　(3)"));
            task.ContinueWith((t) => Sample("Continue Task　(4)"));

            //最後のTaskが完了するまで待機
            task.Wait();//←ここでは、1つ目のタスクが完了するのだけを待機することになる。(※１)
            MessageManager.WriteEnd();

            //出力例
            //Thread ID :1 ,TaskContinueWithPattern2 Start
            //Thread ID :3 ,Sample Start                        ←task
            //Thread ID :3 ,Sample Continue Task(1), Count: 0
            //Thread ID :3 ,Sample Continue Task(1), Count: 1
            //Thread ID :3 ,Sample Continue Task(1), Count: 2
            //Thread ID :3 ,Sample Continue Task(1), Count: 3
            //Thread ID :3 ,Sample End
            //Thread ID :1 ,TaskContinueWithPattern2 End        ←(※１)
            //Thread ID :3 ,Sample Start                        ←taskの完了を待って、その後に実行される処理（2）~(4)が
            //Thread ID :4 ,Sample Start                        ←一斉にThread Poolに投入される。
            //Thread ID :5 ,Sample Start                        ←３つタスクを登録したので、３スレッドが一斉に起動する。
            //Thread ID :5 ,Sample Continue Task(3), Count: 0
            //Thread ID :5 ,Sample Continue Task(3), Count: 1
            //Thread ID :5 ,Sample Continue Task(3), Count: 2
            //Thread ID :5 ,Sample Continue Task(3), Count: 3
            //Thread ID :5 ,Sample End
            //Thread ID :3 ,Sample Continue Task(4), Count: 0
            //Thread ID :3 ,Sample Continue Task(4), Count: 1
            //Thread ID :4 ,Sample Continue Task(2), Count: 0
            //Thread ID :4 ,Sample Continue Task(2), Count: 1
            //Thread ID :4 ,Sample Continue Task(2), Count: 2
            //Thread ID :4 ,Sample Continue Task(2), Count: 3
            //Thread ID :3 ,Sample Continue Task(4), Count: 2
            //Thread ID :3 ,Sample Continue Task(4), Count: 3
            //Thread ID :3 ,Sample End
            //Thread ID :4 ,Sample End
        }

        private void TaskContinueWithPattern3()
        {
            MessageManager.WriteStart();

            //いろいろなオプションを指定することができる
            //その１:ExecuteSynchronously
            //このオプションを指定すると、継続は、前のタスクを最終状態に遷移させた同じスレッドで実行されます。
            Task task = Task.Run(() => Sample("Continue Task　(1)"));
            task.ContinueWith((t) => Sample("Continue Task　(2)"), TaskContinuationOptions.ExecuteSynchronously);
            task.ContinueWith((t) => Sample("Continue Task　(3)"), TaskContinuationOptions.ExecuteSynchronously);
            task.ContinueWith((t) => Sample("Continue Task　(4)"), TaskContinuationOptions.ExecuteSynchronously);

            //最後のTaskが完了するまで待機
            task.Wait();
            MessageManager.WriteEnd();

            //出力例
            //Thread ID :1 ,TaskContinueWithPattern3 Start
            //Thread ID :6 ,Sample Start
            //Thread ID :6 ,Sample Continue Task(1), Count: 0
            //Thread ID :6 ,Sample Continue Task(1), Count: 1
            //Thread ID :6 ,Sample Continue Task(1), Count: 2
            //Thread ID :6 ,Sample Continue Task(1), Count: 3
            //Thread ID :6 ,Sample End
            //Thread ID :1 ,TaskContinueWithPattern3 End
            //Thread ID :6 ,Sample Start                        ←オプションなしだと３つスレッドが立ち上がったが、すべて同じスレッドで実行されるようになった。
            //Thread ID :6 ,Sample Continue Task(2), Count: 0 
            //Thread ID :6 ,Sample Continue Task(2), Count: 1
            //Thread ID :6 ,Sample Continue Task(2), Count: 2
            //Thread ID :6 ,Sample Continue Task(2), Count: 3
            //Thread ID :6 ,Sample End
            //Thread ID :6 ,Sample Start
            //Thread ID :6 ,Sample Continue Task(3), Count: 0
            //Thread ID :6 ,Sample Continue Task(3), Count: 1
            //Thread ID :6 ,Sample Continue Task(3), Count: 2
            //Thread ID :6 ,Sample Continue Task(3), Count: 3
            //Thread ID :6 ,Sample End
            //Thread ID :6 ,Sample Start
            //Thread ID :6 ,Sample Continue Task(4), Count: 0
            //Thread ID :6 ,Sample Continue Task(4), Count: 1
            //Thread ID :6 ,Sample Continue Task(4), Count: 2
            //Thread ID :6 ,Sample Continue Task(4), Count: 3
            //Thread ID :6 ,Sample End
        }

        private void TaskContinueWithPattern4()
        {
            MessageManager.WriteStart();

            //いろいろなオプションを指定することができる
            //その２：NotOnRanToCompletion
            //このオプションを指定すると、前のタスクが完了まで実行された場合は、継続タスクをスケジュールしないように指定します。
            CancellationTokenSource source = new CancellationTokenSource();
            Task task = Task.Run(() => Sample("Continue Task　(1)"), source.Token);
            task.ContinueWith((t) => Sample("Continue Task　(2)"), TaskContinuationOptions.NotOnRanToCompletion);
            task.ContinueWith((t) => Sample("Continue Task　(3)"), TaskContinuationOptions.NotOnRanToCompletion);
            task.ContinueWith((t) => Sample("Continue Task　(4)"), TaskContinuationOptions.NotOnRanToCompletion);

            MessageManager.WriteEnd();

            //出力例
            //Thread ID :1 ,Main Start
            //Thread ID :1 ,TaskContinueWithPattern5 Start
            //Thread ID :1 ,TaskContinueWithPattern5 End                    ←元のタスクが正常に実行されたので、(2)~(4)のタスクは実行されない。
        }

        private void TaskContinueWithPattern5()
        {
            MessageManager.WriteStart();

            //いろいろなオプションを指定することができる
            //その３：NotOnCanceled
            //このオプションを指定すると、前のタスクが取り消された場合は継続タスクをスケジュールしないように指定します。

            //その４：OnlyOnCanceled
            //このオプションを指定すると、継続元が取り消された場合にのみ継続をスケジュールするように指定します。
            CancellationTokenSource source = new CancellationTokenSource();
            Task task = Task.Run(() => Sample("Continue Task　(1)"), source.Token);
            source.Cancel();
            task.ContinueWith((t) => Sample("Continue Task　(2)"), TaskContinuationOptions.NotOnCanceled);
            task.ContinueWith((t) => Sample("Continue Task　(3)"), TaskContinuationOptions.OnlyOnCanceled);
            task.ContinueWith((t) => Sample("Continue Task　(4)"), TaskContinuationOptions.NotOnCanceled);

            MessageManager.WriteEnd();

            //出力例
            //Thread ID :1 ,Main Start
            //Thread ID :1 ,TaskContinueWithPattern5 Start
            //Thread ID :1 ,TaskContinueWithPattern5 End        ←ここの前に元のタスクをキャンセル
            //Thread ID :4 ,Sample Start
            //Thread ID :4 ,Sample Continue Task(3), Count: 0   ←元のタスクをキャンセルしたときだけ実行されるタスクだけ動く。
            //Thread ID :4 ,Sample Continue Task(3), Count: 1
            //Thread ID :4 ,Sample Continue Task(3), Count: 2
            //Thread ID :4 ,Sample Continue Task(3), Count: 3
            //Thread ID :4 ,Sample End
        }

        #endregion

        #region 親子

        private void ParentAndChildTask()
        {
            var parent = new Task(() =>
            {
                new Task(() =>
                {
                    for (int i = 0; i < 4; i++) { MessageManager.WriteJobStatus($"(1) Count : {i}"); }
                }, TaskCreationOptions.AttachedToParent).Start();
                new Task(() =>
                {
                    for (int i = 0; i < 4; i++) { MessageManager.WriteJobStatus($"(2) Count : {i}"); }
                }, TaskCreationOptions.AttachedToParent).Start();

                //3つ目、4つ目のタスクは、Thread.Sleepで重たい処理をシミュレート
                new Task(() =>
                {
                    for (int i = 0; i < 4; i++) { Thread.Sleep(500); MessageManager.WriteJobStatus($"(3) Count : {i}"); }
                }, TaskCreationOptions.AttachedToParent).Start();
                //4つ目のタスクだけは、親タスクにアタッチしない（子タスクとして認識されない）ように設定
                new Task(() =>
                {
                    for (int i = 0; i < 4; i++) { Thread.Sleep(1000); MessageManager.WriteJobStatus($"(4) Count : {i}"); }
                }).Start();
            });

            parent.ContinueWith((t) => { MessageManager.WriteJobStatus("Complete!"); });
            parent.Start();

            //出力例
            //Thread ID :1 ,Main Start
            //Thread ID :4 ,ParentAndChildTask(1) Count: 0
            //Thread ID :4 ,ParentAndChildTask(1) Count: 1
            //Thread ID :4 ,ParentAndChildTask(1) Count: 2
            //Thread ID :4 ,ParentAndChildTask(1) Count: 3
            //Thread ID :6 ,ParentAndChildTask(2) Count: 0
            //Thread ID :6 ,ParentAndChildTask(2) Count: 1
            //Thread ID :6 ,ParentAndChildTask(2) Count: 2
            //Thread ID :6 ,ParentAndChildTask(2) Count: 3
            //Thread ID :4 ,ParentAndChildTask(3) Count: 0
            //Thread ID :3 ,ParentAndChildTask(4) Count: 0
            //Thread ID :4 ,ParentAndChildTask(3) Count: 1
            //Thread ID :4 ,ParentAndChildTask(3) Count: 2
            //Thread ID :3 ,ParentAndChildTask(4) Count: 1
            //Thread ID :4 ,ParentAndChildTask(3) Count: 3
            //Thread ID :5 ,ParentAndChildTask Complete!            ←（3）までは子として認識されているので、子タスクが完了した後に指定したタスクが実行される。
            //Thread ID: 3 ,ParentAndChildTask(4) Count: 2          ←（4）は無視。
            //Thread ID :3 ,ParentAndChildTask(4) Count: 3
        }

        #endregion

        #region サンプル

        private async Task Sample(string message)
        {
            MessageManager.WriteStart();
            for (int i = 0; i < 4; i++)
            {
                MessageManager.WriteJobStatus($"{message}, Count : {i}");
            }
            MessageManager.WriteEnd();
        }
        private async Task Sample(string message, CancellationToken token)
        {
            MessageManager.WriteStart();
            for (int i = 0; i < 4; i++)
            {
                token.ThrowIfCancellationRequested();
                MessageManager.WriteJobStatus($"{message}, Count : {i}");
            }
            MessageManager.WriteEnd();
        }

        #endregion
    }
}
