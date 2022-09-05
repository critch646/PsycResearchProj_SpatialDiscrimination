using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialDiscriminationApp
{
    public struct Range
    {
        public int Min, Max;

        public Range(int min, int max)
        {
            this.Min = min; 
            this.Max = max; 
        }
    }
}
