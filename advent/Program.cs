namespace advent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
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
            var fileData = ReadFile(@"2020-07-input.txt");

            var a202007 = new Advent202007(fileData);
            a202007.StepOne("shiny gold");
            a202007.StepTwo("shiny gold");
        }
    }
}
