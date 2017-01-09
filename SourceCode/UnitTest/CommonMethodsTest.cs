using System.Collections.Generic;
using Duan.Xiugang.Tractor.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    /// <summary>
    ///     This is a test class for CommonMethodsTest and is intended
    ///     to contain all CommonMethodsTest Unit Tests
    /// </summary>
    [TestClass]
    public class CommonMethodsTest
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
        ///     A test for GetMaxCard
        /// </summary>
        [TestMethod]
        public void GetMaxCardTest()
        {
            var cards = new List<int> {0, 1, 2};
            var trump = Suit.Heart;
            int rank = 0;
            Assert.AreEqual(0, CommonMethods.GetMaxCard(cards, trump, rank));

            cards = new List<int> {0, 1, 2, 52};
            Assert.AreEqual(52, CommonMethods.GetMaxCard(cards, trump, rank));

            cards = new List<int> {0, 1, 2, 52, 14};
            Assert.AreEqual(-1, CommonMethods.GetMaxCard(cards, trump, rank));

            cards = new List<int> {11, 12, 12, 10};
            Assert.AreEqual(12, CommonMethods.GetMaxCard(cards, trump, rank));

            cards = new List<int> {21, 22, 22, 24};
            Assert.AreEqual(24, CommonMethods.GetMaxCard(cards, trump, rank));
        }
    }
}