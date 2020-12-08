using System;
using System.Collections;
using System.Collections.Generic;

namespace advent
{


    using ActionTuple = Tuple<string, int>;
    using CommandListResponse = Tuple<int, bool>;

    public class Advent202008
    {

        IEnumerable<string> _fileList;
        List<ActionTuple> _commandsList;

        public Advent202008(IEnumerable<string> fileList)
        {
            _fileList = fileList;
            _commandsList = parseCommandsFile(_fileList);
        }

        public void StepOne()
        {
            var accumulator = runCommands(_commandsList).Item1;
            System.Console.WriteLine($"Accumulator is {accumulator}.");
        }

        public void StepTwo()
        {
            var modableCommandsList = new List<ActionTuple>(_commandsList);
            Tuple<int, bool> response;
            ActionTuple originalCommand;
            string newAction;

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
                response = runCommands(modableCommandsList);

                if (response.Item2)
                {
                    System.Console.WriteLine($"Completed list accumulator is {response.Item1}");
                    return;
                }

                System.Console.WriteLine($"Swap on index {commandIdx} ({originalCommand.Item1} => {modableCommandsList[commandIdx].Item1}) failed to complete...");
                modableCommandsList[commandIdx] = originalCommand;
            }

            System.Console.WriteLine("No successful swap found");
        }

        private CommandListResponse runCommands(List<ActionTuple> commandsList)
        {
            var ranCommands = new HashSet<int>();

            var commandIdx = 0;
            var accumulator = 0;

            while (commandIdx < commandsList.Count)
            {
                if (ranCommands.Contains(commandIdx))
                {
                    return new CommandListResponse(accumulator, false);
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

            return new CommandListResponse(accumulator, true);
        }

        private List<ActionTuple> parseCommandsFile(IEnumerable<string> commandLines)
        {
            var commandsList = new List<ActionTuple>();

            foreach (var commandLine in commandLines)
            {
                int value;
                const StringSplitOptions SplitOptions =
                    StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries;

                if (string.IsNullOrWhiteSpace(commandLine))
                {
                    continue;
                }

                var tokens =
                    commandLine
                        .Trim()
                        .Split(" ", SplitOptions);
                var action = tokens[0];
                if (int.TryParse(tokens[1], out value))
                {
                    commandsList.Add(new ActionTuple(action, value));
                }
                else
                {
                    System.Console.WriteLine($"Failed to parse line \"{commandLine}\"");
                }
            }

            return commandsList;
        }
    }
}