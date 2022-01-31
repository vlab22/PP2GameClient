using System;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleToGUI : MonoBehaviour
{
    public Text textArea;
    
    void OnEnable() { Application.logMessageReceived += Log; }
    void OnDisable() { Application.logMessageReceived -= Log; }
    public void Log(string logString, string stackTrace, LogType type)
    {
        textArea.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + logString + Environment.NewLine + textArea.text;
    }
}