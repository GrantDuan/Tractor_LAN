using System.Collections.Generic;
using Duan.Xiugang.Tractor.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for TractorRulesTest and is intended
    ///to contain all TractorRulesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TractorRulesTest
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

        #region Test GetWinner
        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerSingleNonTrump()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int>{1};
            trickState.ShowedCards["p2"] = new List<int> { 2 };
            trickState.ShowedCards["p3"] = new List<int> { 3 };
            trickState.ShowedCards["p4"] = new List<int> { 12 };
            
            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p4", actual);
            
        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerSingleTrump()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> { 40 };
            trickState.ShowedCards["p3"] = new List<int> { 3 };
            trickState.ShowedCards["p4"] = new List<int> { 12 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p2", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerSingleRank()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> { 40 };
            trickState.ShowedCards["p3"] = new List<int> { 0 };
            trickState.ShowedCards["p4"] = new List<int> { 12 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p3", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerSingleJoker()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1 };
            trickState.ShowedCards["p2"] = new List<int> { 40 };
            trickState.ShowedCards["p3"] = new List<int> { 0 };
            trickState.ShowedCards["p4"] = new List<int> { 53 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p4", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerPair()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1, 1 };
            trickState.ShowedCards["p2"] = new List<int> { 40, 40 };
            trickState.ShowedCards["p3"] = new List<int> { 0, 0 };
            trickState.ShowedCards["p4"] = new List<int> { 53,53 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p4", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerTractor()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1, 1, 2,2 };
            trickState.ShowedCards["p2"] = new List<int> { 40, 40, 45,45 };
            trickState.ShowedCards["p3"] = new List<int> { 8, 8 , 10,10};
            trickState.ShowedCards["p4"] = new List<int> { 53, 53, 25,25 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p1", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerTractor3()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1, 1, 2, 2 ,3,3};
            trickState.ShowedCards["p2"] = new List<int> { 40, 40, 45, 45, 48,48};
            trickState.ShowedCards["p3"] = new List<int> { 8, 8, 10, 10 ,11,11};
            trickState.ShowedCards["p4"] = new List<int> { 53, 53, 52,52,25, 25 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p1", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpSingle()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1, 2, 3, 4,5 };
            trickState.ShowedCards["p2"] = new List<int> { 40, 40, 45, 45, 27 };
            trickState.ShowedCards["p3"] = new List<int> { 18, 18, 20, 20, 21 };
            trickState.ShowedCards["p4"] = new List<int> { 53, 53, 25, 25,1 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p1", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpSingle1()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1, 2, 3, 4, 5 };
            trickState.ShowedCards["p2"] = new List<int> { 40, 40, 45, 45, 13 };
            trickState.ShowedCards["p3"] = new List<int> { 18, 18, 20, 20, 21 };
            trickState.ShowedCards["p4"] = new List<int> { 53, 53, 39, 39, 41 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p4", actual);

        }


        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpSingle2()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 52,53 };
            trickState.ShowedCards["p2"] = new List<int> { 26,39 };
            trickState.ShowedCards["p3"] = new List<int> { 51,39 };
            trickState.ShowedCards["p4"] = new List<int> {25, 1 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p1", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpSingle3()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 14, 15,16,17 };
            trickState.ShowedCards["p2"] = new List<int> { 40,41,42,13};
            trickState.ShowedCards["p3"] = new List<int> { 40, 41, 42, 39 };
            trickState.ShowedCards["p4"] = new List<int> {44, 45, 46, 47};

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p3", actual);

        }

       

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpPair()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1, 3, 3, 4, 5 };
            trickState.ShowedCards["p2"] = new List<int> { 40, 40, 45, 45, 13 };
            trickState.ShowedCards["p3"] = new List<int> { 18, 18, 20, 20, 21 };
            trickState.ShowedCards["p4"] = new List<int> { 53, 53, 39, 39, 41 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p4", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpPair1()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1, 3, 3, 4, 5 };
            trickState.ShowedCards["p2"] = new List<int> { 40, 41, 42, 45, 13 };
            trickState.ShowedCards["p3"] = new List<int> { 18, 18, 20, 20, 21 };
            trickState.ShowedCards["p4"] = new List<int> { 53, 52, 39, 40, 41 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p1", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpPair2()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1, 3, 3, 4, 5 };
            trickState.ShowedCards["p2"] = new List<int> { 40, 40, 45, 45, 14 };
            trickState.ShowedCards["p3"] = new List<int> { 18, 18, 20, 20, 21 };
            trickState.ShowedCards["p4"] = new List<int> { 53, 53, 39, 39, 14 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p1", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpPair3()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 1, 3, 3, 5, 5 };
            trickState.ShowedCards["p2"] = new List<int> { 40, 40, 53, 53, 50 };
            trickState.ShowedCards["p3"] = new List<int> { 18, 18, 20, 20, 21 };
            trickState.ShowedCards["p4"] = new List<int> { 49, 49, 39, 39, 50 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p2", actual);

        }


        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpTractor()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 12, 3, 3, 4, 4 };
            trickState.ShowedCards["p2"] = new List<int> { 44, 44, 45, 45, 13 };
            trickState.ShowedCards["p3"] = new List<int> { 19, 19, 20, 20, 21 };
            trickState.ShowedCards["p4"] = new List<int> { 53, 53, 39, 39, 52 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p2", actual);

        }

        /// <summary>
        ///A test for GetWinner
        ///</summary>
        [TestMethod()]
        public void GetWinnerDumpTractor2()
        {
            CurrentTrickState trickState = new CurrentTrickState();
            trickState.Trump = Suit.Club;
            trickState.Rank = 0;
            trickState.Learder = "p1";
            trickState.ShowedCards["p1"] = new List<int> { 12, 3, 3, 4, 4 };
            trickState.ShowedCards["p2"] = new List<int> { 40, 40, 45, 45, 13 };
            trickState.ShowedCards["p3"] = new List<int> { 19, 19, 20, 20, 21 };
            trickState.ShowedCards["p4"] = new List<int> { 12, 9, 9, 11, 11 };

            string actual = TractorRules.GetWinner(trickState);
            Assert.AreEqual("p1", actual);

        }

        #endregion

        #region IsLeadingCardsValid
        /// <summary>
        ///A test for IsLeadingCardsValid
        ///</summary>
        [TestMethod()]
        public void IsLeadingCardsValidTest()
        {
            Dictionary<string, CurrentPoker> playerHoldingCards = new Dictionary<string, CurrentPoker>();
            playerHoldingCards["p2"] = new CurrentPoker(new List<int>() { 11}, Suit.Spade, 0);
            List<int> selectedCards = new List<int> { 12,11,10}; 
            string player = "p1"; 

            ShowingCardsValidationResult result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingFail, result.ResultType);
            Assert.AreEqual(1, result.MustShowCardsForDumpingFail.Count);
            Assert.IsTrue(result.MustShowCardsForDumpingFail.Contains(10));

            selectedCards = new List<int> { 12, 11 };
            result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingSuccess, result.ResultType);
            
        }

        /// <summary>
        ///A test for IsLeadingCardsValid
        ///</summary>
        [TestMethod()]
        public void IsLeadingCardsValidTestPair()
        {
            Dictionary<string, CurrentPoker> playerHoldingCards = new Dictionary<string, CurrentPoker>();
            playerHoldingCards["p2"] = new CurrentPoker(new List<int>() { 8, 8 }, Suit.Spade, 0);
            List<int> selectedCards = new List<int> { 12,  7, 7 };
            string player = "p1";

            ShowingCardsValidationResult result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingFail, result.ResultType);
            Assert.AreEqual(2, result.MustShowCardsForDumpingFail.Count);
            Assert.IsTrue(result.MustShowCardsForDumpingFail.Contains(7));
            Assert.IsTrue(!result.MustShowCardsForDumpingFail.Contains(12));

            selectedCards = new List<int> { 12, 9, 9 };
            result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingSuccess, result.ResultType);

        }

        /// <summary>
        ///A test for IsLeadingCardsValid
        ///</summary>
        [TestMethod()]
        public void IsLeadingCardsValidTestPair1()
        {
            Dictionary<string, CurrentPoker> playerHoldingCards = new Dictionary<string, CurrentPoker>();
            playerHoldingCards["p2"] = new CurrentPoker(new List<int>() { 8, 8 ,12}, Suit.Spade, 0);
            List<int> selectedCards = new List<int> { 11, 10, 10 };
            string player = "p1";

            ShowingCardsValidationResult result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingFail, result.ResultType);
            Assert.AreEqual(1, result.MustShowCardsForDumpingFail.Count);
            Assert.IsTrue(result.MustShowCardsForDumpingFail.Contains(11));
 
            selectedCards = new List<int> { 12, 9, 9 };
            result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingSuccess, result.ResultType);

        }
        /// <summary>
        ///A test for IsLeadingCardsValid
        ///</summary>
        [TestMethod()]
        public void IsLeadingCardsValidTestPair2()
        {
            Dictionary<string, CurrentPoker> playerHoldingCards = new Dictionary<string, CurrentPoker>();
            playerHoldingCards["p2"] = new CurrentPoker(new List<int>() { 8, 8, 12 }, Suit.Spade, 0);
            playerHoldingCards["p3"] = new CurrentPoker(new List<int>() { 5, 5, 7 }, Suit.Spade, 0);
            List<int> selectedCards = new List<int> { 11, 10, 10 };
            string player = "p1";

            ShowingCardsValidationResult result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingFail, result.ResultType);
            Assert.AreEqual(1, result.MustShowCardsForDumpingFail.Count);
            Assert.IsTrue(result.MustShowCardsForDumpingFail.Contains(11));

            selectedCards = new List<int> { 12, 9, 9 };
            result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingSuccess, result.ResultType);

        }

        /// <summary>
        ///A test for IsLeadingCardsValid
        ///</summary>
        [TestMethod()]
        public void IsLeadingCardsValidDump2Pairs()
        {
            Dictionary<string, CurrentPoker> playerHoldingCards = new Dictionary<string, CurrentPoker>();
            playerHoldingCards["p2"] = new CurrentPoker(new List<int>() { 18, 15, 22 , 19, 21}, Suit.Spade, 0);
            playerHoldingCards["p3"] = new CurrentPoker(new List<int>() { 25, 25, 24 ,24, 22}, Suit.Spade, 0);
            List<int> selectedCards = new List<int> { 10, 10 , 6, 6 };
            string player = "p1";

            ShowingCardsValidationResult result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingSuccess, result.ResultType);



        }

        /// <summary>
        ///A test for IsLeadingCardsValid
        ///</summary>
        [TestMethod()]
        public void IsLeadingCardsValidDumpSingles()
        {
            Dictionary<string, CurrentPoker> playerHoldingCards = new Dictionary<string, CurrentPoker>();
            playerHoldingCards["p2"] = new CurrentPoker(new List<int>() { 35, 7, 9, 22, 52 }, Suit.Spade, 0);
            playerHoldingCards["p3"] = new CurrentPoker(new List<int>() { 30, 35, 36, 9, 10 }, Suit.Spade, 0);
            playerHoldingCards["p4"] = new CurrentPoker(new List<int>() { 27, 31, 37, 36, 16 }, Suit.Spade, 0);
            List<int> selectedCards = new List<int> { 42, 45, 47, 48,49 };
            string player = "p1";

            ShowingCardsValidationResult result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingSuccess, result.ResultType);



        }

        /// <summary>
        ///A test for IsLeadingCardsValid
        ///</summary>
        [TestMethod()]
        public void IsLeadingCardsValidTestTractor()
        {
            Dictionary<string, CurrentPoker> playerHoldingCards = new Dictionary<string, CurrentPoker>();
            playerHoldingCards["p2"] = new CurrentPoker(new List<int>() { 8, 8, 7,7}, Suit.Spade, 0);
            List<int> selectedCards = new List<int> {4,4,5,5, 12 };
            string player = "p1";

            ShowingCardsValidationResult result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingFail, result.ResultType);
            Assert.AreEqual(4, result.MustShowCardsForDumpingFail.Count);
            Assert.IsTrue(result.MustShowCardsForDumpingFail.Contains(4));
            Assert.IsTrue(result.MustShowCardsForDumpingFail.Contains(5));

            selectedCards = new List<int> { 12, 10,10,9,9 };
            result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingSuccess, result.ResultType);

        }

        /// <summary>
        ///A test for IsLeadingCardsValid
        ///</summary>
        [TestMethod()]
        public void IsLeadingCardsValidTestTractor2()
        {
            Dictionary<string, CurrentPoker> playerHoldingCards = new Dictionary<string, CurrentPoker>();
            playerHoldingCards["p2"] = new CurrentPoker(new List<int>() { 8, 8, 7, 7 }, Suit.Spade, 0);
            List<int> selectedCards = new List<int> { 3, 3, 4, 4, 5, 5, 12 };
            string player = "p1";

            ShowingCardsValidationResult result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.DumpingSuccess, result.ResultType);

            selectedCards = new List<int> { 3, 3, 4, 4, 5, 5 };
            player = "p1";

            result = TractorRules.IsLeadingCardsValid(playerHoldingCards, selectedCards, player);
            Assert.AreEqual(ShowingCardsValidationResultType.Valid, result.ResultType);
        }

        #endregion

        /// <summary>
        ///A test for IsValid
        ///</summary>
        [TestMethod()]
        public void IsValidTest()
        {
            CurrentTrickState currentTrickState = new CurrentTrickState { Trump = Suit.Heart, Rank = 0, Learder = "p1" };
            currentTrickState.ShowedCards.Add("p1", new List<int> { });
            currentTrickState.ShowedCards.Add("p2", new List<int> { });
            currentTrickState.ShowedCards.Add("p3", new List<int> { });
            currentTrickState.ShowedCards.Add("p4", new List<int> { });
            List<int> selectedCards = new List<int> { 1, 1, 2, 2, 3, 3 }; 
            CurrentPoker currentCards = new CurrentPoker(new int[]{}, Suit.Heart, 0);
            ShowingCardsValidationResultType expected = ShowingCardsValidationResultType.Valid;
            ShowingCardsValidationResultType actual;
            actual = TractorRules.IsValid(currentTrickState, selectedCards, currentCards).ResultType;
            Assert.AreEqual(expected, actual);
 
        }

        /// <summary>
        ///A test for IsValid
        ///</summary>
        [TestMethod()]
        public void IsValidTest1()
        {
            CurrentTrickState currentTrickState = new CurrentTrickState { Trump = Suit.Heart, Rank = 0, Learder = "p1"};
            currentTrickState.ShowedCards.Add("p1", new List<int> { 39, 39, 0, 0});
            currentTrickState.ShowedCards.Add("p2", new List<int> { });
            currentTrickState.ShowedCards.Add("p3", new List<int> { });
            currentTrickState.ShowedCards.Add("p4", new List<int> { });
            List<int> selectedCards = new List<int> { 1, 2, 3, 4 };
            CurrentPoker currentCards = new CurrentPoker(new int[] { 1, 2, 3, 4 , 5,5}, Suit.Heart, 0);
            ShowingCardsValidationResultType expected = ShowingCardsValidationResultType.Invalid;
            ShowingCardsValidationResultType actual;
            actual = TractorRules.IsValid(currentTrickState, selectedCards, currentCards).ResultType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsValid
        ///</summary>
        [TestMethod()]
        public void IsValidTest2()
        {
            CurrentTrickState currentTrickState = new CurrentTrickState { Trump = Suit.Heart, Rank = 0, Learder = "p1" };
            currentTrickState.ShowedCards.Add("p1", new List<int> { 39, 39, 0, 0 });
            currentTrickState.ShowedCards.Add("p2", new List<int> { });
            currentTrickState.ShowedCards.Add("p3", new List<int> { });
            currentTrickState.ShowedCards.Add("p4", new List<int> { });
            List<int> selectedCards = new List<int> { 1, 2, 5, 5 };
            CurrentPoker currentCards = new CurrentPoker(new int[] { 1, 2, 3, 4, 5, 5 }, Suit.Heart, 0);
            ShowingCardsValidationResultType expected = ShowingCardsValidationResultType.Valid;
            ShowingCardsValidationResultType actual;
            actual = TractorRules.IsValid(currentTrickState, selectedCards, currentCards).ResultType;
            Assert.AreEqual(expected, actual);
        }
    }
}
