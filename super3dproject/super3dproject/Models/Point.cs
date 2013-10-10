﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace super3dproject.Models
{
    public class Point
    {
        public Point GetNegative()
        {
            return new Point()
            {
                X = -X,
                Y = -Y,
                Z = -Z
            };
            
        }
        public double X
        {
            get;
            set;
        }
        public double Y
        {
            get;
            set;
        }
        public double Z
        {
            get;
            set;
        }
        public Point()
        {
 
        }
    }
}
