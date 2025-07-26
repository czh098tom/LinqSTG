using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel
{
    public interface IValueNodeOutput<out T>
    {
        public IObservable<T>? Value { get; }
        public T CurrentValue { get; }
    }
}
