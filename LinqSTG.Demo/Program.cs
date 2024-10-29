using LinqSTG.Easings;
using static LinqSTG.Pattern;

namespace LinqSTG.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RepeatWithInterval(times: 3, interval: 5).TrimEnd().ShootByConsole();
            Console.WriteLine();
            RepeatWithInterval(times: 3, interval: 5).TrimEnd()
                .SelectMany(r => RepeatWithInterval(times: 3, interval: 3).TrimEnd())
                .ShootByConsole();
            Console.WriteLine();

            var p = from r1 in Repeat<int>(times: 6)
                    from r2 in RepeatWithInterval(times: 3, interval: 1)
                    select r1.Sample01(IntervalType.HeadClosed).MinMax(0f, 360f) 
                        + r2.Sample01(IntervalType.BothClosed).MinMax(-5f, 5f);
            p.ShootByConsole();
        }
    }
}
