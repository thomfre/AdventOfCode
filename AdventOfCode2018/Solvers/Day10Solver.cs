using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day10Solver : SolverBase
    {
        public Day10Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 10;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();

            PlayGame();

            StopExecutionTimer();

            switch (part)
            {
                case ProblemPart.Part1:
                    return FormatSolution($"The answer is:\n[{ConsoleColor.Green}!{AnswerSolution1}]\n");
                case ProblemPart.Part2:
                    return FormatSolution($"The elves would have to wait [{ConsoleColor.Green}!{AnswerSolution2}] seconds");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private void PlayGame()
        {
            List<string> input = GetInput().Split('\n').ToList();

            HashSet<Point> points = new HashSet<Point>();
            foreach (string point in input)
            {
                points.Add(new Point(point));
            }

            bool running = true;
            int previousDistance = GetMaxDistance(points);
            int gameTime = 0;
            while (running)
            {
                int maxDistance = GetMaxDistance(points);
                MovePoints(points);
                gameTime++;

                if (previousDistance < maxDistance)
                {
                    running = false;
                    MovePointsBack(points);
                    MovePointsBack(points);
                    gameTime -= 2;
                }

                previousDistance = maxDistance;
            }

            AnswerSolution1 = FormatPoints(points);
            AnswerSolution2 = gameTime;
        }

        private string FormatPoints(HashSet<Point> points)
        {
            int lowestX = points.Min(x => x.X);
            int lowestY = points.Min(y => y.Y);
            int highestX = points.Max(x => x.X);
            int highestY = points.Max(y => y.Y);

            StringBuilder outputBuilder = new StringBuilder();

            for (int y = lowestY; y <= highestY; y++)
            {
                for (int x = lowestX; x <= highestX; x++)
                {
                    outputBuilder.Append(points.Any(p => p.X == x && p.Y == y) ? "#" : ".");
                }

                if (y < highestY)
                {
                    outputBuilder.AppendLine();
                }
            }

            return outputBuilder.ToString();
        }

        private void MovePoints(HashSet<Point> points)
        {
            foreach (Point point in points)
            {
                point.Move();
            }
        }

        private void MovePointsBack(HashSet<Point> points)
        {
            foreach (Point point in points)
            {
                point.MoveBack();
            }
        }

        private int GetMaxDistance(HashSet<Point> points)
        {
            int lowestX = points.Min(x => x.X);
            int lowestY = points.Min(y => y.Y);
            int highestX = points.Max(x => x.X);
            int highestY = points.Max(y => y.Y);

            return Math.Abs(lowestX - highestX) + Math.Abs(lowestY - highestY);
        }

        internal class Point
        {
            public Point(string input)
            {
                MatchCollection matches = Regex.Matches(input, @"-?\d+", RegexOptions.IgnoreCase);
                X = int.Parse(matches[0].Value);
                Y = int.Parse(matches[1].Value);
                VelocityX = int.Parse(matches[2].Value);
                VelocityY = int.Parse(matches[3].Value);
            }

            public int X { get; set; }
            public int Y { get; set; }

            public int VelocityX { get; set; }
            public int VelocityY { get; set; }

            public void Move()
            {
                X += VelocityX;
                Y += VelocityY;
            }

            public void MoveBack()
            {
                X -= VelocityX;
                Y -= VelocityY;
            }
        }
    }
}