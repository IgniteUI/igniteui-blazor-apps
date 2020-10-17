using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infragistics.Samples
{ 
    public class StateLocation
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string ID { get; set; }

        public StateLocation(string id, double x, double y)
        {
            ID = id;
            X = x;
            Y = y;
        }
    }
}
