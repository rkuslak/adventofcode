namespace advent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Advent202009
    {
        List<long> _values;

        public Advent202009(IEnumerable<string> fileList)
        {
            _values = new List<long>();
            long value;

            foreach (var line in fileList)
            {
                if (string.IsNullOrWhiteSpace(line)) { continue; }

                if (!long.TryParse(line, out value))
                {
                    System.Console.WriteLine($"Failed to parse: {line}");
                    throw new Exception($"Failed to parse: {line}");
                }
                _values.Add(value);
            }
        }

        public void StepOne()
        {
            var invalidIdxs = new List<int>();
            const int preamble = 25;

            for (var compareIdx = preamble; compareIdx < _values.Count; compareIdx++)
            {
                if (!isValid(compareIdx, _values[compareIdx], preamble))
                {
                    invalidIdxs.Add((int)compareIdx);
                }
            }

            foreach (var invalidIdx in invalidIdxs)
            {
                System.Console.WriteLine($"Invalid idx: {invalidIdx}: {_values[invalidIdx]}");
            }
        }

        bool isValid(int compareIdx, long desiredValue, int preamble)
        {
            for (int rootIdx = compareIdx - preamble; rootIdx < compareIdx; rootIdx++)
            {
                for (var additiveIdx = compareIdx - 1; additiveIdx > rootIdx; additiveIdx--)
                {
                    if (_values[rootIdx] == _values[additiveIdx])
                    {
                        continue;
                    }

                    if (_values[rootIdx] + _values[additiveIdx] == desiredValue)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        public void StepTwo()
        {
            const long desiredSum = 375054920;
            var (startIdx, endIdx) = findSumIndexes(desiredSum);

            long lowValue = long.MaxValue;
            long highValue = 0;

            for (var idx = startIdx; idx <= endIdx; idx++)
            {
                if (_values[idx] > highValue) { highValue = _values[idx]; }
                if (_values[idx] < lowValue) { lowValue = _values[idx]; }
            }

            long valuesSum = lowValue + highValue;

            System.Console.WriteLine($"{startIdx} - {endIdx}: {lowValue} - {highValue}; {valuesSum}");
        }

        private (int, int) findSumIndexes(long desiredSum)
        {
            for (var startIdx = 0; startIdx < _values.Count - 1; startIdx++)
            {
                long sum = _values[startIdx];

                for (var endIdx = startIdx + 1; endIdx < _values.Count; endIdx++)
                {
                    sum += _values[endIdx];
                    if (sum == desiredSum)
                    {
                        return (startIdx, endIdx);
                    }
                }
            }

            throw new Exception("Can't find sum");
        }
    }
}