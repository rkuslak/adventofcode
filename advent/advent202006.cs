namespace advent
{
    using System.Collections;
    using System.Collections.Generic;

    public class Advent202006
    {
        readonly IEnumerable<string> _inputList;

        public Advent202006(IEnumerable<string> inputList)
        {
            _inputList = inputList;
        }

        public void PartOne()
        {
            var totalAnswers = 0;
            var matchedAnswers = 0;
            var parsedLines = 0;
            var answerKeys = new Dictionary<char, int>();

            foreach (var line in _inputList)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    totalAnswers += answerKeys.Count;
                    foreach (var key in answerKeys.Keys)
                    {
                        if (answerKeys[key] == parsedLines)
                        {
                            matchedAnswers++;
                        }
                    }
                    answerKeys.Clear();
                    parsedLines = 0;
                    continue;
                }

                foreach (var lineCh in line.ToCharArray())
                {
                    var currentCount = 0;
                    if (!answerKeys.TryGetValue(lineCh, out currentCount))
                    {
                        currentCount = 0;
                    }
                    currentCount++;
                    answerKeys[lineCh] = currentCount;
                }
                parsedLines++;
            }
            totalAnswers += answerKeys.Count;
            foreach (var key in answerKeys.Keys)
            {
                if (answerKeys[key] == parsedLines)
                {
                    matchedAnswers++;
                }
            }
            answerKeys.Clear();

            System.Console.WriteLine($"Total: {totalAnswers}");
            System.Console.WriteLine($"Matched: {matchedAnswers}");
        }
    }
}