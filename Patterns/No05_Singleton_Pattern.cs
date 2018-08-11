namespace Patterns
{
    #region using
    using System;
    using System.Collections.Generic;
    using System.Threading;
    #endregion

    class No05_Singleton_Pattern
    {
        static void Main()
        {
            LoadBalancer b1 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b2 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b3 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b4 = LoadBalancer.GetLoadBalancer();

            // Same instance?
            if (b1 == b2 && b2 == b3 && b3 == b4)
            {
                Console.WriteLine("Same instance\n");
            }

            // Load balance 15 server requests
            LoadBalancer balancer = LoadBalancer.GetLoadBalancer();
            for (int i = 0; i < 15; i++)
            {
                string server = balancer.Server;
                Console.WriteLine("Dispatch Request to: " + server);
            }
        }


        /// <summary>
        /// The 'Singleton' class
        /// </summary>
        class LoadBalancer
        {
            private static LoadBalancer _instance = new LoadBalancer();
            private List<string> _servers = new List<string>();
            private Random _random = new Random();

            // Lock synchronization object
            private static object syncLock = new object();

            // Constructor (protected)
            protected LoadBalancer()
            {
                // List of available servers
                _servers.Add("ServerI");
                _servers.Add("ServerII");
                _servers.Add("ServerIII");
                _servers.Add("ServerIV");
                _servers.Add("ServerV");
            }

            //TODO 「実行中のアプリケーションの中でインスタンスを１つにする」
            //Double checked lockingは有効な手法だが、実行コストがかかるため、多用しないほうがいいらしい。
            //マルチスレッドを考えると、Singletoneパターンの実装は奥が深い。
            public static LoadBalancer GetLoadBalancer()
            {
                //pattern1 ：師匠推奨
                //if (_instance == null) _instance = new LoadBalancer();
                //var temp = new LoadBalancer();
                //Interlocked.CompareExchange(ref _instance, temp, null);
                //return _instance;

                //Pattern2
                // 'Double checked locking' pattern 
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LoadBalancer();
                        }
                    }
                }
                return _instance;
                ////Pattern3 =Lockステートメントで実際に行われていることと、Lockステートメントがダメな理由
                //bool lockToken = false;
                //try
                //{
                //    Monitor.Enter(this, ref lockToken);
                //}
                //finally
                //{
                //    //なにがあろうとロックを解除しようとする姿勢がダメだと師匠はおっしゃっている。
                //    //1）Tryステートに入るとパフォーマンスが劣化する
                //    //2）例外が発生するということは、スレッドが破壊されたということ。
                //    //   平静を装い処理を続けようとするなど、言語道断。
                //    if (lockToken) Monitor.Exit(this);
                //}
            }

            public string Server
            {
                get
                {
                    int r = _random.Next(_servers.Count);
                    return _servers[r].ToString();
                }
            }
        }
    }
}