using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using shared;
using UnityEngine;

public class GameClient : MonoBehaviour
{
    public string targetIp = "localhost";
    public int targetPort = 55555;
    private PlayerInfo _thisPlayer;
    public TcpMessageChannel Channel { get; private set; }

    public void Initialize(string pTargetIp, int pPort)
    {
        targetIp = pTargetIp;
        targetPort = pPort;
        
        StartCoroutine(InitializeRoutine());
    }

    IEnumerator InitializeRoutine()
    {
        targetIp = GetIpFromCmdArg(targetIp);
        
        targetPort = GetPortFromCmdArg(targetPort);

        Debug.Log($"Try connect to: {targetIp}:{targetPort}");
        
        yield return new WaitForSeconds(2);

        Channel = new TcpMessageChannel();
        var success = Channel.Connect(targetIp, targetPort);

        Debug.LogWarning($"Success: {success}");

        yield return new WaitForSeconds(2);

        while (true)
        {
            Channel.SendMessage(new WhoAmIRequest());
            yield return new WaitForSeconds(5);
        }
    }

    private string GetIpFromCmdArg(string defaultVal)
    {
        var cmdIpStr = GetArg("--ip");
        return string.IsNullOrWhiteSpace(cmdIpStr) ? defaultVal : cmdIpStr.Trim();
    }

    private int GetPortFromCmdArg(int defaultVal)
    {
        var cmdPortStr = GetArg("--port");

        if (string.IsNullOrWhiteSpace(cmdPortStr))
        {
            return defaultVal;
        }
        
        int cmdPort = -1;

        int.TryParse(cmdPortStr, out cmdPort);

        return cmdPort > -1 ? cmdPort : defaultVal;
    }

    private void Update()
    {
        if (Channel != null)
            ReceiveAndProcessNetworkMessages();
    }

    private void ReceiveAndProcessNetworkMessages()
    {
        if (!Channel.Connected)
        {
            Debug.LogWarning("Trying to receive network messages, but we are no longer connected.");
            return;
        }

        //while there are messages, we have no issues AND we haven't been disabled (important!!):
        //we need to check for gameObject.activeSelf because after sending a message and switching state,
        //we might get an immediate reply from the server. If we don't add this, the wrong state will be processing the message
        while (Channel.HasMessage() && gameObject.activeSelf)
        {
            ASerializable message = Channel.ReceiveMessage();
            HandleNetworkMessage(message);
        }
    }

    private void HandleNetworkMessage(ASerializable pMessage)
    {
        switch (pMessage)
        {
            case WhoAmIResponse whoAmIResponse:
                _thisPlayer = new PlayerInfo()
                {
                    id = whoAmIResponse.idInRoom,
                    userName = whoAmIResponse.userName,
                };
                Debug.LogWarning($"PlayerInfo From Server: {_thisPlayer.userName}");
                break;

                case ValidClientRequest validClientRequest:
                    StartCoroutine(SendValidCodeResponse(validClientRequest.serverCode));
                    break;
            default:
                break;
        }
    }

    private IEnumerator SendValidCodeResponse(string pCode)
    {
        yield return new WaitForSeconds(0.2f);
        
        Channel.SendMessage(new ValidClientResponse()
        {
            code = pCode
        });
    }

    // Helper function for getting the command line arguments
    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }

        return null;
    }
}