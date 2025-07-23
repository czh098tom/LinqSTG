using LinqSTG.Demo.NodeGraph.ViewModel.Nodes;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel
{
    public class LinqSTGConnectionViewModel : ConnectionViewModel
    {
        public Color ColorInput { get; } = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
        public Color ColorOutput { get; } = Color.FromArgb(0xff, 0xff, 0xff, 0xff);

        public LinqSTGConnectionViewModel(NetworkViewModel parent, NodeInputViewModel input, NodeOutputViewModel output) : base(parent, input, output)
        {
            if (input.Port is LinqSTGPortViewModel inputPort && output.Port is LinqSTGPortViewModel outputPort)
            {
                ColorInput = inputPort.PortColor;
                ColorOutput = outputPort.PortColor;
            }
        }
    }
}
