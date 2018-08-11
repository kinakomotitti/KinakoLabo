namespace Patterns
{
    using System;

    public class No01_AbstractFactory_Pattern
    {
        public static void Main()
        {
            //AfricaFactory is Concrete Factory.
            //ContinentFactory is Abstract Factory.
            ContinentFactory africa = new AfricaFactory();
            ContinentFactory america = new AmericaFactory();

            //同じ要領で操作が実行できる
            africa.CreateCarnivore();
            america.CreateHerbivore();
        }

        #region Abstract

        #region Abstract Factory

        /// <summary>
        /// The 'AbstractFactory' abstract class
        /// </summary>
        abstract class ContinentFactory
        {
            //TODO 「抽象クラスの実装：その１」
            public abstract Herbivore CreateHerbivore();
            public abstract Carnivore CreateCarnivore();
        }

        #endregion

        #region Abstract Product

        /// <summary>
        /// The 'AbstractProductA' abstract class
        /// </summary>
        abstract class Herbivore
        {
        }

        /// <summary>
        /// The 'AbstractProductB' abstract class
        /// </summary>
        abstract class Carnivore
        {
            public abstract void Eat(Herbivore h);
        }

        #endregion

        #endregion

        #region Factory

        /// <summary>
        /// The 'ConcreteFactory1' class
        /// </summary>
        class AfricaFactory : ContinentFactory
        {
            public override Herbivore CreateHerbivore()
            {
                return new Wildebeest();
            }

            public override Carnivore CreateCarnivore()
            {
                return new Lion();
            }
        }

        /// <summary>
        /// The 'ConcreteFactory2' class
        /// </summary>
        class AmericaFactory : ContinentFactory
        {
            public override Herbivore CreateHerbivore()
            {
                return new Bison();
            }

            public override Carnivore CreateCarnivore()
            {
                return new Wolf();
            }
        }

        #endregion

        #region Product

        /// <summary>
        /// The 'ProductA1' class
        /// </summary>
        class Wildebeest : Herbivore
        {
        }

        /// <summary>
        /// The 'ProductB1' class
        /// </summary>
        class Lion : Carnivore
        {
            public override void Eat(Herbivore h)
            {
                // Eat Wildebeest
                Console.WriteLine(this.GetType().Name +" eats " + h.GetType().Name);
            }
        }

        /// <summary>
        /// The 'ProductA2' class
        /// </summary>
        class Bison : Herbivore
        {
        }

        /// <summary>
        /// The 'ProductB2' class
        /// </summary>
        class Wolf : Carnivore
        {

            public override void Eat(Herbivore h)
            {
                // Eat Bison
                Console.WriteLine(this.GetType().Name + " eats " + h.GetType().Name);
            }
        }

        #endregion
    }
}