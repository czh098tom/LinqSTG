using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Easings
{
    public static class InterpolationExtension
    {
        /// <summary>
        /// Remap a value between 0 and 1 by a mapper.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="mapper">The mapping function.</param>
        /// <returns>The value mapped.</returns>
        public static T RemapBy<T>(this T value, Func<T, T> mapper)
            where T : struct
        {
            return mapper(value);
        }

        /// <summary>
        /// Scale the value between 0 and 1 into value between min and max.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The value mapped.</returns>
        public static T MinMax<T>(this T value, T min, T max)
            where T : struct, 
            IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, 
            IMultiplyOperators<T, T, T>, IMultiplicativeIdentity<T, T>
        {
            return max * value + min * (T.MultiplicativeIdentity - value);
        }
    }
}
