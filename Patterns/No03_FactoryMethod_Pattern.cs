namespace Patterns
{
    #region using
    using System;
    using System.Collections.Generic;
    #endregion

    class No03_FactoryMethod_Pattern
    {

        /// <summary>
        /// Entry point into console application.
        /// </summary>
        static void Main()
        {
            // Note: constructors call Factory Method
            List<Document> documents = new List<Document>();
            documents.Add(new Resume());
            documents.Add(new Report());

            // Display document pages
            documents.ForEach(document =>
            {
                Console.WriteLine("\n" + document.GetType().Name + "--");
                document.Pages.ForEach(page => Console.WriteLine($" {page.GetType().Name}"));
            });
        }

        abstract class Page { }

        class SkillsPage : Page { }

        class EducationPage : Page { }

        class ExperiencePage : Page { }

        class IntroductionPage : Page { }

        class ResultsPage : Page { }

        class ConclusionPage : Page { }

        class SummaryPage : Page { }

        class BibliographyPage : Page { }

        /// <summary>
        /// The 'Creator' abstract class
        /// </summary>
        abstract class Document
        {
            private List<Page> _pages = new List<Page>();

            // Constructor calls abstract Factory method
            public Document()
            {
                //継承先のクラスで実装された処理を呼び出す
                //TODO「抽象メソッドのオーバーライドの挙動」
                this.CreatePages();
            }

            public List<Page> Pages
            {
                get { return _pages; }
            }

            // Factory Method
            public abstract void CreatePages();
        }

        /// <summary>
        /// A 'ConcreteCreator' class
        /// </summary>
        class Resume : Document
        {
            // Factory Method implementation
            public override void CreatePages()
            {
                Pages.Add(new SkillsPage());
                Pages.Add(new EducationPage());
                Pages.Add(new ExperiencePage());
            }
        }

        /// <summary>
        /// A 'ConcreteCreator' class
        /// </summary>
        class Report : Document
        {
            // Factory Method implementation
            public override void CreatePages()
            {
                Pages.Add(new IntroductionPage());
                Pages.Add(new ResultsPage());
                Pages.Add(new ConclusionPage());
                Pages.Add(new SummaryPage());
                Pages.Add(new BibliographyPage());
            }
        }
    }
}