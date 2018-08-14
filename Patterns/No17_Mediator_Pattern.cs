namespace Patterns
{
    #region using
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// 複雑に絡み合った複数のオブジェクト間の関係を、
    /// 必ず「仲介者」を介して処理を行う様にすることで単純かつ明快なインタフェースを提供するパターンです。
    /// </summary>
    class No17_Mediator_Pattern
    {
        static void Main()
        {
            Chatroom chatroom = new Chatroom();
            Participant George = new Beatle("George");
            Participant Paul = new Beatle("Paul");
            Participant Ringo = new Beatle("Ringo");
            Participant John = new Beatle("John");
            Participant Yoko = new NonBeatle("Yoko");

            //登録するのはチャットルーム
            chatroom.Register(George);
            chatroom.Register(Paul);
            chatroom.Register(Ringo);
            chatroom.Register(John);
            chatroom.Register(Yoko);

            //ユーザーが送信すると、チャットルームを介してメッセージがSENDされる
            Yoko.Send("John", "Hi John!");
            Paul.Send("Ringo", "All you need is love");
            Ringo.Send("George", "My sweet Lord");
            Paul.Send("John", "Can't buy me love");
            John.Send("Yoko", "My sweet love");
        }

        abstract class AbstractChatroom
        {
            public abstract void Register(Participant participant);
            public abstract void Send(string from, string to, string message);
        }

        class Chatroom : AbstractChatroom
        {
            private Dictionary<string, Participant> _participants = new Dictionary<string, Participant>();

            public override void Register(Participant participant)
            {
                if (!_participants.ContainsValue(participant))
                {
                    _participants[participant.Name] = participant;
                }
                //呼び出し元と実際の処理をするものを紐づける
                participant.Chatroom = this;
            }

            public override void Send(string from, string to, string message)
            {
                Participant participant = _participants[to];

                //TODO 「●複雑な処理は、処理の担当者に振り分ける（集約する）」
                //proxyと何が違う・・・？
                if (participant != null) participant.Receive(from, message);
            }
        }

        class Participant
        {
            private Chatroom _chatroom;
            private string _name;

            public Participant(string name)
            {
                this._name = name;
            }

            public string Name
            {
                get { return _name; }
            }

            public Chatroom Chatroom
            {
                set { _chatroom = value; }
                get { return _chatroom; }
            }

            public void Send(string to, string message)
            {
                _chatroom.Send(_name, to, message);
            }

            public virtual void Receive(string from, string message)
            {
                Console.WriteLine("{0} to {1}: '{2}'", from, Name, message);
            }
        }

        class Beatle : Participant
        {
            public Beatle(string name) : base(name) { }

            public override void Receive(string from, string message)
            {
                Console.Write("To a Beatle: ");
                base.Receive(from, message);
            }
        }

        class NonBeatle : Participant
        {
            public NonBeatle(string name) : base(name) { }

            public override void Receive(string from, string message)
            {
                Console.Write("To a non-Beatle: ");
                base.Receive(from, message);
            }
        }
    }
}