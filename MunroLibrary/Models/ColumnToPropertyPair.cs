using System;
using System.Collections.Generic;
using System.Text;

namespace MunroLibrary.Models
{
    /// <summary>
    /// Class that pairs the column name in the header of CSV file with the name of a property in an object
    /// </summary>
    public class ColumnToPropertyPair
    {
        #region Properties

        private string _columnName;

        public string ColumnName
        {
            get { return _columnName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("The name of the column can't be empty", nameof(ColumnName));
                _columnName = value;
            }
        }

        private string _propertyName;

        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("The name of the property can't be empty", nameof(PropertyName));
                _propertyName = value;
            }
        }

        #endregion

        #region Constructors

        public ColumnToPropertyPair(string columnName, string propertyName)
        {
            ColumnName = columnName;
            PropertyName = propertyName;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is ColumnToPropertyPair)
            {
                var other = obj as ColumnToPropertyPair;
                return this.ColumnName == other.ColumnName && this.PropertyName == other.PropertyName;
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
