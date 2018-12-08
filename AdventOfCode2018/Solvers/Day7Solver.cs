using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day7Solver : SolverBase
    {
        public Day7Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 7;

        public int WorkersAvailable { get; set; } = 5;
        public int BaseTimeCost { get; set; } = 60;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string input = GetInput();
            string[] instructions = input.Split('\n');

            Dictionary<char, List<char>> dependencyDictionary = new Dictionary<char, List<char>>();

            foreach (string instruction in instructions)
            {
                MatchCollection matches = Regex.Matches(instruction, @"step (\w)", RegexOptions.IgnoreCase);
                char parent = matches[0].Groups[1].Value[0];
                char child = matches[1].Groups[1].Value[0];

                if (!dependencyDictionary.ContainsKey(child))
                {
                    dependencyDictionary.Add(child, new List<char>());
                }

                dependencyDictionary[child].Add(parent);
            }

            foreach (char grandParent in dependencyDictionary.Values.SelectMany(x => x)
                                                             .Where(x => !dependencyDictionary.ContainsKey(x))
                                                             .Distinct()
                                                             .OrderBy(x => x))
            {
                dependencyDictionary.Add(grandParent, new List<char>());
            }

            switch (part)
            {
                case ProblemPart.Part1:
                    List<char> solution = new List<char>();

                    while (dependencyDictionary.Count > 0)
                    {
                        (char child, List<char> _) = dependencyDictionary.OrderBy(x => x.Key).First(x => x.Value.Count == 0 || x.Value.All(p => solution.Contains(p)));
                        dependencyDictionary.Remove(child);
                        solution.Add(child);
                    }

                    AnswerSolution1 = string.Join("", solution);

                    StopExecutionTimer();

                    return FormatSolution($"The correct order is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    Dictionary<char, int> workers = new Dictionary<char, int>(WorkersAvailable);
                    List<char> done = new List<char>();
                    int totalWorkTime = 0;
                    int instructionsToComplete = dependencyDictionary.Count;

                    while (done.Count < instructionsToComplete)
                    {
                        int shoesToFill = WorkersAvailable - workers.Count;
                        for (int i = 0; i <= shoesToFill; i++)
                        {
                            (char child, List<char> _) = dependencyDictionary.OrderBy(x => x.Key)
                                                                             .FirstOrDefault(x => x.Value.Count == 0 || x.Value.All(p => done.Contains(p)));
                            if (child == default(char) || workers.Count >= WorkersAvailable)
                            {
                                break;
                            }

                            workers.Add(child, CalculateWorkTime(child));
                            dependencyDictionary.Remove(child);
                        }

                        foreach (char instruction in workers.Keys.ToList())
                        {
                            workers[instruction]--;
                            if (workers[instruction] > 0)
                            {
                                continue;
                            }

                            done.Add(instruction);
                            workers.Remove(instruction);
                        }

                        totalWorkTime++;
                    }

                    AnswerSolution2 = totalWorkTime;

                    StopExecutionTimer();

                    return FormatSolution($"It took the [{ConsoleColor.Yellow}!{WorkersAvailable}] workers [{ConsoleColor.Green}!{AnswerSolution2}] seconds to complete the task");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private int CalculateWorkTime(char instruction)
        {
            return BaseTimeCost + char.ToUpper(instruction) - 'A' + 1;
        }
    }
}
