using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day5Solver : SolverBase
    {
        public Day5Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 5;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string input = GetInput();

            switch (part)
            {
                case ProblemPart.Part1:

                    int polymersRemoved = 1;
                    while (polymersRemoved > 0)
                    {
                        int oldLength = input.Length;
                        input = RemovePolymers(input);
                        polymersRemoved = oldLength - input.Length;
                    }

                    AnswerSolution1 = input.Length;

                    StopExecutionTimer();

                    return FormatSolution($"After fully removing all reactions, the resulting polymer contains [{ConsoleColor.Green}!{input.Length}] units");
                case ProblemPart.Part2:

                    char[] uniqueUnits = input.ToLower().ToCharArray().Distinct().ToArray();

                    Dictionary<char, int> unitDictionary = new Dictionary<char, int>();

                    foreach (char uniqueUnit in uniqueUnits)
                    {
                        string inputCopy = input;
                        inputCopy = inputCopy.Replace(uniqueUnit.ToString(), string.Empty, true, CultureInfo.InvariantCulture);

                        polymersRemoved = 1;
                        while (polymersRemoved > 0)
                        {
                            int oldLength = inputCopy.Length;
                            inputCopy = RemovePolymers(inputCopy);
                            polymersRemoved = oldLength - inputCopy.Length;
                        }

                        unitDictionary.Add(uniqueUnit, inputCopy.Length);
                    }

                    KeyValuePair<char, int> bestToRemove = unitDictionary.OrderBy(u => u.Value).First();

                    AnswerSolution2 = bestToRemove.Value;

                    StopExecutionTimer();

                    return
                        FormatSolution($"The best unit to remove is [{ConsoleColor.Yellow}!{char.ToLower(bestToRemove.Key)}/{char.ToUpper(bestToRemove.Key)}] giving the result [{ConsoleColor.Green}!{bestToRemove.Value}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private string RemovePolymers(string input)
        {
            int i = 0;
            while (i < input.Length - 1)
            {
                char current = Convert.ToChar(input.Substring(i, 1));
                char next = Convert.ToChar(input.Substring(i + 1, 1));
                if (char.ToLower(current) == char.ToLower(next))
                {
                    if (char.IsUpper(current) != char.IsUpper(next))
                    {
                        input = input.Remove(i, 2);
                    }
                }

                i++;
            }

            return input;
        }
    }
}
