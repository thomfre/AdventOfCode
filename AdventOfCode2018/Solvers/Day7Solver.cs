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

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string input = GetInput();
            string[] instructions = input.Split('\n');

            switch (part)
            {
                case ProblemPart.Part1:
                    Dictionary<string, List<string>> dependencyDictionary = new Dictionary<string, List<string>>();

                    foreach (string instruction in instructions)
                    {
                        MatchCollection matches = Regex.Matches(instruction, @"step (\w)", RegexOptions.IgnoreCase);
                        string parent = matches[0].Groups[1].Value;
                        string child = matches[1].Groups[1].Value;

                        if (!dependencyDictionary.ContainsKey(child))
                        {
                            dependencyDictionary.Add(child, new List<string>());
                        }

                        dependencyDictionary[child].Add(parent);
                    }

                    List<string> grandParents = dependencyDictionary.Values.SelectMany(x => x)
                                                             .Where(x => !dependencyDictionary.ContainsKey(x))
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList();

                    List<string> solution = new List<string>();
                    solution.Add(ShiftList(grandParents));

                    while (dependencyDictionary.Count > 0)
                    {
                        (string child, List<string> parents) = dependencyDictionary.Where(x => x.Value.All(p => solution.Contains(p))).OrderBy(x => x.Key).FirstOrDefault();
                        if (child == null)
                        {
                            string grandParent = ShiftList(grandParents);
                            solution.Add(grandParent);
                            continue;
                        }

                        dependencyDictionary.Remove(child);
                        solution.Add(child);
                    }

                    AnswerSolution1 = string.Join("", solution);
                    //Give BTEUFNVADWGPLRJOHMXKZQCISY, which is wrong

                    StopExecutionTimer();

                    return FormatSolution($"The correct order is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    AnswerSolution2 = null;

                    StopExecutionTimer();

                    return FormatSolution($"The answer is [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }

            string ShiftList(List<string> list)
            {
                if (list.Count == 0)
                {
                    return null;
                }

                string firstItem = list[0];
                list.RemoveAt(0);
                return firstItem;
            }
        }
    }
}