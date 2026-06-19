# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

LinqSTG is a .NET library for composing **danmaku (bullet-pattern) shooting sequences** using LINQ-style chainable operators. A pattern is a time-ordered stream of two kinds of events: *data events* (something to shoot) and *interval events* (a time delay before the next event). Operators like `Select`, `SelectMany`, `Where`, `Skip`, `Take`, `Concat`, `Reverse`, and `Trim*` transform these streams; an `IShooter` consumes them.

## Build / test / run

```bash
dotnet build LinqSTG.sln                                       # build everything
dotnet test LinqSTG.Test/LinqSTG.Test.csproj                   # run NUnit tests
dotnet test LinqSTG.Test/LinqSTG.Test.csproj --filter Test123  # run a single test
dotnet run --project LinqSTG.Demo                              # console demo
dotnet run --project LinqSTG.Demo.WPF                          # WPF scripting demo
dotnet run --project LinqSTG.Demo.NodeGraph                    # WPF node editor
```

The two WPF demos target `net8.0-windows` and require Windows + the Windows Desktop workload.

## Solution layout

| Project | TFM | Role |
|---|---|---|
| `LinqSTG` | `net8.0;netstandard2.1` | Core library — the `IPattern<TData,TInterval>` model and all operators. |
| `LinqSTG.Kinematics` | `net8.0` | `IParametric<TTime,TData>` interface + `Parametric` factory and `ParametricExtension` combinators (`UniformVelocity`, `UniformAcceleration`, `Offset`, `AfterTime`) for computing trajectories. |
| `LinqSTG.Test` | `net8.0` | NUnit tests for core patterns. |
| `LinqSTG.Demo` | `net8.0` | Console demo (`Program.cs` is top-level statements). |
| `LinqSTG.Demo.WPF` | `net8.0-windows` | Visual demo; the user types C# into a text box and `Microsoft.CodeAnalysis.CSharp.Scripting` evaluates it at runtime. |
| `LinqSTG.Demo.NodeGraph` | `net8.0-windows` | Visual node editor built on `NodeNetwork`; users wire nodes together to build patterns. |
| `LinqSTG.Velocity` | — | Placeholder (no sources yet). |

## Core architecture (read these together)

The whole library is small. To understand it, read in this order:

1. **`LinqSTG/IPattern.cs` + `LinqSTG/PatternNode.cs`** — A pattern is `IEnumerable<PatternNode<TData?, TInterval>>`. `PatternNode` is a discriminated struct: `IsData` switches between a `Data` payload and an `Interval` delay. `TInterval` is constrained to `struct`.

2. **`LinqSTG/Pattern.cs`** — Factory: `Repeat`, `RepeatWithInterval`, `Infinite`, `InfiniteWithInterval`, `Single`, `SingleInterval`, `FromEnumerable`, `Empty`. Concrete pattern types live in `LinqSTG/Extensions/` (e.g. `RepeatPattern.cs`, `FromEnumerablePattern.cs`).

3. **`LinqSTG/PatternExtension.cs`** — All the LINQ-like operators as extension methods. Each returns a new `IPattern` that lazily wraps its source (e.g. `SelectPattern`, `WherePattern`, `ConcatPattern`, `SkipPattern`).

4. **`LinqSTG/Repeater.cs` + `LinqSTG/IntervalType.cs`** — `Repeater(ID, Total)` is the default data type emitted by `Repeat*`/`Infinite*`. `Sample01(IntervalType)` maps the ID/Total ratio into `[0,1]` with open/closed endpoints, then `MinMax(min,max)` (in `LinqSTG/Easings/InterpolationExtension.cs`) rescales it. This is the idiomatic way to fan a repeater out into angles, speeds, etc.

5. **`LinqSTG/Extensions/SelectManyPattern.cs`** — The most intricate piece. `SelectMany` doesn't just concatenate sub-patterns; it **merges their interval timelines into the parent's timeline** (think of each sub-pattern as a parallel track whose delays interleave with the parent's). The algorithm maintains a list of live sub-enumerators with their accumulated elapsed time and always advances the one with the smallest next-event time. `SelectManyConcat` (`SelectManyConcatPattern.cs`) is the simpler variant that just concatenates.

6. **`LinqSTG/IShooter.cs`** — Consumers. `IShooter<TData,TInterval>` is side-effecting; `IShooter<TData,TInterval,TResult>` returns a value (e.g. the WPF demos' `PointShooter` returns `IEnumerable<PointPrediction>`). See `LinqSTG.Demo/ConsoleShooter.cs` for the canonical "iterate the pattern, branch on `IsData`" loop.

### Numeric type handling (important)

`TInterval` operators that need arithmetic (`SelectMany`, `ConsoleShooter`, `MaterializeEnumerable`) come in **two flavors selected by `#if NET7_0_OR_GREATER`**:

- On `net8.0`: a single generic implementation constrained to `INumberBase<TInterval>` / `IComparisonOperators<,>` / `IMinMaxValue<>`.
- On `netstandard2.1`: duplicated concrete implementations per numeric type — `SelectManyPatternInt/Long/Float/Double`, `MaterializeEnumerable.Int/Long/Float/Double`. When adding a new interval-consuming operator, you must duplicate it the same way or it won't be available on the `netstandard2.1` target.

`LinqSTG/Extensions/MaterializeEnumerable.cs` + `LinqSTG/ShootEvent.cs` provide an opt-in helper that flattens a pattern into `IEnumerable<ShootEvent<TData,TTime>>` (data + absolute start time), which is handy for non-streaming consumers.

## Demo: NodeGraph editor

The NodeGraph demo is its own subsystem and has a distinct mental model:

- **Everything is `Contextual<T>`** (`ViewModel/Contextual.cs`): a delegate `T Parameter -> T`. A node output doesn't produce a value; it produces a *function* that produces a value given the parameters visible at that point in the pattern. This is how inner-loop nodes (e.g. inside a `RepeatWithInterval`) can read the outer repeater's `ID`/`Total`. `Parameter` (`ViewModel/Parameter.cs`) is a `Dictionary<string,float>` threaded through as the context.
- **Nodes are registered in two places that must stay in sync**: `MainViewModel.cs` (`NodeList.AddNodeType(...)`) and `App.xaml.cs` (Splat view registration `Register(() => new LinqSTGNodeView(), typeof(IViewFor<...>))`). Adding a node means touching both.
- **All nodes derive from `LinqSTGNodeViewModel`** (`ViewModel/Nodes/LinqSTGNodeViewModel.cs`) and use `AddInput`/`AddOutput`/`AddEditor` with string keys — those keys are what the serializer (`Serialization/NetworkModel.cs`) uses to reconnect ports on load, so renaming a key breaks saved graphs.
- **Input/output factories** live in the static `LinqSTGNodeInputViewModel` / `LinqSTGNodeOutputViewModel` classes and determine the port color (`PortColor.cs`) and type. `ContextAwareNodeInputViewModel` is for ports that accept more than one numeric type.
- **Persistence**: `MainViewModel.Save/Load` round-trips the network through `Newtonsoft.Json` into `Properties.Settings.Default.Temp`. The graph is rebuilt by re-instantiating nodes by their parameterless constructor and re-connecting ports by name key.

## Demo: WPF scripting

`LinqSTG.Demo.WPF/MainViewModel.cs` uses `Microsoft.CodeAnalysis.CSharp.Scripting` to `CSharpScript.EvaluateAsync` user-typed code. The script options pre-import `LinqSTG`, `LinqSTG.Pattern`, `LinqSTG.Easings`, `LinqSTG.Kinematics`, etc. — so example scripts in the text box can call `Repeat<int>(...)` unqualified. `DemoScript.cs` is a catalog of ready-made patterns (spiral, wave, polyhedron, multi-speed) that double as executable examples.

## Conventions

- XML doc comments on public operators are the norm in `LinqSTG/` — keep adding them for new public APIs.
- The codebase mixes `Microsoft.CodeAnalysis.CSharp.Scripting` and `Newtonsoft.Json` only inside the WPF demos; the core library has no third-party dependencies.
- Tests are written in NUnit (`[Test]`, `Assert.Multiple`); the existing `PatternTest.cs` only covers the `Repeat*` factories — most operators are exercised through the demo projects rather than unit tests.
