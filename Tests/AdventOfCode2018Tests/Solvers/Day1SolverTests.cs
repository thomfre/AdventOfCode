using FluentAssertions;
using NUnit.Framework;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    internal class Day1SolverTests : SolverTestBase<Day1Solver>
    {
        protected override string TestData => "+1, -2, +3, +1";      

        [Test]
        public override void TestInput1()
        {
            Solver.Solve(ProblemPart.Part1);
            Solver.Answer1.Should().Be(3);
        }

        [Test]
        public override void TestInput2()
        {
            Solver.Solve(ProblemPart.Part2);
            Solver.Answer2.Should().Be(2);
        }
    }
}