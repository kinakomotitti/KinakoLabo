﻿namespace Patterns
{
    #region using
    using System;
    using System.Collections.Generic;
    #endregion

    class No08_Composite_Pattern
    {
        /// <summary>
        /// Entry point into console application.
        /// </summary>
        static void Main()
        {
            // Create a tree structure 
            CompositeElement root = new CompositeElement("Picture");
            root.Add(new PrimitiveElement("Red Line"));
            root.Add(new PrimitiveElement("Blue Circle"));
            root.Add(new PrimitiveElement("Green Box"));

            // Create a branch
            CompositeElement comp = new CompositeElement("Two Circles");
            comp.Add(new PrimitiveElement("Black Circle"));
            comp.Add(new PrimitiveElement("White Circle"));
            root.Add(comp);

            // Add and remove a PrimitiveElement
            PrimitiveElement pe = new PrimitiveElement("Yellow Line");
            root.Add(pe);
            root.Remove(pe);

            // Recursively display nodes
            root.Display(1);
        }

        abstract class DrawingElement
        {
            protected string _name;

            public DrawingElement(string name)
            {
                this._name = name;
            }
            public abstract void Add(DrawingElement d);
            public abstract void Remove(DrawingElement d);
            public abstract void Display(int indent);
        }

        /// <summary>
        /// The 'Leaf' class
        /// </summary>
        class PrimitiveElement : DrawingElement
        {
            // Constructor
            public PrimitiveElement(string name) : base(name) { }
            public override void Add(DrawingElement c) 
            {
                Console.WriteLine("Cannot add to a PrimitiveElement");
            }

            public override void Remove(DrawingElement c)
            {
                Console.WriteLine("Cannot remove from a PrimitiveElement");
            }

            public override void Display(int indent)
            {
                Console.WriteLine($"{new String('-', indent)}+ {_name}");
            }
        }

        class CompositeElement : DrawingElement
        {
            //TODO 「✇木構造を表現する」
            private List<DrawingElement> elements =new List<DrawingElement>();

            public CompositeElement(string name) : base(name) { }

            public override void Add(DrawingElement d)
            {
                elements.Add(d);
            }

            public override void Remove(DrawingElement d)
            {
                elements.Remove(d);
            }

            public override void Display(int indent)
            {
                Console.WriteLine($"{new String('-', indent)}+ {_name}");
                foreach (DrawingElement d in elements)
                {
                    d.Display(indent + 2);
                }
            }
        }
    }
}