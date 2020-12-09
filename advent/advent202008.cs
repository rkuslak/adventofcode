using System;
using System.Collections;
using System.Collections.Generic;

namespace advent
{
    using ActionTuple = Tuple<string, int>;

    public class Advent202008
    {

        List<ActionTuple> _commandsList;
        const StringSplitOptions CommandLineSplitOptions =
            StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries;

        public Advent202008(IEnumerable<string> fileList)
        {
            _commandsList = parseCommandsFile(fileList);
        }

        public void StepOne()
        {
            var accumulator = runCommands(_commandsList, true);
            System.Console.WriteLine($"Accumulator is {accumulator}.");
        }

        public void StepTwo()
        {
            var modableCommandsList = new List<ActionTuple>(_commandsList);
            ActionTuple originalCommand;
            string newAction;
            int? response;

            for (var commandIdx = 0; commandIdx < modableCommandsList.Count; commandIdx++)
            {
                originalCommand = modableCommandsList[commandIdx];

                switch (originalCommand.Item1)
                {
                    case "nop":
                        newAction = "jmp";
                        break;
                    case "jmp":
                        newAction = "nop";
                        break;
                    default:
                        continue;
                }

                modableCommandsList[commandIdx] = new ActionTuple(newAction, originalCommand.Item2);
                response = runCommands(modableCommandsList, false);

                if (null != response)
                {
                    System.Console.WriteLine($"Completed list accumulator is {response}");
                    return;
                }

                System.Console.WriteLine(
                    "Swap on index {0} ({1} => {2}) failed to complete",
                    commandIdx,
                    originalCommand.Item1,
                    modableCommandsList[commandIdx].Item1
                );

                modableCommandsList[commandIdx] = originalCommand;
            }

            System.Console.WriteLine("No successful swap found");
        }

        private int? runCommands(List<ActionTuple> commandsList, bool returnPartial)
        {
            var ranCommands = new HashSet<int>();

            var commandIdx = 0;
            var accumulator = 0;

            while (commandIdx < commandsList.Count)
            {
                if (ranCommands.Contains(commandIdx))
                {
                    return returnPartial ? accumulator : null;
                }

                ranCommands.Add(commandIdx);

                var action = commandsList[commandIdx].Item1;
                var value = commandsList[commandIdx].Item2;

                switch (action)
                {
                    case "jmp":
                        commandIdx += value;
                        break;
                    case "acc":
                        commandIdx++;
                        accumulator += value;
                        break;
                    case "nop":
                        commandIdx++;
                        break;
                    default:
                        System.Console.WriteLine($"Invalid command: {action}");
                        break;
                }
            }

            return accumulator;
            // return new CommandListResponse(accumulator, true);
        }

        private List<ActionTuple> parseCommandsFile(IEnumerable<string> commandLines)
        {
            string[] tokens;
            int value;

            var commandsList = new List<ActionTuple>();

            foreach (var commandLine in commandLines)
            {
                if (string.IsNullOrWhiteSpace(commandLine))
                {
                    continue;
                }

                tokens = commandLine.Trim().Split(" ", CommandLineSplitOptions);
                if (tokens.Length != 2 || !int.TryParse(tokens[1], out value))
                {
                    System.Console.WriteLine($"Failed to parse line \"{commandLine}\"");
                    continue;
                }

                commandsList.Add(new ActionTuple(tokens[0], value));
            }

            return commandsList;
        }
    }
}