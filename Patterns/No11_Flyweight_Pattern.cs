namespace Patterns
{
    #region using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// このパターンは、生成済みのオブジェクトをできるだけ再利用(共有)し、
    /// 無駄なオブジェクト生成(メモリ使用)を防止する仕組みを提供するパターンです。
    /// </summary>
    class No11_Flyweight_Pattern
    {
        static void Main()
        {
            // Build a document with text
            string document = "AAZZBBZB";
            char[] chars = document.ToCharArray();
            CharacterFactory factory = new CharacterFactory();
            
            // extrinsic state
            int pointSize = 10;

            // For each character use a flyweight object
            foreach (char c in chars)
            {
                pointSize++;
                Character character = factory.GetCharacter(c);
                character.Display(pointSize);
            }
        }

        /// <summary>
        /// The 'FlyweightFactory' class
        /// </summary>
        class CharacterFactory
        {
            private Dictionary<char, Character> _characters =new Dictionary<char, Character>();

            public Character GetCharacter(char key)
            {
                // Uses "lazy initialization"
                Character character = null;
                if (_characters.ContainsKey(key))
                {
                    //このクラスの生存期間の中で、_characters変数が再利用される。
                    //TODO 「共通クラスのプロパティのライフサイクルとその使い方」
                    //Flyweightの実態は、_charactersこれと、これをホストしているこのクラス！
                    character = _characters[key];
                }
                else
                {
                    switch (key)
                    {
                        case 'A': character = new CharacterA(); break;
                        case 'B': character = new CharacterB(); break;
                        case 'Z': character = new CharacterZ(); break;
                    }
                    //ない場合は生成される
                    _characters.Add(key, character);
                }
                return character;
            }
        }

        /// <summary>
        /// The 'Flyweight' abstract class
        /// </summary>
        abstract class Character
        {
            protected char symbol;

            protected int width;

            protected int height;

            protected int ascent;

            protected int descent;

            protected int pointSize;

            public abstract void Display(int pointSize);
        }

        #region FlyWeight Class

        class CharacterA : Character
        {
            // Constructor
            public CharacterA()
            {
                this.symbol = 'A';
                this.height = 100;
                this.width = 120;
                this.ascent = 70;
                this.descent = 0;
            }

            public override void Display(int pointSize)
            {
                this.pointSize = pointSize;
                Console.WriteLine($"{this.symbol} (pointsize{this.pointSize}) ");
            }
        }
        class CharacterB : Character
        {
            // Constructor
            public CharacterB()
            {
                this.symbol = 'B';
                this.height = 100;
                this.width = 140;
                this.ascent = 72;
                this.descent = 0;
            }

            public override void Display(int pointSize)
            {
                this.pointSize = pointSize;
                Console.WriteLine($"{this.symbol} (pointsize{this.pointSize}) ");
            }
        }

        class CharacterZ : Character
        {
            // Constructor
            public CharacterZ()
            {
                this.symbol = 'Z';
                this.height = 100;
                this.width = 100;
                this.ascent = 68;
                this.descent = 0;
            }

            public override void Display(int pointSize)
            {
                this.pointSize = pointSize;
                Console.WriteLine($"{this.symbol} (pointsize{this.pointSize}) ");
            }
        }
        #endregion
    }
}