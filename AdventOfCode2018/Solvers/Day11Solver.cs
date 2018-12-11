using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day11Solver : SolverBase
    {
        public Day11Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 11;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            int serialNumber = int.Parse(GetInput().Trim());
            Dictionary<Point, int> fuelCellGrid = new Dictionary<Point, int>();

            for (int x = 1; x <= 300; x++)
            for (int y = 1; y <= 300; y++)
            {
                Point fuelCell = new Point(x, y);
                int power = CalculateFuelCellPower(serialNumber, fuelCell);
                fuelCellGrid.Add(fuelCell, power);
            }

            switch (part)
            {
                case ProblemPart.Part1:
                    Dictionary<Point, int> fuelCellValues = new Dictionary<Point, int>();
                    foreach (KeyValuePair<Point, int> fuelCell in fuelCellGrid)
                    {
                        if (300 - fuelCell.Key.X < 3 || 300 - fuelCell.Key.Y < 3)
                        {
                            continue;
                        }

                        int totalValue = 0;
                        for (int x = 0; x < 3; x++)
                        for (int y = 0; y < 3; y++)
                        {
                            totalValue += fuelCellGrid[new Point(fuelCell.Key.X + x, fuelCell.Key.Y + y)];
                        }

                        if (totalValue > 0)
                        {
                            fuelCellValues.Add(fuelCell.Key, totalValue);
                        }
                    }

                    KeyValuePair<Point, int> bestCell = fuelCellValues.OrderByDescending(f => f.Value).First();

                    AnswerSolution1 = bestCell.Key;

                    StopExecutionTimer();

                    return
                        FormatSolution($"The X,Y coordinate for the top left of the best 3x3 fuel cell grid is [{ConsoleColor.Green}!{bestCell.Key.X},{bestCell.Key.Y}] (giving a total power of [{ConsoleColor.Yellow}!{bestCell.Value}])");
                case ProblemPart.Part2:

                    Dictionary<(Point Coordinate, int Size), int> fuelCellValueMap = new Dictionary<(Point, int), int>();
                    for (int i = 1; i <= 300; i++)
                    {
                        for (int X = 1; X < 300 - i; X++)
                        for (int Y = 1; Y < 300 - i; Y++)
                        {
                            int totalValue = 0;
                            for (int x = 0; x < i; x++)
                            for (int y = 0; y < i; y++)
                            {
                                totalValue += fuelCellGrid[new Point(X + x, Y + y)];
                            }

                            if (totalValue > 0)
                            {
                                fuelCellValueMap.Add((new Point(X, Y), i), totalValue);
                            }
                        }
                    }


                    KeyValuePair<(Point Coordinate, int Size), int> bestCell2 = fuelCellValueMap.OrderByDescending(f => f.Value).First();

                    AnswerSolution2 = bestCell2.Key;

                    StopExecutionTimer();

                    return
                        FormatSolution($"The X,Y coordinate for the top left of the best fuel cell grid is [{ConsoleColor.Green}!{bestCell2.Key.Coordinate.X},{bestCell2.Key.Coordinate.Y},{bestCell2.Key.Size}] (giving a total power of [{ConsoleColor.Yellow}!{bestCell2.Value}])");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        public int CalculateFuelCellPower(int serialNumber, Point fuelCell)
        {
            int rackId = fuelCell.X + 10;
            int initialPower = rackId * fuelCell.Y;
            int powerLevel = initialPower + serialNumber;
            powerLevel *= rackId;
            powerLevel = powerLevel < 100 ? 0 : powerLevel / 100 % 10;
            powerLevel -= 5;

            return powerLevel;
        }
    }
}