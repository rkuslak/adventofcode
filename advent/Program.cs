﻿namespace advent
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {

        const String rootPath = @"/workspaces/advent";


        public static List<string> ReadFile(string fileName)
        {
            var filePath = Path.Combine(rootPath, fileName);
            var result = new List<string>();

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
            var fileData = ReadFile(@"2020-11-input.txt");
            var adventure = new Advent202011(fileData);

            adventure.StepOne();
            adventure.StepTwo();
        }
    }
}
