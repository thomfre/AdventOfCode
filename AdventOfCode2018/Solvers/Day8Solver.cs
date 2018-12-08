using System;
using System.Collections.Generic;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day8Solver : SolverBase
    {
        public Day8Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 8;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            int[] license = GetInput().Split(' ').Select(int.Parse).ToArray();
            List<Node> nodes = new List<Node>();

            nodes = GetNodes(license).ToList();

            switch (part)
            {
                case ProblemPart.Part1:
                    AnswerSolution1 = GetMetadataSum(nodes);

                    StopExecutionTimer();

                    return FormatSolution($"The total sum of the metadata is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    AnswerSolution2 = GetNodeValue(nodes[0]);

                    StopExecutionTimer();

                    return FormatSolution($"The value of the root node is [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private int GetNodeValue(Node node)
        {
            if (node.Children.Count == 0)
            {
                return node.Metadata.Sum();
            }

            int[] childValues = node.Children.Select(GetNodeValue).ToArray();

            int value = 0;
            foreach (int metadata in node.Metadata)
            {
                if (childValues.Length >= metadata)
                {
                    value += childValues[metadata - 1];
                }
            }

            return value;
        }

        private int GetMetadataSum(List<Node> nodes)
        {
            int sum = nodes.Sum(x => x.Metadata.Sum());
            return sum + nodes.Sum(x => GetMetadataSum(x.Children));
        }

        private IEnumerable<Node> GetNodes(int[] license)
        {
            for (int i = 0; i < license.Length; i++)
            {
                yield return GetNode(license, ref i);
            }
        }

        private Node GetNode(IReadOnlyList<int> license, ref int start)
        {
            Node node = new Node();
            int numberOfChildren = license[start++];
            int numberOfMetadata = license[start++];

            for (int i = 0; i < numberOfChildren; i++)
            {
                node.Children.Add(GetNode(license, ref start));
            }

            for (int i = 0; i < numberOfMetadata; i++)
            {
                node.Metadata.Add(license[start++]);
            }

            return node;
        }

        internal class Node
        {
            public Node()
            {
                Children = new List<Node>();
                Metadata = new List<int>();
            }

            public Node(IEnumerable<Node> nodes, IEnumerable<int> metadata)
            {
                Children = new List<Node>(nodes);
                Metadata = new List<int>(metadata);
            }

            public List<Node> Children { get; set; }
            public List<int> Metadata { get; set; }
        }
    }
}