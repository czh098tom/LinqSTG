# LinqSTG
LinqSTG is a LINQ based danmaku pattern creating/analyzing framework.
Inspired by functional/reactive programming, 
LinqSTG provides a easy and clear way to represent, and analyze patterns,
making it one of the most intuitive library available.

## Example
Creating patterns using chaining calls:
```C#
RepeatWithInterval(times: 3, interval: 5).TrimEnd()
    .SelectMany(r => RepeatWithInterval(times: 3, interval: 3).TrimEnd());
```
or using query syntax:
```C#
from r1 in Repeat<int>(times: 6)
from r2 in RepeatWithInterval(times: 3, interval: 1)
select r1.Sample01(IntervalType.HeadClosed).MinMax(0f, 360f)
    + r2.Sample01(IntervalType.BothClosed).MinMax(-5f, 5f);
```