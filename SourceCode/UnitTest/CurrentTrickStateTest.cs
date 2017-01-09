using System.Collections.Generic;
using Duan.Xiugang.Tractor.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for CurrentTrickStateTest and is intended
    ///to contain all CurrentTrickStateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CurrentTrickStateTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for NextPlayer
        ///</summary>
        [TestMethod()]
        public void NextPlayerTest()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> { 2 };
            trickState.ShowedCards["p3"] = new List<int> { 3 };
            trickState.ShowedCards["p4"] = new List<int> { 12 };

            string actual;
            actual = trickState.NextPlayer("p1");
            Assert.AreEqual("p2", actual);
            Assert.AreEqual("p3", trickState.NextPlayer("p2"));
            Assert.AreEqual("p4", trickState.NextPlayer("p3"));
            Assert.AreEqual("p1", trickState.NextPlayer("p4"));
        }

        /// <summary>
        ///A test for NextPlayer
        ///</summary>
        [TestMethod()]
        public void NextPlayerTest1()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> {  };
            trickState.ShowedCards["p2"] = new List<int> {  };
            trickState.ShowedCards["p3"] = new List<int> {  };
            trickState.ShowedCards["p4"] = new List<int> {  };

            Assert.AreEqual("p1", trickState.NextPlayer());

            trickState.ShowedCards["p1"] = new List<int> { 1};
            trickState.ShowedCards["p2"] = new List<int> { };
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p2", trickState.NextPlayer());


            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> {2 };
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p3", trickState.NextPlayer());


            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> { 2 };
            trickState.ShowedCards["p3"] = new List<int> {3 };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p4", trickState.NextPlayer());

            trickState.Learder = "p2";
            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { };
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };

            Assert.AreEqual("p2", trickState.NextPlayer());

            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { 1};
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p3", trickState.NextPlayer());


            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { 1 };
            trickState.ShowedCards["p3"] = new List<int> {2 };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p4", trickState.NextPlayer());

            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { 1 };
            trickState.ShowedCards["p3"] = new List<int> {2};
            trickState.ShowedCards["p4"] = new List<int> { 3};
            Assert.AreEqual("p1", trickState.NextPlayer());

            trickState.Learder = "p2";
            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { };
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };

            Assert.AreEqual("p2", trickState.NextPlayer());

            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { 1,1, 5, 12};
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p3", trickState.NextPlayer());


            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { 1, 1, 5, 12 };
            trickState.ShowedCards["p3"] = new List<int> { 1, 1, 5, 12 };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p4", trickState.NextPlayer());

            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { 1, 1, 5, 12 };
            trickState.ShowedCards["p3"] = new List<int> { 1, 1, 5, 12 };
            trickState.ShowedCards["p4"] = new List<int> { 1, 1, 5, 12 };
            Assert.AreEqual("p1", trickState.NextPlayer());
        }

        /// <summary>
        ///A test for LatestPlayerShowedCard
        ///</summary>
        [TestMethod()]
        public void LatestPlayerShowedCardTest()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { };
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };

            Assert.AreEqual("", trickState.LatestPlayerShowedCard());

            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> { };
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p1", trickState.LatestPlayerShowedCard());




            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> { 2 };
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p2", trickState.LatestPlayerShowedCard());


            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> { 2 };
            trickState.ShowedCards["p3"] = new List<int> { 3 };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p3", trickState.LatestPlayerShowedCard());

            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> { 2 };
            trickState.ShowedCards["p3"] = new List<int> { 3 };
            trickState.ShowedCards["p4"] = new List<int> { 4};
            Assert.AreEqual("p4", trickState.LatestPlayerShowedCard());

            trickState.Learder = "p2";
            trickState.ShowedCards["p1"] = new List<int> { };
            trickState.ShowedCards["p2"] = new List<int> { 1 };
            trickState.ShowedCards["p3"] = new List<int> { };
            trickState.ShowedCards["p4"] = new List<int> { };
            Assert.AreEqual("p2", trickState.LatestPlayerShowedCard());
        }
    }
}
