using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MunroLibrary.Models
{
    /// <summary>
    /// Class defining a Munro
    /// </summary>
    public class Munro
    {
        #region Properties

        public enum CategoryType
        {
            NONE = 0,
            MUN,
            TOP
        }

        public string Name { get; set; }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("The height of the munro can't be negative or zero", nameof(Height));
                _height = value;
            }
        }

        public CategoryType HillCategory { get; set; }

        public string GridReference { get; set; }

        #endregion

        #region Constructors

        public Munro()
        {

        }

        /// <summary>
        /// Create a Munro object from the values of a line in the CSV using a Map to know which columns correspond to the properties of the Munro
        /// </summary>
        /// <param name="values">List of strings containing the values of one line of the CSV</param>
        /// <param name="columnsMap">Map corelating the index for the list of values with the correct pair CSV column name-Munro property name</param>
        public Munro(List<string> values, Dictionary<int, ColumnToPropertyPair> columnsMap)
        {
            if (values == null || values.Count == 0)
            {
                throw new ArgumentException("List of values empty ", nameof(values));
            }
            if (columnsMap == null || columnsMap.Count == 0)
            {
                throw new ArgumentException("Map empty ", nameof(columnsMap));
            }

            var categoryValue = GetValueForProperty(nameof(HillCategory), values, columnsMap);
            if (Enum.TryParse(categoryValue, out CategoryType categoryType))
            {
                if (categoryType != CategoryType.NONE)
                {
                    HillCategory = categoryType;
                }
                else
                {
                    throw new ArgumentException("Invalid value for the category of the munro", nameof(HillCategory));
                }
            }
            else
            {
                throw new ArgumentException("Invalid value for the category of the munro", nameof(HillCategory));
            }

            var heightString = GetValueForProperty(nameof(Height), values, columnsMap);
            if (double.TryParse(heightString, out double height))
            {                
                Height = height;
            }
            else
            {
                throw new ArgumentException("Invalid value for the height of the munro", nameof(Height));
            }

            Name = GetValueForProperty(nameof(Name), values, columnsMap);

            GridReference = GetValueForProperty(nameof(GridReference), values, columnsMap);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Get the value of a property name from a list of values if a mapping exist for it
        /// </summary>
        /// <param name="propertyName">Name of the property that we want to retrieve the value</param>
        /// <param name="values">List of values to go through</param>
        /// <param name="columnsMap">Mapping for retrieving the correct value</param>
        /// <returns>String containing the value for the property</returns>
        private string GetValueForProperty(string propertyName, List<string> values, Dictionary<int, ColumnToPropertyPair> columnsMap)
        {
            var item = columnsMap.Where(x => x.Value.PropertyName == propertyName);
            if (item != null && item.Count()!=0)
            {
                return values[item.First().Key];
            }
            return null;
        }

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj is Munro)
            {
                var other = obj as Munro;
                return this.Name == other.Name && this.HillCategory == other.HillCategory && this.Height == other.Height && this.GridReference == other.GridReference;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

    }
}
