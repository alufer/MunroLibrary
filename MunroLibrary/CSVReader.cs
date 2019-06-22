using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using MunroLibrary.Models;

namespace MunroLibrary
{
    /// <summary>
    /// Class for reading CSV files and obtain a list of values
    /// </summary>
    public class CSVReader
    {
        #region Properties

        private Dictionary<int, ColumnToPropertyPair> _columnsMap;
        public List<ColumnToPropertyPair> ColumnToPropertyMap { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a CSVReader object containing the specified mapping or the default one
        /// </summary>
        /// <param name="map">List of pairs of column name-property name to use when reading the CSV file</param>
        public CSVReader(IEnumerable<ColumnToPropertyPair> map = null)
        {            

            if (map == null)
            {
                ColumnToPropertyMap = new List<ColumnToPropertyPair>
                {
                    new ColumnToPropertyPair("Name", "Name"),
                    new ColumnToPropertyPair("Height (m)", "Height"),
                    new ColumnToPropertyPair("Post 1997", "HillCategory"),
                    new ColumnToPropertyPair("Grid Ref", "GridReference")
                };
            }
            else
            {
                ColumnToPropertyMap = new List<ColumnToPropertyPair>(map);
            }
        }

        #endregion

        /// <summary>
        /// Read a CSV file with a first line as header extracting only the values based on the mapping and create a corresponding list of <see cref="Munro"/>
        /// </summary>
        /// <param name="filePath">String containing the file path of the CSV file</param>
        /// <returns>List of qualifing <see cref="Munro"/> based on the mapping</returns>
        public List<Munro> ReadCSV(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Path for CSV file can't be empty", nameof(filePath));

            List<Munro> results = new List<Munro>();
            
            try
            {
                List<string> lines = new List<string>(File.ReadAllLines(filePath));
                if (lines.Count == 0 || lines == null)
                {
                    Console.WriteLine("File empty");
                    return null;
                }

                //Pop the header from the list
                var headers = lines[0];
                lines.RemoveAt(0);

                _columnsMap = CreateIndexColumnPropertyPairMap(headers);

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var munro = CreateMunro(line);
                        if (munro != null)
                            results.Add(munro);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in reading CSV file at path: " + filePath);
                Console.WriteLine(e.Message);
                throw e.InnerException;
            }

            return results;
        }

        #region Helpers

        /// <summary>
        /// Create a <see cref="Munro"/> object parsing one line of the CSV file
        /// </summary>
        /// <param name="line">String containing the values of one record from the CSV file</param>
        /// <returns><see cref="Munro"/> object containing the corresponding values based on the mapping</returns>
        private Munro CreateMunro(string line)
        {
            try
            {
                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                var values = new List<string>(CSVParser.Split(line));

                var munro = new Munro(values, _columnsMap);
                return munro;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Create a map corelating the index for the list of values with the correct pair CSV column name-Munro property name
        /// </summary>
        /// <param name="headers">List of values containing the names of the columns in the CSV file</param>
        /// <returns>Dictionary rappresenting the mapping column index-column name-property name</returns>
        private Dictionary<int, ColumnToPropertyPair> CreateIndexColumnPropertyPairMap(string headers)
        {
            Dictionary<int, ColumnToPropertyPair> results = new Dictionary<int, ColumnToPropertyPair>();
            var columnNames = new List<string>(headers.Split(','));

            foreach (var pair in ColumnToPropertyMap)
            {
                var index = columnNames.IndexOf(pair.ColumnName);
                if (index != -1)
                {
                    results.Add(index, pair);
                }
                else
                {
                    throw new Exception("Column not found in the file");
                }
            }

            return results;
        }

        #endregion
    }
}
