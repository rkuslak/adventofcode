namespace advent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Advent202007
    {
        private Dictionary<string, Dictionary<string, int>> _relationshipMap;

        public Advent202007(IEnumerable<string> infile)
        {
            _relationshipMap = new Dictionary<string, Dictionary<string, int>>();

            foreach (var line in infile)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var split = line.Split(" contain ", StringSplitOptions.None);
                var key = cleanBagKey(split[0]);

                _relationshipMap[key] = new Dictionary<string, int>();

                if (split[1].StartsWith("no "))
                {
                    continue;
                }

                var childLines = split[1].Split(
                        new[] { ", ", "." },
                        StringSplitOptions.RemoveEmptyEntries
                );

                foreach (var childLine in childLines)
                {
                    var splitIdx = childLine.IndexOf(' ');
                    var bagAmountString = childLine.Substring(0, splitIdx);
                    var bagColor = childLine.Substring(splitIdx + 1);
                    bagColor = cleanBagKey(bagColor);
                    var bagsAmount = int.Parse(bagAmountString);
                    _relationshipMap[key][bagColor] = bagsAmount;
                }

            }
        }

        private string cleanBagKey(string rawBagKey)
        {
            return rawBagKey
                    .Replace("bags", "")
                    .Replace("bag", "")
                    .Trim();
        }

        public void StepOne(string bagName)
        {
            var canContain = new HashSet<string>(FindBagsContaining(bagName));
            var newBagNames = new List<string>(canContain);

            while (newBagNames.Count > 0)
            {
                var foundBagNames = new List<string>();

                foreach (var searchBagName in canContain)
                {
                    foundBagNames.AddRange(FindBagsContaining(searchBagName));
                }
                newBagNames = new List<string>();

                foreach (var foundBag in foundBagNames)
                {
                    if (!canContain.Contains(foundBag))
                    {
                        canContain.Add(foundBag);
                        newBagNames.Add(foundBag);
                    }
                }
            }
            System.Console.WriteLine($"{canContain.Count} matches");
        }

        public void StepTwo(string bagName)
        {
            var totalBags = getSubbagCount(bagName);
            System.Console.WriteLine($"{totalBags} total bags");
        }

        int getSubbagCount(string bagName)
        {
            var totalBags = 0;

            foreach (var subBagName in _relationshipMap[bagName].Keys)
            {
                var numberOfSubBags = _relationshipMap[bagName][subBagName];

                var subBagCount = getSubbagCount(subBagName);

                subBagCount *= numberOfSubBags;
                totalBags += numberOfSubBags;
                totalBags += subBagCount;
            }

            return totalBags;
        }

        private IEnumerable<string> FindBagsContaining(string bagName)
        {
            var result = new List<string>();

            foreach (var parentBagName in _relationshipMap.Keys)
            {
                foreach (var childBagName in _relationshipMap[parentBagName].Keys)
                {
                    if (childBagName.Equals(bagName))
                    {
                        result.Add(parentBagName);
                        break;
                    }
                }
            }

            return result;
        }
    }
}