using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Core.Management;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class EndEditTerminalBehaviour : MonoBehaviour
    {

        GameObject term;
        CommandManager cm;

        public void TerminalEnter()
        {
            term = GameObject.Find("Terminal");
            cm = term.GetComponent<CommandManager>();
            Text CommandToExecute = GameObject.Find("TerminalInputText").GetComponent<Text>();

            // 1. Store string in string history + Get response from backend
            string[] output = cm.ProcessCommand(CommandToExecute.text);

            // 2. Move all command (including this one) and output up
            int moveUpBy = output.Length;
            Debug.Log(moveUpBy);
            int i, j;
            for (i = 0; i < moveUpBy; i++)
            {
                for (j = 9; j > -1; j--)
                {
                    if (j == 0)
                        term.transform.Find("Text" + j).GetComponent<Text>().text = output[i];
                    else
                        term.transform.Find("Text" + j).GetComponent<Text>().text = term.transform.Find("Text" + (j - 1)).GetComponent<Text>().text;

                }
                Debug.Log(i);
            }

            // 4. Clear input field
            CommandToExecute.text = "";


        }
    }
}