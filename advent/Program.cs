using System;

namespace advent
{

    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;

    class Program
    {

        const String rootPath = @"/workspaces/advent";


        public static List<String> ReadFile(String fileName)
        {
            var filePath = Path.Combine(rootPath, fileName);
            var result = new List<String>();

            var inputFile = System.IO.File.ReadAllText(filePath);

            var splitLines = inputFile.Split(
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
            );

            foreach (var line in splitLines)
            {
                result.Add(line);
            }

            return result;
        }

        static void Main(string[] args)
        {
            var fileData = ReadFile(@"2020-06-input.txt");

            var a202006 = new Advent202006(fileData);
            a202006.PartOne();
        }
    }
}
