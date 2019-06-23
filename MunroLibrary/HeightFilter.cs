using MunroLibrary.Interfaces;
using MunroLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MunroLibrary
{
   /// <summary>
   /// Base for the creation of filters by height
   /// </summary>
    public abstract class HeightFilter : BaseMunroFilter
    {
        #region Properties

        private double _height;

        public double Height
        {
            get { return _height; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Height can't be negative");
                _height = value;
            }
        }

        #endregion

        #region Constructors

        public HeightFilter() : base()
        {
            Height = 0;
        }

        public HeightFilter(List<Munro> munros, double height = 0) : base(munros)
        {
            Height = height;
            
        }

        public HeightFilter(IMunroFilter filterMunrosSource, double height = 0) : base(filterMunrosSource)
        {            
            Height = height;            
        }

        #endregion                
    }
}
