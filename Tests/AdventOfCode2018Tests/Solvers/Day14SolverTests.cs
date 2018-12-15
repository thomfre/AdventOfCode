using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day14SolverTests : SolverTestBase<Day14Solver>
    {
        protected override string TestData1 => "2018";
        protected override string TestData2 => "59414";
        protected override object CorrectAnswer1 => "5941429882";
        protected override object CorrectAnswer2 => 2018;
    }
}
