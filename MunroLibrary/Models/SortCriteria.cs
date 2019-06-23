using System;
using System.Collections.Generic;
using System.Text;

namespace MunroLibrary.Models
{
    /// <summary>
    /// The criteria for sorting the list of <see cref="Munro"/>
    /// </summary>
    public class SortCriteria
    {
        public enum PropertyCriteriaType
        {            
            ALPHABETICALLY,
            HEIGHT
        }

        public enum OrderType
        {
            ASCENDING,
            DESCENDING
        }

        #region Properties

        public PropertyCriteriaType PropertyCriteria { get; set; }

        public OrderType Order { get; set; }

        #endregion

        #region Constructors

        public SortCriteria(PropertyCriteriaType propertyCriteria = PropertyCriteriaType.ALPHABETICALLY, OrderType order = OrderType.ASCENDING)
        {
            PropertyCriteria = propertyCriteria;
            Order = order;
        }

        #endregion
    }
}
