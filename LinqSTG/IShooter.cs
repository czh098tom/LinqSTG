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

    public interface IShooter<TData, TInterval, TResult>
        where TInterval : struct
    {
        public TResult Shoot(IPattern<TData, TInterval> pattern);
    }
}
