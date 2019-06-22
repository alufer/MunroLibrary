using MunroLibrary.Interfaces;
using MunroLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MunroLibrary
{
    public abstract class BaseMunroFilter : IMunroFilter
    {
        #region Properties
        public List<Munro> Munros { get; set; }

        #endregion

        #region Constructors

        public BaseMunroFilter()
        {
            Munros = new List<Munro>();
        }

        public BaseMunroFilter(List<Munro> munros)
        {
            Munros = munros;
        }

        public BaseMunroFilter(IMunroFilter filter)
        {
            Munros = filter.GetResults();
        }

        #endregion

        public virtual List<Munro> GetResults()
        {
            return Munros;
        }

        #region Helpers

        protected bool IsListMunrosEmpty()
        {
            return Munros?.Count == 0;
        }

        #endregion
    }
}
