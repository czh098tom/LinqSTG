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
        }
    }
}
