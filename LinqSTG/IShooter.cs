using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG
{
    public interface IShooter<TData, TInterval>
        where TInterval : struct
    {
        public void Shoot(IPattern<TData, TInterval> pattern);
    }
}
