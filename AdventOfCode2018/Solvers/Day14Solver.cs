using System;
using System.Collections.Generic;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day14Solver : SolverBase
    {
        public Day14Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 14;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            List<int> recipes = new List<int>();
            int[] elves = new int[2];

            recipes.Add(3);
            recipes.Add(7);

            elves[0] = 0;
            elves[1] = 1;

            switch (part)
            {
                case ProblemPart.Part1:
                    int numberOfRecipes = int.Parse(GetInput().Trim());
                    while (recipes.Count < numberOfRecipes + 10)
                    {
                        int currentElf1 = recipes[elves[0]];
                        int currentElf2 = recipes[elves[1]];

                        int newRecipeBase = currentElf1 + currentElf2;
                        int[] newRecipes = newRecipeBase.ToString().ToCharArray().Select(r => int.Parse(r.ToString())).ToArray();
                        recipes.AddRange(newRecipes);

                        int stepsToMoveElf1 = currentElf1 + 1;
                        int stepsToMoveElf2 = currentElf2 + 1;

                        elves[0] = (elves[0] + stepsToMoveElf1) % recipes.Count;
                        elves[1] = (elves[1] + stepsToMoveElf2) % recipes.Count;
                    }

                    string scoreOfNextTen = string.Join("", recipes.TakeLast(10));

                    AnswerSolution1 = scoreOfNextTen;

                    StopExecutionTimer();

                    return FormatSolution($"The scores of the then recipes immediately after our number are [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    int[] recipeScoresToFind = GetInput().Trim().ToCharArray().Select(r => int.Parse(r.ToString())).ToArray();
                    int lastPositionFound = 0;
                    int recipeRowFoundAtPosition = -1;
                    int indexToLookAt = 0;

                    while (recipeRowFoundAtPosition < 0)
                    {
                        int currentElf1 = recipes[elves[0]];
                        int currentElf2 = recipes[elves[1]];

                        int newRecipeBase = currentElf1 + currentElf2;
                        int[] newRecipes = newRecipeBase.ToString().ToCharArray().Select(r => int.Parse(r.ToString())).ToArray();
                        recipes.AddRange(newRecipes);

                        int stepsToMoveElf1 = currentElf1 + 1;
                        int stepsToMoveElf2 = currentElf2 + 1;

                        elves[0] = (elves[0] + stepsToMoveElf1) % recipes.Count;
                        elves[1] = (elves[1] + stepsToMoveElf2) % recipes.Count;

                        while (indexToLookAt + lastPositionFound < recipes.Count)
                        {
                            if (recipeScoresToFind[lastPositionFound] == recipes[indexToLookAt + lastPositionFound])
                            {
                                if (lastPositionFound == recipeScoresToFind.Length - 1)
                                {
                                    recipeRowFoundAtPosition = indexToLookAt;
                                    break;
                                }

                                lastPositionFound++;
                            }
                            else
                            {
                                lastPositionFound = 0;
                                indexToLookAt++;
                            }
                        }
                    }

                    AnswerSolution2 = recipeRowFoundAtPosition;

                    StopExecutionTimer();

                    return FormatSolution($"There is a total of [{ConsoleColor.Green}!{AnswerSolution2}] recipes on the scoreboard before the sequence");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }
    }
}
