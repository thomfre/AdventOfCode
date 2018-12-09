using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day9Solver : SolverBase
    {
        public Day9Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 9;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string input = GetInput();
            MatchCollection matches = Regex.Matches(input, @"(^|\s)(\d+)\w", RegexOptions.IgnoreCase);
            int numberOfPlayers = int.Parse(matches[0].Value);
            int maxWorth = int.Parse(matches[1].Value);

            Dictionary<int, List<long>> players = Enumerable.Range(0, numberOfPlayers).ToDictionary(x => x, x => new List<long>());

            int[] marbles;
            switch (part)
            {
                case ProblemPart.Part1:
                    marbles = Enumerable.Range(0, maxWorth).ToArray();
                    PlayGame(marbles, ref players);

                    AnswerSolution1 = players.Select(p => p.Value.Sum()).OrderByDescending(x => x).First();

                    StopExecutionTimer();

                    return FormatSolution($"The answer is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    marbles = Enumerable.Range(0, maxWorth * 100).ToArray();
                    PlayGame(marbles, ref players);

                    AnswerSolution2 = players.Select(p => p.Value.Sum()).OrderByDescending(x => x).First();

                    StopExecutionTimer();

                    return FormatSolution($"The answer is [{ConsoleColor.Green}!{AnswerSolution2}]");

                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private void PlayGame(int[] marbles, ref Dictionary<int, List<long>> players)
        {
            int currentPlayer = 0;
            int numberOfPlayers = players.Count;

            LinkedList<int> gameBoard = new LinkedList<int>();
            LinkedListNode<int> currentMarble = null;

            foreach (int marble in marbles)
            {
                currentPlayer++;
                if (currentPlayer >= numberOfPlayers)
                {
                    currentPlayer = 0;
                }

                if (gameBoard.Count < 2)
                {
                    currentMarble = gameBoard.AddLast(marble);
                }
                else
                {
                    if (marble % 23 == 0)
                    {
                        players[currentPlayer].Add(marble);
                        LinkedListNode<int> x = currentMarble;
                        for (int j = 0; j < 7; j++)
                        {
                            x = x.Previous ?? gameBoard.Last;
                        }

                        players[currentPlayer].Add(x.Value);
                        currentMarble = x.Next ?? gameBoard.First;
                        gameBoard.Remove(x);
                    }
                    else
                    {
                        currentMarble = gameBoard.AddAfter(currentMarble.Next ?? gameBoard.First, marble);
                    }
                }
            }
        }
    }
}