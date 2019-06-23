using MunroLibrary.Interfaces;
using MunroLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MunroLibrary
{
    /// <summary>
    /// Sorts a list of <see cref="Munro"/> with multiple criterias
    /// </summary>
    public class SortFilter : BaseMunroFilter
    {

        #region Properties

        List<SortCriteria> SortCriterias { get; set; }

        #endregion

        #region Constructors

        public SortFilter() : base()
        {

        }

        public SortFilter(List<Munro> munros, params SortCriteria[] sortCriterias) : base(munros)
        {
            SortCriterias = sortCriterias.ToList();
        }

        public SortFilter(IMunroFilter filterMunrosSource, params SortCriteria[] sortCriterias) : base(filterMunrosSource)
        {
            SortCriterias = sortCriterias.ToList();

        }

        #endregion

        #region Overrides

        public override List<Munro> GetResults()
        {
            if (IsListMunrosEmpty() || SortCriterias?.Count == 0)
            {
                return base.GetResults();
            }

            var firstCriteria = SortCriterias.First();
            SortCriterias.RemoveAt(0);

            IOrderedEnumerable<Munro> orderable;

            if (firstCriteria.Order == SortCriteria.OrderType.ASCENDING)
            {
                orderable = SortBy(firstCriteria, Munros);
            }
            else
            {
                orderable = SortByDescending(firstCriteria, Munros);
            }

            foreach (var criteria in SortCriterias)
            {
                if (criteria.Order == SortCriteria.OrderType.ASCENDING)
                {
                    orderable = SortThenBy(criteria, orderable);
                }
                else
                {
                    orderable = SortThenByDescending(criteria, orderable);
                }
            }

            return orderable.ToList();
        }

        #endregion

        #region Helpers

        private IOrderedEnumerable<Munro> SortBy(SortCriteria criteria, List<Munro> munros)
        {
            IOrderedEnumerable<Munro> orderable = null;

            switch (criteria.PropertyCriteria)
            {
                case SortCriteria.PropertyCriteriaType.ALPHABETICALLY:
                    {
                        orderable = munros.OrderBy(x => x.Name);
                        break;
                    }
                case SortCriteria.PropertyCriteriaType.HEIGHT:
                    {
                        orderable = munros.OrderBy(x => x.Height);
                        break;
                    }
                default:
                    break;
            }

            return orderable;
        }

        private IOrderedEnumerable<Munro> SortByDescending(SortCriteria criteria, List<Munro> munros)
        {
            IOrderedEnumerable<Munro> orderable = null;

            switch (criteria.PropertyCriteria)
            {
                case SortCriteria.PropertyCriteriaType.ALPHABETICALLY:
                    {
                        orderable = munros.OrderByDescending(x => x.Name);
                        break;
                    }
                case SortCriteria.PropertyCriteriaType.HEIGHT:
                    {
                        orderable = munros.OrderByDescending(x => x.Height);
                        break;
                    }
                default:
                    break;
            }

            return orderable;
        }

        private IOrderedEnumerable<Munro> SortThenBy(SortCriteria criteria, IOrderedEnumerable<Munro> munros)
        {
            IOrderedEnumerable<Munro> orderable = null;

            switch (criteria.PropertyCriteria)
            {
                case SortCriteria.PropertyCriteriaType.ALPHABETICALLY:
                    {
                        orderable = munros.ThenBy(x => x.Name);
                        break;
                    }
                case SortCriteria.PropertyCriteriaType.HEIGHT:
                    {
                        orderable = munros.ThenBy(x => x.Height);
                        break;
                    }
                default:
                    break;
            }

            return orderable;
        }

        private IOrderedEnumerable<Munro> SortThenByDescending(SortCriteria criteria, IOrderedEnumerable<Munro> munros)
        {
            IOrderedEnumerable<Munro> orderable = null;

            switch (criteria.PropertyCriteria)
            {
                case SortCriteria.PropertyCriteriaType.ALPHABETICALLY:
                    {
                        orderable = munros.ThenByDescending(x => x.Name);
                        break;
                    }
                case SortCriteria.PropertyCriteriaType.HEIGHT:
                    {
                        orderable = munros.ThenByDescending(x => x.Height);
                        break;
                    }
                default:
                    break;
            }

            return orderable;
        }

        #endregion
    }
}
