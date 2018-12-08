using System;
using System.Collections.Generic;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day4Solver : SolverBase
    {
        public Day4Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 4;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();

            string input = GetInput();
            Dictionary<DateTime, string> guardMovements = input.Split('\n')
                                                               .Select(m => new {Date = DateTime.Parse(m.Substring(1, 16)), Movement = m.Substring(18, m.Length - 18)})
                                                               .OrderBy(m => m.Date)
                                                               .ToDictionary(m => m.Date, m => m.Movement);

            int currentGuard = 0;
            int sleepStart = 0;
            Dictionary<int, Dictionary<int, int>> sleepMap = new Dictionary<int, Dictionary<int, int>>();
            foreach (KeyValuePair<DateTime, string> guardMovement in guardMovements)
            {
                if (guardMovement.Value.Contains("falls asleep"))
                {
                    sleepStart = guardMovement.Key.Minute;
                }
                else if (guardMovement.Value.Contains("wakes up"))
                {
                    for (int i = sleepStart; i < guardMovement.Key.Minute; i++)
                    {
                        if (!sleepMap.ContainsKey(currentGuard))
                        {
                            sleepMap.Add(currentGuard, new Dictionary<int, int>());
                        }

                        if (!sleepMap[currentGuard].ContainsKey(i))
                        {
                            sleepMap[currentGuard].Add(i, 1);
                        }
                        else
                        {
                            sleepMap[currentGuard][i]++;
                        }
                    }
                }
                else if (guardMovement.Value.Contains("#"))
                {
                    int start = guardMovement.Value.IndexOf("#", StringComparison.Ordinal);
                    int end = guardMovement.Value.IndexOf(" ", start, StringComparison.Ordinal);
                    currentGuard = int.Parse(guardMovement.Value.Substring(start + 1, end - start - 1));
                }
            }


            switch (part)
            {
                case ProblemPart.Part1:
                    KeyValuePair<int, Dictionary<int, int>> guard = sleepMap.OrderByDescending(s => s.Value.Values.Sum()).FirstOrDefault();
                    int bestMinute = guard.Value.OrderByDescending(g => g.Value).Select(g => g.Key).FirstOrDefault();

                    AnswerSolution1 = guard.Key * bestMinute;

                    StopExecutionTimer();

                    return
                        FormatSolution($"The guard that sleeps the most is [{ConsoleColor.Yellow}!{guard.Key}], most often asleep at minute [{ConsoleColor.Yellow}!{bestMinute}] making the correct answer [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    KeyValuePair<int, Dictionary<int, int>> guard2 = sleepMap.OrderByDescending(s => s.Value.Values.Max()).FirstOrDefault();
                    int bestMinute2 = guard2.Value.OrderByDescending(g => g.Value).Select(g => g.Key).FirstOrDefault();

                    AnswerSolution2 = guard2.Key * bestMinute2;

                    StopExecutionTimer();

                    return
                        FormatSolution($"The guard most frequently asleep at the same minute is [{ConsoleColor.Yellow}!{guard2.Key}] most frequently asleep at minute [{ConsoleColor.Yellow}!{bestMinute2}] making the correct answer [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }
    }
}