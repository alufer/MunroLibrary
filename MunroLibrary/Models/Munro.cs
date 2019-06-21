using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MunroLibrary.Models
{
    public class Munro
    {
        public enum CategoryType
        {
            NONE = 0,
            MUN,
            TOP
        }

        public string Name { get; set; }
        public decimal Height { get; set; }
        public CategoryType HillCategory { get; set; }
        public string GridReference { get; set; }

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
            var categoryValue = values[columnsMap.Where(x => x.Value.PropertyName == nameof(HillCategory)).FirstOrDefault().Key];
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

            var heightString = values[columnsMap.Where(x => x.Value.PropertyName == nameof(Height)).FirstOrDefault().Key];
            if (decimal.TryParse(heightString, out decimal height))
            {
                if (height <= 0)
                {
                    throw new ArgumentException("The height of the munro can't be negative or zero", nameof(Height));
                }
                Height = height;
            }
            else
            {
                throw new ArgumentException("Invalid value for the height of the munro", nameof(Height));
            }

            Name = values[columnsMap.Where(x => x.Value.PropertyName == nameof(Name)).FirstOrDefault().Key];

            GridReference = values[columnsMap.Where(x => x.Value.PropertyName == nameof(GridReference)).FirstOrDefault().Key];
        }
    }
}
