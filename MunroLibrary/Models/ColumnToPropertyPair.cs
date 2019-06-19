using System;
using System.Collections.Generic;
using System.Text;

namespace MunroLibrary.Models
{
    public class ColumnToPropertyPair
    {
        public string ColumnName { get; private set; }
        public string PropertyName { get; private set; }

        public ColumnToPropertyPair(string columnName, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException(nameof(columnName) + " can't be empty", nameof(columnName));
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException(nameof(propertyName) + " can't be empty", nameof(propertyName));

            ColumnName = columnName;
            PropertyName = propertyName;
        }
    }
}
