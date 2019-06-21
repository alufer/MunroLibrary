using MunroLibrary.Interfaces;
using MunroLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MunroLibrary
{
    public class BaseMunroFilter : IMunroFilter
    {
        public List<Munro> Munros { get; set; }

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

        public virtual List<Munro> GetResults()
        {
            return Munros;
        }

        protected bool IsListMunrosEmpty()
        {
            return Munros?.Count > 0;
        }
    }
}
