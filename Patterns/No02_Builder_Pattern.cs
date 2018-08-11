namespace Patterns
{
    #region using
    using System;
    using System.Collections.Generic;
    #endregion

    public class No02_Builder_Pattern
    {
        public static void Main()
        {
            //Builder
            VehicleBuilder builder;

            //Create ConcreteBuilder and Execute Build
            builder = new ScooterBuilder();
            Shop.Construct(builder);
            builder.Vehicle.Show();

            builder = new CarBuilder();
            Shop.Construct(builder);
            builder.Vehicle.Show();

            builder = new MotorCycleBuilder();
            Shop.Construct(builder);
            builder.Vehicle.Show();
        }

        /// <summary>
        /// The 'Director' class
        /// </summary>
        static class Shop
        {
            // Builder uses a complex series of steps
            public static void Construct(VehicleBuilder vehicleBuilder)
            {
                //一か所にまとめておくことも可能。
                vehicleBuilder.BuildFrame();
                vehicleBuilder.BuildEngine();
                vehicleBuilder.BuildWheels();
                vehicleBuilder.BuildDoors();
            }
        }

        #region Builder implimention

        /// <summary>
        /// The 'ConcreteBuilder1' class
        /// </summary>
        class MotorCycleBuilder : VehicleBuilder
        {
            public MotorCycleBuilder()
            {
                vehicle = new Vehicle("MotorCycle");
            }

            public override void BuildFrame()
            {
                vehicle["frame"] = "MotorCycle Frame";
            }

            public override void BuildEngine()
            {
                vehicle["engine"] = "500 cc";
            }

            public override void BuildWheels()
            {
                vehicle["wheels"] = "2";
            }

            public override void BuildDoors()
            {
                vehicle["doors"] = "0";
            }
        }

        /// <summary>
        /// The 'ConcreteBuilder2' class
        /// </summary>
        class CarBuilder : VehicleBuilder
        {
            public CarBuilder()
            {
                vehicle = new Vehicle("Car");
            }

            public override void BuildFrame()
            {
                vehicle["frame"] = "Car Frame";
            }

            public override void BuildEngine()
            {
                vehicle["engine"] = "2500 cc";
            }

            public override void BuildWheels()
            {
                vehicle["wheels"] = "4";
            }

            public override void BuildDoors()
            {
                vehicle["doors"] = "4";
            }
        }

        /// <summary>
        /// The 'ConcreteBuilder3' class
        /// </summary>
        class ScooterBuilder : VehicleBuilder
        {
            public ScooterBuilder()
            {
                vehicle = new Vehicle("Scooter");
            }

            public override void BuildFrame()
            {
                vehicle["frame"] = "Scooter Frame";
            }

            public override void BuildEngine()
            {
                vehicle["engine"] = "50 cc";
            }

            public override void BuildWheels()
            {
                vehicle["wheels"] = "2";
            }

            public override void BuildDoors()
            {
                vehicle["doors"] = "0";
            }
        }

        #endregion

        /// <summary>
        /// The 'Builder' abstract class
        /// </summary>
        abstract class VehicleBuilder
        {
            //TODO 「抽象クラスの実装：その２」
            protected Vehicle vehicle;

            // Gets vehicle instance
            public Vehicle Vehicle
            {
                get { return vehicle; }
            }

            // Abstract build methods
            public abstract void BuildFrame();
            public abstract void BuildEngine();
            public abstract void BuildWheels();
            public abstract void BuildDoors();
        }

        /// <summary>
        /// The 'Product' class
        /// </summary>
        class Vehicle
        {
            private string _vehicleType;
            private Dictionary<string, string> _parts = new Dictionary<string, string>();

            // Constructor
            public Vehicle(string vehicleType)
            {
                this._vehicleType = vehicleType;
            }

            // Indexer
            public string this[string key]
            {
                get { return _parts[key]; }
                set { _parts[key] = value; }
            }

            public void Show()
            {
                Console.WriteLine("\n---------------------------");
                Console.WriteLine("Vehicle Type: {0}", _vehicleType);
                Console.WriteLine(" Frame : {0}", _parts["frame"]);
                Console.WriteLine(" Engine : {0}", _parts["engine"]);
                Console.WriteLine(" #Wheels: {0}", _parts["wheels"]);
                Console.WriteLine(" #Doors : {0}", _parts["doors"]);
            }
        }
    }
}