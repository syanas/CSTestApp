using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using App;

namespace AppTests
{
    
    [TestClass]
    public class ExtremaFinderTests
    {
        [TestMethod]
        public void Test_ExtremaFinder_EmptyInput()
        {
            var source_data = new List<Int64>();
            var expected_data = new List<int>();
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_OneElement()
        {
            var source_data = new List<Int64>() { 5 };
            var expected_data = new List<int>();
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_TwoElements()
        {
            var source_data = new List<Int64>() { 1, 3};
            var expected_data = new List<int>() { 0, 1};
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_AllEqualElements()
        {
            var source_data = new List<Int64>() { -1, -1, -1, -1, -1, -1, -1, -1 };
            var expected_data = new List<int>();
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_IncreasingElements()
        {
            var source_data = new List<Int64>() { -1, 0, 1, 2, 3, 4};
            var expected_data = new List<int>() { 0, 5};
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_DecreasingElements()
        {
            var source_data = new List<Int64>() { 4, 3, 2, 1, 0, -1 };
            var expected_data = new List<int>() { 0, 5};
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_NonstrictIncreasingElements()
        {
            var source_data = new List<Int64>() { 1, 1, 1, 3, 3 };
            var expected_data = new List<int>();
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_NonstrictDecreasingElements()
        {
            var source_data = new List<Int64>() { 3, 3, 1, 1, 1 };
            var expected_data = new List<int>();
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_HasMinInTheCentreAndBorderMaxs()
        {
            var source_data = new List<Int64>() { 1, 0, 1, 3 };
            var expected_data = new List<int>() { 0, 1, 3 };
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_HasMinInTheCentreAndBorderMaxs2()
        {
            var source_data = new List<Int64>() { 1, 0, 0, -1, 0, 3 };
            var expected_data = new List<int>() { 0, 3, 5 };
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }

        [TestMethod]
        public void Test_ExtremaFinder_HasMaxSegment()
        {
            var source_data = new List<Int64>() { -1, 0, 1, 1, -3 };
            var expected_data = new List<int>() { 0, 2, 4 };
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }
        
        [TestMethod]
        public void Test_ExtremaFinder_HasThreeMinsAndOneMax()
        {
            var source_data = new List<Int64>() { 1, 0, 0, 1, 0, 3 };
            var expected_data = new List<int>() { 0, 1, 3, 4, 5};
            var extrema_indices = new List<int>();
            SecondSolution.ExtremaFinder.GetNonStrictExtremaIndices(source_data, extrema_indices);
            CollectionAssert.AreEqual(expected_data, extrema_indices);
        }
    }

    [TestClass]
    public class DataManipulatorTests {

        [TestMethod]
        public void Test_DataManipulator_ShiftRight()
        {
            Double a = 0;
            var source_data = new List<Double>() { 1, 2, 3, 4};
            var expected_data = new List<Double>() {2, 3, 4, 4};
            var smoothed_data = new List<Double>();
            SecondSolution.DataManipulator.SmoothData(a, source_data, smoothed_data);
            CollectionAssert.AreEqual(expected_data, smoothed_data);
        }

        [TestMethod]
        public void Test_DataManipulator_SameAsInput()
        {
            Double a = 1;
            var source_data = new List<Double>() { 1, 2, 3, 4 };
            var expected_data = new List<Double>() { 1, 2, 3, 4 };
            var smoothed_data = new List<Double>();
            SecondSolution.DataManipulator.SmoothData(a, source_data, smoothed_data);
            CollectionAssert.AreEqual(expected_data, smoothed_data);
        }

        [TestMethod]
        public void Test_DataManipulator_OneElement()
        {
            Double a = 0.5;
            var source_data = new List<Double>() { 1};
            var expected_data = new List<Double>() { 1 };
            var smoothed_data = new List<Double>();
            SecondSolution.DataManipulator.SmoothData(a, source_data, smoothed_data);
            CollectionAssert.AreEqual(expected_data, smoothed_data);
        }
    }
}
