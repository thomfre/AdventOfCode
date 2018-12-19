using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

// ReSharper disable StringLiteralTypo

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day19SolverTests : SolverTestBase<Day19Solver>
    {
        protected override string TestData1 => @"#ip 0
seti 5 0 1
seti 6 0 2
addi 0 1 0
addr 1 2 3
setr 1 0 0
seti 8 0 4
seti 9 0 5";

        protected override string TestData2 => TestData1;

        protected override object CorrectAnswer1 => 7;
        protected override object CorrectAnswer2 => null;
    }
}
