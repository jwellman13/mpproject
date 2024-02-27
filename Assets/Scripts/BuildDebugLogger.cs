using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDebugLogger : MonoBehaviour
{
    // Code from https://stackoverflow.com/questions/67704820/how-do-i-print-unitys-debug-log-to-the-screen-gui

    public uint qSize = 15; // Set default queue size to 15

    Queue logQueue = new Queue();

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Start build debug logger.");
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        logQueue.Enqueue("[" + type + "] : " + logString);

        if (type == LogType.Exception)
        {
            logQueue.Enqueue(stackTrace);
        }

        while (logQueue.Count > qSize)
        {
            logQueue.Dequeue();
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 400, Screen.height));
        GUILayout.Label("\n" + string.Join("\n", logQueue.ToArray()));
        GUILayout.EndArea();
    }
}
