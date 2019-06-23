using System;
using System.Collections.Generic;
using System.Linq;
using MunroLibrary;
using MunroLibrary.Models;
using NUnit.Framework;

namespace MunroLibraryTest
{
    [TestFixture]
    public class HillCategoryFilterTest
    {
        [Test]
        public void HillCategoryFilter_NoHillCategory_InstantiateDefault()
        {
            var expected = Munro.CategoryType.NONE;

            var filter = new HillCategoryFilter();
            Assert.AreEqual(expected, filter.HillCategory);

        }

        static object[] munrosList =
        {
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
            },
            new List<Munro>()
            {
                new Munro(){Name=null, Height= 1931,GridReference=null,HillCategory=Munro.CategoryType.MUN },
                new Munro(){Name=null, Height= 931,GridReference=null,HillCategory=Munro.CategoryType.MUN },
            }
        };

        [TestCaseSource("munrosList")]
        public void GetResults_NoHillCategory_CompleteList(List<Munro> munros)
        {

            var filter = new HillCategoryFilter(munros);
            var actual = filter.GetResults();
            CollectionAssert.AreEquivalent(munros, actual);

        }

        [TestCaseSource("munrosList")]
        public void GetResults_HillCategoryMun_OnlyMunMunrosList(List<Munro> munros)
        {

            var filter = new HillCategoryFilter(munros,Munro.CategoryType.MUN);
            var actualList = filter.GetResults();
            var actual= actualList.Where(x => x.HillCategory != Munro.CategoryType.MUN).FirstOrDefault();
            Assert.IsNull(actual);

        }

        [TestCaseSource("munrosList")]
        public void GetResults_HillCategoryTop_OnlyTopMunrosList(List<Munro> munros)
        {

            var filter = new HillCategoryFilter(munros, Munro.CategoryType.TOP);
            var actualList = filter.GetResults();
            var actual = actualList.Where(x => x.HillCategory != Munro.CategoryType.TOP).FirstOrDefault();
            Assert.IsNull(actual);

        }
    }
}
