﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class RepeatPattern<TInterval>(int times) : IPattern<Repeater, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<Repeater, TInterval>> GetEnumerator()
        {
            for (int i = 0; i < times; i++)
            {
                yield return new PatternNode<Repeater, TInterval>(new Repeater(i, times));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
