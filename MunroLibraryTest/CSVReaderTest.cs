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
    public class CSVReaderTest
    {

        [Test]
        public void CSVReader_ParameterMapEmpty_IstantiateWithDefaultMap()
        {
            var defaultMap = new List<ColumnToPropertyPair>
            {
                new ColumnToPropertyPair("Name", "Name"),
                new ColumnToPropertyPair("Height (m)", "Height"),
                new ColumnToPropertyPair("Post 1997", "HillCategory"),
                new ColumnToPropertyPair("Grid Ref", "GridReference")
            };
            var csvReader = new CSVReader();
            CollectionAssert.AreEquivalent(defaultMap, csvReader.ColumnToPropertyMap);

            //Assert.IsTrue(defaultMap.SequenceEqual( csvReader.ColumnToPropertyMap));
        }

        [Test]
        public void CSVReader_ParameterMapNotEmpty_IstantiateWithParameterMap()
        {
            var parameterMap = new List<ColumnToPropertyPair>
            {
                new ColumnToPropertyPair("Name", "Name"),
                new ColumnToPropertyPair("Height (m)", "Height"),
            };
            var csvReader = new CSVReader(parameterMap);
            CollectionAssert.AreEquivalent(parameterMap, csvReader.ColumnToPropertyMap);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("     ")]
        public void ReadCSV_FilePathEmpty_ThrowArgumentException(string filePath)
        {
            var csvReader = new CSVReader();
            Assert.Throws<ArgumentException>(() => csvReader.ReadCSV(filePath));
        }

        [TestCase(@"file.csv")]
        [TestCase(@"1\")]
        public void ReadCSV_FilePathInvalid_ThrowFileNotFoundException(string filePath)
        {
            string completePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../../" + filePath));

            var csvReader = new CSVReader();
            Assert.Throws<FileNotFoundException>(() => csvReader.ReadCSV(completePath));
        }

        [TestCase(@"MockCSV\munrotabMockValid.csv")]
        public void ReadCSV_FilePathInvalid_ThrowDirectoryNotFoundException(string filePath)
        {
            string completePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../../" + filePath));

            var csvReader = new CSVReader();
            Assert.Throws<DirectoryNotFoundException>(() => csvReader.ReadCSV(completePath));
        }

        [TestCase(@"CSVFileTests\munrotabMockMissingColumns.csv")]
        public void ReadCSV_ColumnMapNotInHeaders_ThrowException(string filePath)
        {
            string completePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../../" + filePath));

            var csvReader = new CSVReader();
            var ex = Assert.Throws<Exception>(() => csvReader.ReadCSV(completePath));
            Assert.AreEqual("Column not found in the file", ex.Message);
        }

        static IEnumerable<object[]> Get_ValidCSV_ExpectedList()
        {
            yield return new object[]
            {
                @"CSVFileTests\munrotabMockValid.csv",
                null,
                new List<Munro>()
                {
                    new Munro(){Name="Ben Chonzie", Height= 1931,GridReference="NN773308",HillCategory=Munro.CategoryType.MUN },
                    new Munro(){Name="Ben Chonzie", Height= 931,GridReference="NN773308",HillCategory=Munro.CategoryType.MUN },
                    new Munro(){Name="Ben", Height= 931,GridReference="NN773308",HillCategory=Munro.CategoryType.MUN },
                    new Munro(){Name="Ben Vorlich", Height= 985,GridReference="NN629189",HillCategory=Munro.CategoryType.MUN },
                    new Munro(){Name="Stuc a' Chroin", Height= 975,GridReference="NN617174",HillCategory=Munro.CategoryType.MUN },
                    new Munro(){Name="Ben Lomond", Height= 974,GridReference="NN367028",HillCategory=Munro.CategoryType.MUN },
                    new Munro(){Name="Ben More", Height= 1174,GridReference="NN432244",HillCategory=Munro.CategoryType.MUN },
                    new Munro(){Name="Stob Binnein", Height= 1165,GridReference="NN434227",HillCategory=Munro.CategoryType.MUN },
                    new Munro(){Name="Stob Binnein - Stob Coire an Lochain", Height= 1068,GridReference="NN438220",HillCategory=Munro.CategoryType.TOP },
                    new Munro(){Name="Stob Binnein - Meall na Dige", Height= 966,GridReference="NN450225",HillCategory=Munro.CategoryType.TOP },
                }
            };
            yield return new object[]
            {
                @"CSVFileTests\munrotabMockValidShort.csv",
                new List<ColumnToPropertyPair>
                {
                    new ColumnToPropertyPair("Height (m)", "Height"),
                    new ColumnToPropertyPair("Post 1997", "HillCategory"),
                },
                new List<Munro>()
                {
                    new Munro(){Name=null, Height= 1931,GridReference=null,HillCategory=Munro.CategoryType.MUN },
                    new Munro(){Name=null, Height= 931,GridReference=null,HillCategory=Munro.CategoryType.MUN },
                }
            };
        }

        [Test, TestCaseSource("Get_ValidCSV_ExpectedList")]
        public void ReadCSV_ValidCSV_ExpectedList(string filePath, IEnumerable<ColumnToPropertyPair> map, List<Munro> expectedList)
        {
            string completePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../../" + filePath));

            var csvReader = new CSVReader(map);
            var actual = csvReader.ReadCSV(completePath);
            Assert.AreEqual(expectedList, actual);
        }
    }
}