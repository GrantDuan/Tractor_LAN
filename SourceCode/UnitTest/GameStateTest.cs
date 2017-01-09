using Duan.Xiugang.Tractor.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for GameStateTest and is intended
    ///to contain all GameStateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GameStateTest
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
        ///A test for GetNextPlayerAfterThePlayer
        ///</summary>
        [TestMethod()]
        public void GetNextPlayerAfterThePlayerTest()
        {
            GameState target = new GameState(); 
            target.Players.Add(new PlayerEntity{PlayerId = "p1",Rank = 0,Team = GameTeam.VerticalTeam});
            target.Players.Add(new PlayerEntity { PlayerId = "p2", Rank = 2, Team = GameTeam.HorizonTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p3", Rank = 0, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p4", Rank = 2, Team = GameTeam.HorizonTeam });
            string playerId = "p1";
            string expected = "p2"; 
            PlayerEntity actual;
            actual = target.GetNextPlayerAfterThePlayer(playerId);
            Assert.AreEqual(expected, actual.PlayerId);
            
        }

        /// <summary>
        ///A test for NextRank
        ///</summary>
        [TestMethod()]
        public void NextRankTest()
        {
            GameState target = new GameState();
            target.Players.Add(new PlayerEntity { PlayerId = "p1", Rank = 0, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p2", Rank = 2, Team = GameTeam.HorizonTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p3", Rank = 0, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p4", Rank = 2, Team = GameTeam.HorizonTeam });

            var nextStart = target.NextRank("p1", 40);
            Assert.AreEqual(1, nextStart.Rank);

            nextStart = target.NextRank("p3", 30);
            Assert.AreEqual(3, nextStart.Rank);

            nextStart = target.NextRank("p1", 0);
            Assert.AreEqual(6, nextStart.Rank);

        }

        /// <summary>
        ///A test for NextRank
        ///</summary>
        [TestMethod()]
        public void NextRankTest1()
        {
            GameState target = new GameState();
            target.Players.Add(new PlayerEntity { PlayerId = "p1", Rank = 0, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p2", Rank = 2, Team = GameTeam.HorizonTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p3", Rank = 0, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p4", Rank = 2, Team = GameTeam.HorizonTeam });



            var nextStart = target.NextRank("p2", 80);
            Assert.AreEqual(0, nextStart.Rank);

            nextStart = target.NextRank("p2", 120);
            Assert.AreEqual(0, nextStart.Rank);

            nextStart = target.NextRank("p2", 160);
            Assert.AreEqual(0, nextStart.Rank);

            nextStart = target.NextRank("p3", 70);
            Assert.AreEqual(1, nextStart.Rank);

            nextStart = target.NextRank("p4", 120);
            Assert.AreEqual(2, nextStart.Rank);

            nextStart = target.NextRank("p4", 160);
            Assert.AreEqual(4, nextStart.Rank);

        }

        ///A test for NextRank
        ///</summary>
        [TestMethod()]
        public void NextRankTest2()
        {
            GameState target = new GameState();
            target.Players.Add(new PlayerEntity { PlayerId = "p1", Rank = 8, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p2", Rank = 9, Team = GameTeam.HorizonTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p3", Rank = 8, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p4", Rank = 9, Team = GameTeam.HorizonTeam });



            var nextStart = target.NextRank("p1", 0);
            Assert.AreEqual(9, nextStart.Rank);



        }

        ///A test for NextRank
        ///</summary>
        [TestMethod()]
        public void NextRankTest3()
        {
            GameState target = new GameState();
            target.Players.Add(new PlayerEntity { PlayerId = "p1", Rank = 8, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p2", Rank = 9, Team = GameTeam.HorizonTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p3", Rank = 8, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p4", Rank = 9, Team = GameTeam.HorizonTeam });



            var nextStart = target.NextRank("p2", 170);
            Assert.AreEqual(9, nextStart.Rank);

            nextStart = target.NextRank("p3", 120);
            Assert.AreEqual(9, nextStart.Rank);

        }

        /// <summary>
        ///A test for GetNextPlayerAfterThePlayer
        ///</summary>
        [TestMethod()]
        public void GetNextPlayerAfterThePlayerTest2()
        {
            GameState target = new GameState();
            target.Players.Add(new PlayerEntity { PlayerId = "p1", Rank = 0, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p2", Rank = 2, Team = GameTeam.HorizonTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p3", Rank = 0, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p4", Rank = 2, Team = GameTeam.HorizonTeam });

            

            Assert.AreEqual("p2", target.GetNextPlayerAfterThePlayer(false, "p1").PlayerId);
            Assert.AreEqual("p3", target.GetNextPlayerAfterThePlayer(false, "p2").PlayerId);
            Assert.AreEqual("p4", target.GetNextPlayerAfterThePlayer(false, "p3").PlayerId);
            Assert.AreEqual("p1", target.GetNextPlayerAfterThePlayer(false, "p4").PlayerId);

            Assert.AreEqual("p3", target.GetNextPlayerAfterThePlayer(true, "p1").PlayerId);
            Assert.AreEqual("p4", target.GetNextPlayerAfterThePlayer(true, "p2").PlayerId);
            Assert.AreEqual("p1", target.GetNextPlayerAfterThePlayer(true, "p3").PlayerId);
            Assert.AreEqual("p2", target.GetNextPlayerAfterThePlayer(true, "p4").PlayerId);
            
            
        }



        /// <summary>
        ///A test for ArePlayersInSameTeam
        ///</summary>
        [TestMethod()]
        public void ArePlayersInSameTeamTest()
        {
            GameState target = new GameState();
            target.Players.Add(new PlayerEntity { PlayerId = "p1", Rank = 0, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p2", Rank = 2, Team = GameTeam.HorizonTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p3", Rank = 0, Team = GameTeam.VerticalTeam });
            target.Players.Add(new PlayerEntity { PlayerId = "p4", Rank = 2, Team = GameTeam.HorizonTeam });

            Assert.IsTrue(target.ArePlayersInSameTeam("p1", "p3"));
            Assert.IsTrue(target.ArePlayersInSameTeam("p2", "p4"));
            Assert.IsFalse(target.ArePlayersInSameTeam("p2", "p1"));
            Assert.IsFalse(target.ArePlayersInSameTeam("p3", "p4"));
        }
    }
}
