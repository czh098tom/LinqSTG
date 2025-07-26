using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes
{
    public record RepeaterKey(string ID, string Total)
    {
        public static RepeaterKey Default { get; } = new("ID", "Total");

        public Repeater GetRepeater(Parameter dict)
        {
            var id = dict.GetValueOrDefault(ID, 0f);
            var total = dict.GetValueOrDefault(Total, 0f);
            return new Repeater(Convert.ToInt32(id), Convert.ToInt32(total));
        }
    }
}
