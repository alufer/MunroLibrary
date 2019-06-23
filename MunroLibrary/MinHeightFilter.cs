using MunroLibrary.Interfaces;
using MunroLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MunroLibrary
{
    /// <summary>
    /// Filters a list of <see cref="Munro"/> by being at least tall as the specific value
    /// </summary>
    public class MinHeightFilter:HeightFilter
    {
        #region Constructors

        public MinHeightFilter():base()
        {

        }

        public MinHeightFilter(List<Munro> munros, double height = 0) : base(munros, height)
        {

        }

        public MinHeightFilter(IMunroFilter filterMunrosSource, double height = 0) : base(filterMunrosSource, height)
        {

        }

        #endregion

        #region Overrides

        public override List<Munro> GetResults()
        {
            if (IsListMunrosEmpty() || Height == 0)
            {
                return base.GetResults();
            }
            return Munros.Where(x => x.Height >= Height).ToList();
        }

        #endregion
    }
}
