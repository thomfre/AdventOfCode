using System.IO;
using JetBrains.Annotations;

namespace Thomfre.AdventOfCode2018.Tools
{
    [UsedImplicitly]
    internal class InputLoader : IInputLoader
    {
        public string LoadInput(int dayNumber)
        {
            string inputFile = $@"Input\Day{dayNumber}.input";
            return File.ReadAllText(inputFile);
        }
    }
}