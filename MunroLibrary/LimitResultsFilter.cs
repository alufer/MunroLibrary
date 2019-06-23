using MunroLibrary.Interfaces;
using MunroLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MunroLibrary
{
    /// <summary>
    /// Limit the number of elements retrievied from a list of <see cref="Munro"/> from the top or the bottom
    /// </summary>
    public class LimitResultsFilter : BaseMunroFilter
    {
        public enum LimitType
        {
            TOP,
            BOTTOM
        }

        #region Properties

        private int _numberResults;

        public int NumberResults
        {
            get { return _numberResults; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("The number of results limited can't be negative");
                _numberResults = value;
            }
        }

        public LimitType LimitPosition { get; set; }

        #endregion

        #region Constructors

        public LimitResultsFilter() : base()
        {
            NumberResults = 0;
            LimitPosition = LimitType.TOP;
        }

        public LimitResultsFilter(List<Munro> munros, int numberResults = 0, LimitType limitPosition = LimitType.TOP) : base(munros)
        {
            NumberResults = numberResults;
            LimitPosition = limitPosition;
        }

        public LimitResultsFilter(IMunroFilter filterMunrosSource, int numberResults = 0, LimitType limitPosition = LimitType.TOP) : base(filterMunrosSource)
        {
            NumberResults = numberResults;
            LimitPosition = limitPosition;
        }

        #endregion

        #region Overrides

        public override List<Munro> GetResults()
        {
            if (IsListMunrosEmpty() || NumberResults == 0)
            {
                return base.GetResults();
            }
            if (LimitPosition == LimitType.TOP)
            {
                return Munros.Take(NumberResults).ToList();
            }
            else
            {
                var itemsToSkip = Munros.Count - NumberResults;
                return Munros.Skip(itemsToSkip).ToList();
            }
        }

        #endregion
    }
}
