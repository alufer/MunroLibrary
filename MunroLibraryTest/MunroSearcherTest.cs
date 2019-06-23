using MunroLibrary;
using MunroLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System;
using System.IO;

namespace MunroLibraryTest
{
    [TestFixture]
    public class MunroSearcherTest
    {
        [TestCase(@"file.csv")]
        [TestCase(@"1\")]
        public void Search_InvalidFilePath_ThrowFileNotFoundException(string filePath)
        {
            Assert.Throws<FileNotFoundException>(()=> MunroSearcher.Search(filePath));
        }

        [TestCase(@"MockCSV\munrotabMockValid.csv")]
        public void Search_InvalidFilePath_ThrowDirectoryNotFoundException(string filePath)
        {
            Assert.Throws<DirectoryNotFoundException>(() => MunroSearcher.Search(filePath));
        }

        [TestCase(-5,0)]
        [TestCase(0,-5)]
        [TestCase(-12,-5)]
        [TestCase(12,5)]
        public void Search_MinHeightGreaterMaxHeightOrNegative_ThrowArgumentException(double minHeight, double maxHeight)
        {
            var ex=Assert.Throws<ArgumentException>(()=> MunroSearcher.Search(null, default, minHeight, maxHeight));
            Assert.AreEqual("Minimum height can't be greater then maximum height and they both need to be positive numbers", ex.Message);
        }

        [TestCase(-12)]
        public void Search_LimitResultsNegative_ThrowArgumentException(int numberResults)
        {
            var ex = Assert.Throws<ArgumentException>(() => MunroSearcher.Search(null, default, default, default,numberResults));
            Assert.AreEqual("The number of results limited can't be negative", ex.Message);
        }

        [TestCase(-5, 0)]
        [TestCase(0, -5)]
        [TestCase(-12, -5)]
        [TestCase(12, 5)]
        public void SearchLoosley_MinHeightGreaterMaxHeightOrNegative_DoesntThrowException(double minHeight, double maxHeight)
        {
             Assert.DoesNotThrow(() => MunroSearcher.SearchLoosely(null, default, minHeight, maxHeight));
        }

        [TestCase(-12)]
        public void SearchLoosely_LimitResultsNegative_DoesntThrowException(int numberResults)
        {
            Assert.DoesNotThrow(() => MunroSearcher.SearchLoosely(null, default, default, default,numberResults));
        }
    }
}
