using System;
using System.Collections;
using UnityEngine;

public class InitGameClient : MonoBehaviour
{
    public StringIntUnityEvent initializeClientEvent;

    private IEnumerator Start()
    {
        yield return null;

        var serverAddress = CrossScenesVars.Instance.serverAddress;
        var serverPort = CrossScenesVars.Instance.serverPort;

        initializeClientEvent.Invoke(serverAddress, serverPort);
    }
}