using System;
using System.Collections.Generic;
using System.Linq;
using MunroLibrary.Models;
using NUnit.Framework;

namespace MunroLibraryTest
{
    [TestFixture]
    public class MunroTest
    {
        [TestCase("Munro", "1000", "", "Ref")]
        [TestCase("Munro", "1000", " ", "Ref")]
        [TestCase("Munro", "1000", "null", "Ref")]
        [TestCase("Munro", "1000", "NONE", "Ref")]
        public void Munro_InvalidHillCategory_ThrowArgumentException(params string[] values)
        {
            var munro = new Munro();

            var map = new Dictionary<int, ColumnToPropertyPair>
            {
                { 0, new ColumnToPropertyPair("Name", "Name") },
                { 1, new ColumnToPropertyPair("Height (m)", "Height") },
                { 2, new ColumnToPropertyPair("Post 1997", "HillCategory") },
                { 3, new ColumnToPropertyPair("Grid Ref", "GridReference") }
            };
            var ex = Assert.Throws<ArgumentException>(() => new Munro(values.ToList(), map));
            Assert.AreEqual(nameof(munro.HillCategory), ex.ParamName);
        }

        [TestCase("Munro", "0", "MUN", "Ref")]
        [TestCase("Munro", "-5", "MUN", "Ref")]
        [TestCase("Munro", "null", "MUN", "Ref")]
        [TestCase("Munro", "", "MUN", "Ref")]
        public void Munro_InvalidHeight_ThrowArgumentException(params string[] values)
        {
            var munro = new Munro();

            var map = new Dictionary<int, ColumnToPropertyPair>
            {
                { 0, new ColumnToPropertyPair("Name", "Name") },
                { 1, new ColumnToPropertyPair("Height (m)", "Height") },
                { 2, new ColumnToPropertyPair("Post 1997", "HillCategory") },
                { 3, new ColumnToPropertyPair("Grid Ref", "GridReference") }
            };
            var ex = Assert.Throws<ArgumentException>(() => new Munro(values.ToList(), map));
            Assert.AreEqual(nameof(munro.Height), ex.ParamName);
        }

        [Test]
        public void Munro_EmptyValues_ThrowArgumentException()
        {
            var values = new List<string>();
            var map = new Dictionary<int, ColumnToPropertyPair>
            {
                { 0, new ColumnToPropertyPair("Name", "Name") },
                { 1, new ColumnToPropertyPair("Height (m)", "Height") },
                { 2, new ColumnToPropertyPair("Post 1997", "HillCategory") },
                { 3, new ColumnToPropertyPair("Grid Ref", "GridReference") }
            };

            var ex = Assert.Throws<ArgumentException>(() => new Munro(values, map));
            Assert.AreEqual("values", ex.ParamName);
            ex = Assert.Throws<ArgumentException>(() => new Munro(null, map));
            Assert.AreEqual("values", ex.ParamName);
        }

        [Test]
        public void Munro_EmptyMap_ThrowArgumentException()
        {
            var values = new List<string>() { "Munro", "1000", "MUN", "Ref" };
            var map = new Dictionary<int, ColumnToPropertyPair>();

            var ex = Assert.Throws<ArgumentException>(() => new Munro(values, map));
            Assert.AreEqual("columnsMap", ex.ParamName);
            ex = Assert.Throws<ArgumentException>(() => new Munro(values, null));
            Assert.AreEqual("columnsMap", ex.ParamName);
        }

        [TestCase("Munro", "1000", "MUN", "Ref")]
        [TestCase("Munro", "1000", "TOP", "Ref")]
        [TestCase("", "1000", "MUN", "Ref")]
        [TestCase("Munro", "1000", "MUN", "")]
        [TestCase("", "1000", "MUN", "")]
        public void Munro_ValidValues_CreateExpectedMunro(params string[] values)
        {
            var expected = new Munro()
            {
                Name = values[0],
                Height = double.Parse(values[1]),
                HillCategory = (Munro.CategoryType)Enum.Parse(typeof(Munro.CategoryType), values[2]),
                GridReference = values[3]
            };

            var map = new Dictionary<int, ColumnToPropertyPair>
            {
                { 0, new ColumnToPropertyPair("Name", "Name") },
                { 1, new ColumnToPropertyPair("Height (m)", "Height") },
                { 2, new ColumnToPropertyPair("Post 1997", "HillCategory") },
                { 3, new ColumnToPropertyPair("Grid Ref", "GridReference") }
            };

            var actual = new Munro(values.ToList(), map);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Munro_MissingHillCategoryMapping_ThrowArgumentException()
        {
            var munro = new Munro();

            var values = new List<string>() { "Munro", "1000", "MUN", "Ref" };

            var map =
            new Dictionary<int, ColumnToPropertyPair>()
            {
                { 0, new ColumnToPropertyPair("Name", "Name") },
                { 1, new ColumnToPropertyPair("Height (m)", "Height") },
                { 3, new ColumnToPropertyPair("Grid Ref", "GridReference") }
            };

            var ex = Assert.Throws<ArgumentException>(() => new Munro(values, map));
            Assert.AreEqual(nameof(munro.HillCategory), ex.ParamName);

        }

        [Test]
        public void Munro_MissingHeightMapping_ThrowArgumentException()
        {
            var munro = new Munro();

            var values = new List<string>() { "Munro", "1000", "MUN", "Ref" };

            var map =
            new Dictionary<int, ColumnToPropertyPair>
            {
                { 0, new ColumnToPropertyPair("Name", "Name") },
                { 2, new ColumnToPropertyPair("Post 1997", "HillCategory") },
                { 3, new ColumnToPropertyPair("Grid Ref", "GridReference") }
            };

            var ex = Assert.Throws<ArgumentException>(() => new Munro(values, map));
            Assert.AreEqual(nameof(munro.Height), ex.ParamName);

        }

        [Test]
        public void Munro_MissingNameMapping_ExpectedMunro()
        {
            var expected = new Munro()
            {
                Name = null,
                Height = 1000,
                HillCategory = Munro.CategoryType.MUN,
                GridReference = "Ref"
            };

            var values = new List<string>() { "Munro", "1000", "MUN", "Ref" };

            var map =
            new Dictionary<int, ColumnToPropertyPair>
            {
                { 1, new ColumnToPropertyPair("Height (m)", "Height") },
                { 2, new ColumnToPropertyPair("Post 1997", "HillCategory") },
                { 3, new ColumnToPropertyPair("Grid Ref", "GridReference") }
            };

            var actual = new Munro(values, map);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Munro_MissingGridRefMapping_ExpectedMunro()
        {
            var expected = new Munro()
            {
                Name = "Munro",
                Height = 1000,
                HillCategory = Munro.CategoryType.MUN,
                GridReference = null
            };

            var values = new List<string>() { "Munro", "1000", "MUN", "Ref" };

            var map =
            new Dictionary<int, ColumnToPropertyPair>
            {
                { 0, new ColumnToPropertyPair("Name", "Name") },
                { 1, new ColumnToPropertyPair("Height (m)", "Height") },
                { 2, new ColumnToPropertyPair("Post 1997", "HillCategory") },
            };

            var actual = new Munro(values, map);

            Assert.AreEqual(expected, actual);
        }
    }
}
