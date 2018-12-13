using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day13Solver : SolverBase
    {
        public Day13Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 13;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string[] input = GetInput().Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            int maxX = input[0].Length;
            int maxY = input.Length;

            char[,] track = new char[maxX, maxY];
            List<Cart> carts = new List<Cart>();

            for (int trackRow = 0; trackRow < maxY; trackRow++)
            for (int trackColumn = 0; trackColumn < maxX; trackColumn++)
            {
                char trackPiece = input[trackRow][trackColumn];
                track[trackColumn, trackRow] = trackPiece;
                if (trackPiece == '<' || trackPiece == '>' || trackPiece == 'v' || trackPiece == '^')
                {
                    carts.Add(new Cart(trackColumn, trackRow, GetDirection(trackPiece)));
                }
            }

            switch (part)
            {
                case ProblemPart.Part1:

                    while (carts.All(c => !c.IsCrashed))
                    {
                        carts.OrderBy(c => c.Y).ThenBy(c => c.X).ForEach(cart =>
                                                                         {
                                                                             cart.MoveCart(track);
                                                                             cart.IsCrashed = carts.Except(new[] {cart}).Any(c => c.X == cart.X && c.Y == cart.Y);
                                                                         });
                    }

                    Cart crashedCart = carts.First(c => c.IsCrashed);
                    AnswerSolution1 = $"{crashedCart.X},{crashedCart.Y}";

                    StopExecutionTimer();

                    return FormatSolution($"The answer [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    while (carts.Count > 1)
                    {
                        carts.OrderBy(c => c.Y).ThenBy(c => c.X).ForEach(cart =>
                                                                         {
                                                                             cart.MoveCart(track);
                                                                             if (!carts.Except(new[] {cart}).Any(c => c.X == cart.X && c.Y == cart.Y))
                                                                             {
                                                                                 return;
                                                                             }

                                                                             Cart cartToRemove = carts.FirstOrDefault(c => c.X == cart.X && c.Y == cart.Y);
                                                                             while (cartToRemove != null)
                                                                             {
                                                                                 carts.Remove(cartToRemove);
                                                                                 cartToRemove = carts.FirstOrDefault(c => c.X == cart.X && c.Y == cart.Y);
                                                                             }
                                                                         });
                    }

                    Cart lastCartStanding = carts.First();
                    AnswerSolution2 = $"{lastCartStanding.X},{lastCartStanding.Y}";

                    StopExecutionTimer();

                    return FormatSolution($"The answer [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private Direction GetDirection(char trackPiece)
        {
            switch (trackPiece)
            {
                case '<':
                    return Direction.Left;
                case '>':
                    return Direction.Right;
                case 'v':
                    return Direction.Down;
                case '^':
                    return Direction.Up;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal class Cart
        {
            private Direction _lastTurnDecision = Direction.Right;

            public Cart(int x, int y, Direction initialDirection)
            {
                X = x;
                Y = y;
                Direction = initialDirection;
            }

            public int X { get; private set; }
            public int Y { get; private set; }
            public Direction Direction { get; private set; }
            public int NumberOfTicks { get; private set; }
            public bool IsCrashed { get; set; }

            public bool MoveCart(char[,] track)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (Direction)
                {
                    case Direction.Left:
                        X = X - 1;
                        break;
                    case Direction.Right:
                        X = X + 1;
                        break;
                    case Direction.Up:
                        Y = Y - 1;
                        break;
                    case Direction.Down:
                        Y = Y + 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                NumberOfTicks++;

                char nextTrack = track[X, Y];

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (nextTrack)
                {
                    case '\\':
                        // ReSharper disable once SwitchStatementMissingSomeCases
                        switch (Direction)
                        {
                            case Direction.Left:
                                Direction = Direction.Up;
                                break;
                            case Direction.Right:
                                Direction = Direction.Down;
                                break;
                            case Direction.Up:
                                Direction = Direction.Left;
                                break;
                            case Direction.Down:
                                Direction = Direction.Right;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case '/':
                        // ReSharper disable once SwitchStatementMissingSomeCases
                        switch (Direction)
                        {
                            case Direction.Left:
                                Direction = Direction.Down;
                                break;
                            case Direction.Right:
                                Direction = Direction.Up;
                                break;
                            case Direction.Up:
                                Direction = Direction.Right;
                                break;
                            case Direction.Down:
                                Direction = Direction.Left;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case '+':
                        Direction decidedDirection = MakeTurnDecision();
                        switch (decidedDirection)
                        {
                            case Direction.Left:
                                switch (Direction)
                                {
                                    case Direction.Up:
                                        Direction = Direction.Left;
                                        break;
                                    case Direction.Down:
                                        Direction = Direction.Right;
                                        break;
                                    case Direction.Left:
                                        Direction = Direction.Down;
                                        break;
                                    case Direction.Right:
                                        Direction = Direction.Up;
                                        break;
                                }

                                break;
                            case Direction.Right:
                                switch (Direction)
                                {
                                    case Direction.Up:
                                        Direction = Direction.Right;
                                        break;
                                    case Direction.Down:
                                        Direction = Direction.Left;
                                        break;
                                    case Direction.Left:
                                        Direction = Direction.Up;
                                        break;
                                    case Direction.Right:
                                        Direction = Direction.Down;
                                        break;
                                }

                                break;
                        }

                        break;
                }

                return false;
            }

            private Direction MakeTurnDecision()
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (_lastTurnDecision)
                {
                    case Direction.Left:
                        _lastTurnDecision = Direction.Straight;
                        return _lastTurnDecision;
                    case Direction.Straight:
                        _lastTurnDecision = Direction.Right;
                        return _lastTurnDecision;
                    case Direction.Right:
                        _lastTurnDecision = Direction.Left;
                        return _lastTurnDecision;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        internal enum Direction
        {
            Left,
            Straight,
            Right,
            Up,
            Down
        }
    }
}
