using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using System.Linq;
using UnityEditor.Compilation;

namespace Assets.Scripts.Core.Management
{
    public class CommandManager : MonoBehaviour
    {

        Queue<string> CommandHistory;
        const int CommandLimit = 100;

        // Use this for initialization
        void Start()
        {
            CommandHistory = new Queue<string>();
        }

        public string[] ProcessCommand (string a)
        {
            AddCommandHistory(a);
            return ExecuteCommand(a);
        }

        public string[] ExecuteCommand(string a)
        {
            // Output from command processing, input + command output
            string[] tokenizedString = a.Split(' ');

            string[] commandOutput = ParseArguments(tokenizedString);
            string[] inputCommandOutput = new string[commandOutput.Length + 1];

            inputCommandOutput[0] = "> " + a;
            for (int i = 1; i <= (commandOutput.Length); i++)
            {
                inputCommandOutput[i] = commandOutput[i - 1];
            }


            return inputCommandOutput;
        }

        public void AddCommandHistory (string a)
        {
            CommandHistory.Enqueue(a);
            if (CommandHistory.Count > CommandLimit)
            {
                CommandHistory.Dequeue();
            }
        }

        private string[] ParseArguments( string[] arguments )
        {
            string[] response = { "COMMAND NOT RECOGNISED" };
            string fArg = arguments[0].ToString().ToUpper();

            switch (fArg)
            {
                case "HELP":
                    response = CHelp(arguments.Skip(1).Take(arguments.Length - 1).ToArray());
                    break;
                case "MOVE":
                    response = CMove(arguments.Skip(1).Take(arguments.Length - 1).ToArray());
                    break;
                case "HIRE":
                    response = CHire(arguments.Skip(1).Take(arguments.Length - 1).ToArray());
                    break;
                case "SCHEDULE":
                    response = CSchedule(arguments.Skip(1).Take(arguments.Length - 1).ToArray());
                    break;
                case "BUY":
                    response = CBuy(arguments.Skip(1).Take(arguments.Length - 1).ToArray());
                    break;
                case "SELL":
                    response = CSell(arguments.Skip(1).Take(arguments.Length - 1).ToArray());
                    break;
                case "MFEST":
                    response = CMfest(arguments.Skip(1).Take(arguments.Length - 1).ToArray());
                    break;
                default:
                    break;

            }

            return response;
        }

        private string[] InvalidNoArgsCheck(string[] providedArgs, string[] expectedArgs, int no)
        {
            if (providedArgs.Length != no)
            {

                string allArgsExpected = "";
                foreach (string i in expectedArgs)
                {
                    allArgsExpected += "[" + i + "] ";
                }

                if (expectedArgs.Length > 0)
                {
                    providedArgs = new string[] { "INVALID NUMBER OF ARGUMENTS, EXPECTED " + no, allArgsExpected };
                } else
                {
                    providedArgs = new string[] { "INVALID NUMBER OF ARGUMENTS, EXPECTED " + no};
                }

                return providedArgs;

            } else
            {
                return null;
            }

        }

        private string[] CSchedule(string[] arguments)
        {
            int noArgs = 3;
            string[] response = { "THINGO SCHEDULED" };
            string[] expectedArgs = { "TIME", "DATE", "PLACE" };

            string[] responseInvalidNo = InvalidNoArgsCheck(arguments, expectedArgs, noArgs);

            if (responseInvalidNo == null)
            {
                return response;
            } else
            {
                return responseInvalidNo;
            }
        }

        private void CScheduleHelp(ref List<string> response)
        {
            response.Add("NAME: schedule");
            response.Add("ARGUMENTS: [time] [date/day] [script]");
            response.Add("DESCRIPTION: Schedules a script to run at a given time and date or day of the week");
            response.Add("(days of the week are defined from 0 - 6 for Monday - Sunday");
            response.Add("EXAMPLES: schedule 0400 01,11,3989 showShips.run");

        }

        private string[] CMove(string[] arguments)
        {
            string[] response = { "THINGO MOVED" };
            return response;
        }

        private string[] CHire(string[] arguments)
        {
            string[] response = { "THINGO HIRED" };
            return response;
        }

        private string[] CBuy(string[] arguments)
        {
            string[] response = { "THINGO BOUGHT" };
            return response;
        }

        private string[] CSell(string[] arguments)
        {
            string[] response = { "THINGO SOLD" };
            return response;
        }

        private string[] CMfest(string[] arguments)
        {
            string[] response = { "MANIFEST SHOWS:" };
            return response;
        }

        private string[] CHelp(string[] arguments)
        {
            List<string> response = new List<string>();



            int noArgs = 1;
            string[] expectedArgs = { "[command]" };

            string[] responseInvalidNo = InvalidNoArgsCheck(arguments, expectedArgs, noArgs);

            string fArg = arguments[0].ToString().ToUpper();
            Debug.Log(fArg);
            if (responseInvalidNo == null)
            {
                switch (fArg)
                {
                    case "SCHEDULE":
                        CScheduleHelp(ref response);
                        break;
                    default:
                        response.Add("PLEASE PROVIDE A VALID COMMAND TO GET HELP");
                        response.Add("VALID COMMANDS INCLUDE:");
                        response.Add("  schedule, move, buy, hire, sell, mfest, help");
                        break;
                }

                string[] responseArray = response.ToArray();
                return responseArray;

            }
            else
            {
                return responseInvalidNo;
            }
        }
    }
}