using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day1SolverTests : SolverTestBase<Day1Solver>
    {
        protected override string TestData1 => @"+1
-2
+3
+1";

        protected override string TestData2 => TestData1;

        protected override object CorrectAnswer1 => 3;
        protected override object CorrectAnswer2 => 2;
    }
}
