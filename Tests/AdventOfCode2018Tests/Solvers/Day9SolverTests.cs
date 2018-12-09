using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;
using Thomfre.AdventOfCode2018.Solvers;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day9SolverTests : SolverTestBase<Day9Solver>
    {
        protected override string TestData1 => "10 players; last marble is worth 1618 points";
        protected override string TestData2 => "476 players; last marble is worth 71431 points";
        protected override object CorrectAnswer1 => 8317;
        protected override object CorrectAnswer2 => 3066307353;


        [TestCase("10 players; last marble is worth 1618 points", 8317)]
        [TestCase("13 players; last marble is worth 7999 points", 146373)]
        [TestCase("21 players; last marble is worth 6111 points", 54718)]
        [TestCase("30 players; last marble is worth 5807 points", 37305)]
        public void Solution_for_first_part_is_calculated_correctly(string input, int correctAnswer)
        {
            A.CallTo(() => AutoFake.Resolve<IInputLoader>().LoadInput(A<int>._)).Returns(input);
            Solver.Solve(ProblemPart.Part1);
            Solver.Answer1.Should().Be(correctAnswer);
        }
    }
}