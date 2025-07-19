using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel
{
    public class Parameter : Dictionary<string, float>
    {
        public static readonly Parameter Empty = [];

        public Parameter() : base()
        {
        }

        public Parameter(IDictionary<string, float> dictionary) : base(dictionary)
        {
        }
    }
}
