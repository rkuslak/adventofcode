namespace advent
{
    using System;
    using System.Collections.Generic;

    public class Advent202010
    {
        List<uint> _sortedJoltRatings = new List<uint>();
        Dictionary<int, long> _cache = new Dictionary<int, long>();


        public Advent202010(IEnumerable<string> _inFile)
        {
            uint value;

            foreach (var line in _inFile)
            {
                if (string.IsNullOrWhiteSpace(line))
                { continue; }

                if (!uint.TryParse(line, out value))
                {
                    throw new Exception($"Failed to parse {line}");
                }
                _sortedJoltRatings.Add(value);
            }
            _sortedJoltRatings.Sort();
        }

        public void StepOne()
        {
            // Final "step" will be a 3 step outside of the values list to
            // indicate the device itself
            uint singleSteps = 0;
            uint tripleSteps = 1;
            uint lastAdaptorJolt = 0;

            for (var joltIdx = 0; joltIdx < _sortedJoltRatings.Count; joltIdx++)
            {
                switch (_sortedJoltRatings[joltIdx] - lastAdaptorJolt)
                {
                    case 1:
                        singleSteps++;
                        break;
                    case 3:
                        tripleSteps++;
                        break;
                    default:
                        throw new Exception("Failed!!!");
                }
                lastAdaptorJolt = _sortedJoltRatings[joltIdx];
            }

            var solution = singleSteps * tripleSteps;
            Console.WriteLine($"Single: {singleSteps}, Triple: {tripleSteps} - {solution}");
        }

        public void StepTwo()
        {
            var sum = findValidCombos(0, 0);
            Console.WriteLine($"Combinations: {sum}");
        }

        private long findValidCombos(uint currentJolt, int startIdx)
        {
            long result;
            var maxIdx = _sortedJoltRatings.Count;
            uint joltRangeMax = currentJolt + 3;

            if (startIdx == _sortedJoltRatings.Count) { return 1; }

            // We can safely assume if we're on startIdx of 0 that we are
            // evaluating the full data set. Zero out cache on the initial run
            if (startIdx == 0)
            {
                _cache.Clear();
            }
            else if (_cache.TryGetValue(startIdx, out result))
            {
                return result;
            }

            result = 0;
            for (var idx = startIdx; idx < maxIdx && idx <= startIdx + 3; idx++)
            {
                if (_sortedJoltRatings[idx] <= joltRangeMax)
                {
                    result += findValidCombos(_sortedJoltRatings[idx], idx + 1);
                }
            }

            _cache[startIdx] = result;
            return result;
        }
    }
}