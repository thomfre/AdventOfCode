using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day18Solver : SolverBase
    {
        private string _currentForest;

        private string _originalForest;

        public Day18Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 18;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            _originalForest = GetInput();
            _currentForest = _originalForest;

            switch (part)
            {
                case ProblemPart.Part1:
                    GrowForest(10);

                    AnswerSolution1 = _currentForest.ToCharArray().Count(a => a == '#') * _currentForest.ToCharArray().Count(a => a == '|');
                    StopExecutionTimer();

                    return FormatSolution($"The resource score after 10 minutes is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    GrowForest(1000000000);

                    AnswerSolution2 = _currentForest.ToCharArray().Count(a => a == '#') * _currentForest.ToCharArray().Count(a => a == '|');

                    StopExecutionTimer();

                    return FormatSolution($"The resource score after 1000000000 minutes is [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private void GrowForest(int minutes)
        {
            List<string> forests = new List<string> {_currentForest};
            for (int i = 0; i < minutes; i++)
            {
                StringBuilder newForest = new StringBuilder();

                string[] scanResult = _currentForest.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).ToArray();

                for (int y = 0; y < scanResult.Length; y++)
                {
                    for (int x = 0; x < scanResult[y].Length; x++)
                    {
                        List<char> adjacent = GetAdjacent(scanResult, x, y);
                        switch (scanResult[y][x])
                        {
                            case '.':
                                newForest.Append(adjacent.Count(a => a == '|') >= 3 ? '|' : '.');
                                break;
                            case '|':
                                newForest.Append(adjacent.Count(a => a == '#') >= 3 ? '#' : '|');
                                break;
                            case '#':
                                newForest.Append(adjacent.Any(a => a == '#') && adjacent.Any(a => a == '|') ? '#' : '.');
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    newForest.AppendLine();
                }

                _currentForest = newForest.ToString().Trim();
                forests.Add(_currentForest);

                if (i <= 1000)
                {
                    continue;
                }

                string firstRepeatedForest = forests.GroupBy(f => f)
                                                    .Where(group => group.Count() > 1)
                                                    .Select(group => group.Key)
                                                    .First();

                int firstIndex = forests.IndexOf(firstRepeatedForest);
                int nextIndex = forests.IndexOf(firstRepeatedForest, firstIndex + 1);

                int interval = nextIndex - firstIndex;

                int index = firstIndex + (minutes - firstIndex) % interval;

                _currentForest = forests[index];
                return;
            }
        }

        private List<char> GetAdjacent(IReadOnlyList<string> forest, int x, int y)
        {
            HashSet<(int X, int Y)> adjacentCheck = new HashSet<(int X, int Y)>
                                                    {
                                                        (x - 1, y - 1),
                                                        (x - 1, y),
                                                        (x - 1, y + 1),
                                                        (x, y - 1),
                                                        (x, y + 1),
                                                        (x + 1, y - 1),
                                                        (x + 1, y),
                                                        (x + 1, y + 1)
                                                    };

            List<char> adjacent = new List<char>();

            adjacentCheck.ForEach(a =>
                                  {
                                      if (forest.Count > a.Y && a.Y >= 0 && forest[a.Y].Length > a.X && a.X >= 0)
                                      {
                                          adjacent.Add(forest[a.Y][a.X]);
                                      }
                                  });

            return adjacent;
        }
    }
}
