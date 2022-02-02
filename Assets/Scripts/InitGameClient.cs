using System;
using System.Collections;
using UnityEngine;

public class InitGameClient : MonoBehaviour
{
    public StringIntUnityEvent initializeClientEvent;

    public string serverAddress = "localhost";
    public int serverPort  = 55555; 
    
    private IEnumerator Start()
    {
        yield return null;

        serverAddress = string.IsNullOrWhiteSpace(CrossScenesVars.Instance.serverAddress) ? serverAddress : CrossScenesVars.Instance.serverAddress;
        serverPort = CrossScenesVars.Instance.serverPort < 1 ? serverPort : CrossScenesVars.Instance.serverPort;

        initializeClientEvent.Invoke(serverAddress, serverPort);
    }
}