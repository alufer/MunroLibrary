using MunroLibrary.Interfaces;
using MunroLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MunroLibrary
{
    class HillCategoryFilter : BaseMunroFilter
    {
        public Munro.CategoryType HillCategory { get; set; }

        public HillCategoryFilter() : base()
        {
            HillCategory = Munro.CategoryType.NONE;
        }

        public HillCategoryFilter(List<Munro> munros, Munro.CategoryType category = Munro.CategoryType.NONE) : base(munros)
        {

            HillCategory = category;
        }

        public HillCategoryFilter(IMunroFilter filterMunrosSource, Munro.CategoryType category = Munro.CategoryType.NONE) : base(filterMunrosSource)
        {

            HillCategory = category;
        }

        public override List<Munro> GetResults()
        {
            if (IsListMunrosEmpty() || HillCategory == Munro.CategoryType.NONE)
            {
                base.GetResults();
            }
            return new List<Munro>(Munros.Where(x => x.HillCategory == HillCategory));
        }

    }
}
