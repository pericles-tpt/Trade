using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;

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

        // Update is called once per frame
        void Update()
        {

        }

        public string[] ProcessCommand (string a)
        {
            AddCommandHistory(a);
            return ExecuteCommand(a);
        }

        public string[] ExecuteCommand(string a)
        {
            return new string[] { "> " + a, "COMMAND ACCEPTED" };
        }

        public void AddCommandHistory (string a)
        {
            CommandHistory.Enqueue(a);
            if (CommandHistory.Count > CommandLimit)
            {
                CommandHistory.Dequeue();
            }
        }
    }
}