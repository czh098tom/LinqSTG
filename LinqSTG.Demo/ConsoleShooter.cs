using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo
{
    public class ConsoleShooter<TData, TInterval> : IShooter<TData, TInterval>
        where TInterval : struct,
        IAdditionOperators<TInterval, TInterval, TInterval>,
        INumberBase<TInterval>
    {
        public void Shoot(IPattern<TData, TInterval> pattern)
        {
            var elapsed = TInterval.Zero;
            foreach (var item in pattern)
            {
                if (item.IsData)
                {
                    Console.WriteLine($"{elapsed}:\t Shoot a bullet with param ({item.Data})");
                }
                else
                {
                    Console.WriteLine($"{elapsed}:\t Delay ({item.Interval})");
                    elapsed += item.Interval;
                }
            }
        }
    }

    public static class ConsoleShooterExtension
    {
        public static void ShootByConsole<TData, TInterval>(this IPattern<TData, TInterval> pattern)
            where TInterval : struct,
            IAdditionOperators<TInterval, TInterval, TInterval>,
            INumberBase<TInterval>
        {
            new ConsoleShooter<TData, TInterval>().Shoot(pattern);
        }
    }
}
