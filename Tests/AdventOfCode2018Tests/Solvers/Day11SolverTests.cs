using System.Drawing;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day11SolverTests : SolverTestBase<Day11Solver>
    {
        protected override string TestData1 => "18";
        protected override string TestData2 => TestData1;
        protected override object CorrectAnswer1 => new Point(33, 45);
        protected override object CorrectAnswer2 => new Day11Solver.PointSize(90, 269, 16);

        [TestCase(57, 122, 79, -5)]
        [TestCase(39, 217, 196, 0)]
        [TestCase(71, 101, 153, 4)]
        [TestCase(8, 3, 5, 4)]
        public void CalculateFuelCellPower_calculates_correct_power(int serialNumber, int fuelCellX, int fuelCellY, int correctAnswer)
        {
            int power = Solver.CalculateFuelCellPower(serialNumber, fuelCellX, fuelCellY);
            power.Should().Be(correctAnswer);
        }
    }
}