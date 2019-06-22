using MunroLibrary.Interfaces;
using MunroLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MunroLibrary
{
    /// <summary>
    /// Filters a list of <see cref="Munro"/> by the category of the hill
    /// </summary>
    public class HillCategoryFilter : BaseMunroFilter
    {
        /// <summary>
        /// Category of the hill( default value <see cref="Munro.CategoryType.NONE"/>)
        /// </summary>
        public Munro.CategoryType HillCategory { get; set; }

        /// <summary>
        /// Instantiate the filter with default value for <see cref="HillCategory"/>
        /// </summary>
        public HillCategoryFilter() : base()
        {
            HillCategory = Munro.CategoryType.NONE;
        }

        /// <summary>
        /// Instantiate the filter with the given list of <see cref="Munro"/> for <see cref="BaseMunroFilter.Munros"/> and the given value for <see cref="HillCategory"/>
        /// </summary>
        public HillCategoryFilter(List<Munro> munros, Munro.CategoryType category = Munro.CategoryType.NONE) : base(munros)
        {
            HillCategory = category;
        }

        /// <summary>
        /// Instantiate the filter with another filter for <see cref="BaseMunroFilter.Munros"/> and the given value for <see cref="HillCategory"/>
        /// </summary>
        public HillCategoryFilter(IMunroFilter filterMunrosSource, Munro.CategoryType category = Munro.CategoryType.NONE) : base(filterMunrosSource)
        {
            HillCategory = category;
        }

        /// <summary>
        /// Get the list of <see cref="Munro"/> that matches the hill category <see cref="HillCategory"/>( <see cref="Munro.CategoryType.NONE"/> matches all categories ) from <see cref="BaseMunroFilter.Munros"/>
        /// </summary>
        /// <returns>List of matching <see cref="Munro"/></returns>
        public override List<Munro> GetResults()
        {
            if (IsListMunrosEmpty() || HillCategory == Munro.CategoryType.NONE)
            {
                return base.GetResults();
            }
            return Munros.Where(x => x.HillCategory == this.HillCategory).ToList();
        }

    }
}
