using MunroLibrary.Interfaces;
using MunroLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MunroLibrary
{
    /// <summary>
    /// Utility class for searching munros
    /// </summary>
    public static class MunroSearcher
    {
        private enum HeightFilterType
        {
            ERROR,
            NONE,
            MIN,
            MAX,
            BOTH
        }

        #region Constructors
        //public MunroSearcher()
        //{

        //}

        #endregion

        /// <summary>
        /// Search in a CSV file containing a list of <see cref="Munro"/> the items matching the given parameters
        /// </summary>
        /// <param name="csvFilePath">String containing the file path of the CSV file</param>
        /// <param name="map">List of pairs of column name-property name to use when reading the CSV file</param>
        /// <param name="hillCategory">Type of hill to search for</param>
        /// <param name="minHeight">Minimum height the munro needs to have</param>
        /// <param name="maxHeight">Maximum height the munro can have</param>
        /// <param name="numberResults">Number of results to be returned</param>
        /// <param name="limitPosition">Where the limiting begins</param>
        /// <param name="sortCriterias">Criterias for sorting the list</param>
        /// <returns>List of <see cref="Munro"/> matching the given parameters</returns>
        public static List<Munro> Search(string csvFilePath, IEnumerable<ColumnToPropertyPair> map = null, Munro.CategoryType hillCategory = Munro.CategoryType.NONE, double minHeight = 0, double maxHeight = 0, int numberResults = 0, LimitResultsFilter.LimitType limitPosition = LimitResultsFilter.LimitType.TOP, params SortCriteria[] sortCriterias)
        {
            var csvReader = new CSVReader(map);
            var results = csvReader.ReadCSV(csvFilePath);

            return Search(results, hillCategory, minHeight, maxHeight, numberResults, limitPosition, sortCriterias);
        }

        /// <summary>
        /// Search in a list of <see cref="Munro"/> the items matching the given parameters
        /// </summary>
        /// <param name="munros">List of the munros</param>
        /// <param name="hillCategory">Type of hill to search for</param>
        /// <param name="minHeight">Minimum height the munro needs to have</param>
        /// <param name="maxHeight">Maximum height the munro can have</param>
        /// <param name="numberResults">Number of results to be returned</param>
        /// <param name="limitPosition">Where the limiting begins</param>
        /// <param name="sortCriterias">Criterias for sorting the list</param>
        /// <returns>List of <see cref="Munro"/> matching the given parameters</returns>
        public static List<Munro> Search(List<Munro> munros, Munro.CategoryType hillCategory = Munro.CategoryType.NONE, double minHeight = 0, double maxHeight = 0, int numberResults = 0, LimitResultsFilter.LimitType limitPosition = LimitResultsFilter.LimitType.TOP, params SortCriteria[] sortCriterias)
        {
            List<Munro> results = munros;

            switch (CheckHeightValues(minHeight, maxHeight))
            {
                case HeightFilterType.ERROR:
                    {
                        throw new ArgumentException("Minimum height can't be greater then maximum height and they both need to be positive numbers");
                    }
                case HeightFilterType.MIN:
                    {
                        var minHeightFilter = new MinHeightFilter(results, minHeight);
                        results = minHeightFilter.GetResults();
                        break;
                    }
                case HeightFilterType.MAX:
                    {
                        var maxHeightFilter = new MinHeightFilter(results, maxHeight);
                        results = maxHeightFilter.GetResults();
                        break;
                    }
                case HeightFilterType.BOTH:
                    {
                        var minHeightFilter = new MinHeightFilter(munros, minHeight);
                        results = minHeightFilter.GetResults();
                        var maxHeightFilter = new MaxHeightFilter(results, maxHeight);
                        results = maxHeightFilter.GetResults();
                        break;
                    }
            }

            var categoryFilter = new HillCategoryFilter(results, hillCategory);
            results = categoryFilter.GetResults();

            var sortFilter = new SortFilter(results, sortCriterias);
            results = sortFilter.GetResults();

            var limitFilter = new LimitResultsFilter(results, numberResults, limitPosition);
            results = limitFilter.GetResults();

            return results;
        }

        /// <summary>
        /// Search by filter(filters can be created using other filters so to be combined, the order of creation matters)
        /// </summary>
        /// <param name="munroFilter"></param>
        /// <returns>List of <see cref="Munro"/> matching the given parameters</returns>
        public static List<Munro> Search(IMunroFilter munroFilter)
        {
            return munroFilter.GetResults();
        }

        /// <summary>
        /// Search in a CSV file containing a list of <see cref="Munro"/> the items loosely matching the given parameters, ignoring invalid values (if maxHeight smaller then minHeight they will be inverted so to keep the correct range)
        /// </summary>
        /// <param name="csvFilePath">String containing the file path of the CSV file</param>
        /// <param name="map">List of pairs of column name-property name to use when reading the CSV file</param>
        /// <param name="hillCategory">Type of hill to search for</param>
        /// <param name="minHeight">Minimum height the munro needs to have</param>
        /// <param name="maxHeight">Maximum height the munro can have</param>
        /// <param name="numberResults">Number of results to be returned</param>
        /// <param name="limitPosition">Where the limiting begins</param>
        /// <param name="sortCriterias">Criterias for sorting the list</param>
        /// <returns>List of <see cref="Munro"/> matching the given parameters</returns>
        public static List<Munro> SearchLoosely(string csvFilePath, IEnumerable<ColumnToPropertyPair> map = null, Munro.CategoryType hillCategory = Munro.CategoryType.NONE, double minHeight = 0, double maxHeight = 0, int numberResults = 0, LimitResultsFilter.LimitType limitPosition = LimitResultsFilter.LimitType.TOP, params SortCriteria[] sortCriterias)
        {
            if (minHeight < 0)
                minHeight = 0;
            if (maxHeight < 0)
                maxHeight = 0;
            if (maxHeight > 0)
            {
                if (maxHeight - minHeight < 0)
                {
                    var temp = maxHeight;
                    maxHeight = minHeight;
                    minHeight = temp;
                }
            }

            if (numberResults < 0)
                numberResults = 0;

            return Search(csvFilePath, map, hillCategory, minHeight, maxHeight, numberResults, limitPosition, sortCriterias);
        }

        /// <summary>
        /// Search in a list of <see cref="Munro"/> the items loosely matching the given parameters, ignoring invalid values (if maxHeight smaller then minHeight they will be inverted so to keep the correct range)
        /// </summary>
        /// <param name="munros">List of the munros</param>
        /// <param name="hillCategory">Type of hill to search for</param>
        /// <param name="minHeight">Minimum height the munro needs to have</param>
        /// <param name="maxHeight">Maximum height the munro can have</param>
        /// <param name="numberResults">Number of results to be returned</param>
        /// <param name="limitPosition">Where the limiting begins</param>
        /// <param name="sortCriterias">Criterias for sorting the list</param>
        /// <returns>List of <see cref="Munro"/> matching the given parameters</returns>
        public static List<Munro> SearchLoosely(List<Munro> munros, Munro.CategoryType hillCategory = Munro.CategoryType.NONE, double minHeight = 0, double maxHeight = 0, int numberResults = 0, LimitResultsFilter.LimitType limitPosition = LimitResultsFilter.LimitType.TOP, params SortCriteria[] sortCriterias)
        {
            if (minHeight < 0)
                minHeight = 0;
            if (maxHeight < 0)
                maxHeight = 0;
            if(maxHeight>0)
            {
                if(maxHeight-minHeight<0)
                {
                    var temp = maxHeight;
                    maxHeight = minHeight;
                    minHeight = temp;
                }
            }

            if (numberResults < 0)
                numberResults = 0;

            return Search(munros, hillCategory, minHeight, maxHeight, numberResults, limitPosition, sortCriterias);
        }

        #region Helpers

        private static HeightFilterType CheckHeightValues(double minHeight, double maxHeight)
        {
            HeightFilterType heightFilterCase;
            if (minHeight >= 0 && maxHeight >= 0)
            {
                var sub = maxHeight - minHeight;
                if (sub > 0)
                {
                    heightFilterCase = HeightFilterType.BOTH;
                }
                else
                {
                    if (sub == 0)
                    {
                        heightFilterCase = HeightFilterType.NONE;
                    }
                    else if (minHeight == Math.Abs(sub))
                    {
                        heightFilterCase = HeightFilterType.MIN;
                    }
                    else if (maxHeight == Math.Abs(sub))
                    {
                        heightFilterCase = HeightFilterType.MAX;
                    }
                    else
                    {
                        heightFilterCase = HeightFilterType.ERROR;
                    }
                }
            }
            else
            {
                heightFilterCase = HeightFilterType.ERROR;
            }

            return heightFilterCase;
        }

        #endregion
    }
}
