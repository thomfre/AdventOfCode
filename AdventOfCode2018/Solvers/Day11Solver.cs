using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day11Solver : SolverBase
    {
        private const int GridSize = 300;

        public Day11Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 11;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            int serialNumber = int.Parse(GetInput().Trim());
            int[,] fuelCellGrid = new int[GridSize, GridSize];

            for (int x = 0; x < GridSize; x++)
            for (int y = 0; y < GridSize; y++)
            {
                int power = CalculateFuelCellPower(serialNumber, x, y);
                fuelCellGrid[x, y] = power;
            }

            switch (part)
            {
                case ProblemPart.Part1:
                    Dictionary<Point, int> fuelCellValues = new Dictionary<Point, int>();
                    for (int fuelCellX = 0; fuelCellX < GridSize; fuelCellX++)
                    for (int fuelCellY = 0; fuelCellY < GridSize; fuelCellY++)
                    {
                        if (GridSize - fuelCellX < 3 || GridSize - fuelCellY < 3)
                        {
                            continue;
                        }

                        int totalValue = 0;
                        for (int x = 0; x < 3; x++)
                        for (int y = 0; y < 3; y++)
                        {
                            totalValue += fuelCellGrid[fuelCellX + x, fuelCellY + y];
                        }

                        if (totalValue > 0)
                        {
                            fuelCellValues.Add(new Point(fuelCellX, fuelCellY), totalValue);
                        }
                    }

                    KeyValuePair<Point, int> bestCell = fuelCellValues.OrderByDescending(f => f.Value).First();

                    AnswerSolution1 = bestCell.Key;

                    StopExecutionTimer();

                    return
                        FormatSolution($"The X,Y coordinate for the top left of the best 3x3 fuel cell grid is [{ConsoleColor.Green}!{bestCell.Key.X},{bestCell.Key.Y}] (giving a total power of [{ConsoleColor.Yellow}!{bestCell.Value}])");
                case ProblemPart.Part2:
                    int highestValue = 0;
                    PointSize bestValue = new PointSize(0, 0, 0);

                    for (int size = 1; size <= GridSize; size++)
                    {
                        Console.Write($"-{size}");
                        for (int x = 0; x < GridSize - size + 1; x++)
                        for (int y = 0; y < GridSize - size + 1; y++)
                        {
                            int sum = 0;
                            for (int xx = 0; xx < size; xx++)
                            for (int yy = 0; yy < size; yy++)
                            {
                                sum += fuelCellGrid[x + xx, y + xx];
                            }

                            if (sum <= highestValue)
                            {
                                continue;
                            }

                            highestValue = sum;
                            bestValue = new PointSize(x, y, size);
                        }
                    }


                    AnswerSolution2 = bestValue;

                    StopExecutionTimer();

                    return
                        FormatSolution($"The X,Y coordinate for the top left of the best fuel cell grid is [{ConsoleColor.Green}!{bestValue.X},{bestValue.Y},{bestValue.Size}] (giving a total power of [{ConsoleColor.Yellow}!{highestValue}])");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        public int CalculateFuelCellPower(int serialNumber, int x, int y)
        {
            int rackId = x + 10;
            int initialPower = rackId * y;
            int powerLevel = initialPower + serialNumber;
            powerLevel *= rackId;
            powerLevel = powerLevel < 100 ? 0 : powerLevel / 100 % 10;
            powerLevel -= 5;

            return powerLevel;
        }

        internal class PointSize
        {
            public PointSize(int x, int y, int size)
            {
                X = x;
                Y = y;
                Size = size;
            }

            public int X { get; }
            public int Y { get; }
            public int Size { get; }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 269;
                    hash = hash * 19 + X.GetHashCode();
                    hash = hash * 19 + Y.GetHashCode();
                    hash = hash * 19 + Size.GetHashCode();
                    return hash;
                }
            }

            public override bool Equals(object obj)
            {
                return obj is PointSize other && other.X == X && other.Y == Y && other.Size == Size;
            }
        }
    }
}