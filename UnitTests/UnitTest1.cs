using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberCore2;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class UnitTestPuzzle
    {
        [TestMethod]
        public void TestPermutations()
        {
            var puzzle = new Puzzle
            {
                Numbers = new List<int> { 1, 2, 3 },
            };

            List<List<int>> expected = new List<List<int>>
            {
                new List<int> { 1 },
                new List<int> { 2 },
                new List<int> { 1, 2 },
                new List<int> { 2, 1 },
                new List<int> { 3 },
                new List<int> { 1, 3 },
                new List<int> { 3, 1 },
                new List<int> { 2, 3 },
                new List<int> { 3, 2 },
                new List<int> { 1, 2, 3 },
                new List<int> { 2, 1, 3 },
                new List<int> { 1, 3, 2 },
                new List<int> { 3, 1, 2 },
                new List<int> { 2, 3, 1 },
                new List<int> { 3, 2, 1 },
            };
            
            for (int i = 0; i < puzzle.Permutations.Count; i++)
            {
                var test = puzzle.Permutations.ElementAt(i);
                var expect = expected.ElementAt(i);
                if (!test.SequenceEqual(expect))
                {
                    Assert.Fail($"Permutations for {{1, 2, 3}} do not match expected.\n{test}\n{expect}");
                }
            }
        }

        [TestMethod]
        public void TestAnswers()
        {
            var puzzle = new Puzzle
            {
                Numbers = new List<int> { 1, 2, 3 },
            };

            var answerCount = puzzle.PossibleAnswers.Count;
            Assert.AreEqual(77, answerCount, $"Number or answers for {{1, 2, 3}} was not correct.\nHave {answerCount}");

            var answerMax = puzzle.PossibleAnswers.Max(a => a.Value);
            var max = puzzle.PossibleAnswers.OrderByDescending(a => a.Value).FirstOrDefault();
            Assert.AreEqual(9, answerMax, $"Highest possible answer for {{1, 2, 3}} was not correct.\nMax {answerMax}");            
        }


        [TestMethod]
        public void TestTarget()
        {
            var puzzle = new Puzzle
            {
                Numbers = new List<int> { 10, 2, 4, 30 },
                Target = 73
            };
            
            Assert.AreEqual(2, puzzle.ExactAnswers.Count, $"Number of correct answers for puzzle is off.");
        }


        [TestMethod]
        public void TestRandom()
        {
            var puzzle = new Puzzle(2);
            
        }
    }
}
