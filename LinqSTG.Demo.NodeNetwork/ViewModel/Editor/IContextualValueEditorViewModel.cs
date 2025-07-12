using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeNetwork.ViewModel.Editor
{
    public interface IContextualValueEditorViewModel<T>
    {
        public T RawValue { get; set; }
    }
}
