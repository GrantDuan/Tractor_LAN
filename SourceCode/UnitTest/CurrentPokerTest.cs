using System.Collections.Generic;
using Duan.Xiugang.Tractor.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    /// <summary>
    ///     This is a test class for CurrentPokerTest and is intended
    ///     to contain all CurrentPokerTest Unit Tests
    /// </summary>
    [TestClass]
    public class CurrentPokerTest
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

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
        ///     A test for GetTrumpTractor
        /// </summary>
        [TestMethod]
        public void GetTrumpTractorTest()
        {
            var target = new CurrentPoker(new[] {53, 53, 52, 52}, Suit.Heart, 0);
            List<int> actual;
            actual = target.GetTrumpTractor();
            Assert.AreEqual(53, actual[0]);
            Assert.AreEqual(52, actual[1]);

            target = new CurrentPoker(new[] {53, 52, 52, 0, 0, 13}, Suit.Heart, 0);
            actual = target.GetTrumpTractor();
            Assert.AreEqual(52, actual[0]);
            Assert.AreEqual(0, actual[1]);

            target = new CurrentPoker(new[] {53, 52, 0, 0, 13, 26, 26, 12}, Suit.Heart, 0);
            actual = target.GetTrumpTractor();
            Assert.AreEqual(0, actual[0]);
            Assert.AreEqual(26, actual[1]);


            target = new CurrentPoker(new[] {53, 52, 0, 0, 13, 26, 26, 12, 12}, Suit.Heart, 0);
            actual = target.GetTrumpTractor();
            Assert.AreEqual(0, actual[0]);
            Assert.AreEqual(26, actual[1]);
            Assert.AreEqual(12, actual[2]);

            target = new CurrentPoker(new[] {53, 52, 0, 13, 26, 12, 12, 11, 11}, Suit.Heart, 0);
            actual = target.GetTrumpTractor();
            Assert.AreEqual(12, actual[0]);
            Assert.AreEqual(11, actual[1]);

            target = new CurrentPoker(new[] {53, 52, 0, 13, 26, 12, 12, 11, 11, 10, 10}, Suit.Heart, 0);
            actual = target.GetTrumpTractor();
            Assert.AreEqual(12, actual[0]);
            Assert.AreEqual(11, actual[1]);
            Assert.AreEqual(10, actual[2]);

            target = new CurrentPoker(new[] {53, 52, 0, 13, 26, 1, 1, 2, 2}, Suit.Heart, 0);
            actual = target.GetTrumpTractor();
            Assert.AreEqual(2, actual[0]);
            Assert.AreEqual(1, actual[1]);
        }

        /// <summary>
        ///     A test for GetTractor
        /// </summary>
        [TestMethod]
        public void GetTractorTest()
        {
            var target = new CurrentPoker(new[] {53, 52, 0, 13, 26, 12, 12, 11, 11}, Suit.Spade, 0);
            List<int> actual = target.GetTractor(Suit.Heart);
            Assert.AreEqual(12, actual[0]);
            Assert.AreEqual(11, actual[1]);

            target = new CurrentPoker(new[] {53, 52, 0, 13, 26, 12, 12, 11, 11, 10, 10}, Suit.Spade, 0);
            actual = target.GetTractor(Suit.Heart);
            Assert.AreEqual(12, actual[0]);
            Assert.AreEqual(11, actual[1]);
            Assert.AreEqual(10, actual[2]);

            target = new CurrentPoker(new[] {53, 52, 0, 13, 26, 1, 1, 2, 2}, Suit.Spade, 0);
            actual = target.GetTractor(Suit.Heart);
            Assert.AreEqual(2, actual[0]);
            Assert.AreEqual(1, actual[1]);


            target = new CurrentPoker(new[] {53, 52, 0, 0, 13, 26, 1, 1, 2, 2}, Suit.Spade, 1);
            actual = target.GetTractor(Suit.Heart);
            Assert.AreEqual(2, actual[0]);
            Assert.AreEqual(0, actual[1]);
        }

        /// <summary>
        ///     A test for IsMixed
        /// </summary>
        [TestMethod]
        public void IsMixedTest()
        {
            var target = new CurrentPoker(new[] {53, 52, 0, 0, 13, 26, 1, 1, 2, 2}, Suit.Spade, 1);
            Assert.IsTrue(target.IsMixed());
            target = new CurrentPoker(new[] {53, 52, 0, 0, 13, 26, 1, 1, 2, 2}, Suit.Spade, 0);
            Assert.IsTrue(target.IsMixed());
            target = new CurrentPoker(new[] {1, 1, 2, 2}, Suit.Joker, 0);
            Assert.IsFalse(target.IsMixed());
            target = new CurrentPoker(new[] {53, 53, 39, 39, 14}, Suit.Club, 0);
            Assert.IsTrue(target.IsMixed());
        }
    }
}