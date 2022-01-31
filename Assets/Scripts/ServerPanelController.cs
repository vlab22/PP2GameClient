using System;
using UnityEngine;
using UnityEngine.UI;

public class ServerPanelController : MonoBehaviour
{
    public Text serverNameLabel;
    public Text playersCountLabel;
    public Text serverStatusLabel;
    public Button connectButton;

    public CrossSceneVarSetter sceneVarSetter;
    public SceneLoader sceneLoader;
    
    private void Awake()
    {
        sceneVarSetter = FindObjectOfType<CrossSceneVarSetter>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void UpdateUiControls(ServerDetailsData serverData)
    {
        serverNameLabel.text = serverData.name;
        playersCountLabel.text = $"{serverData.playersCount}/{serverData.maxPlayers}";
        serverStatusLabel.text = $"{serverData.status}";

        UpdateStatusLabelColor(serverData.status);
        
        UpdateButtonEvent( $"{serverData.dns}.{serverData.region}.azurecontainer.io", serverData.port, serverData.status, serverData.maxPlayers, serverData.playersCount);
    }

    void UpdateStatusLabelColor(string pStatus)
    {
        Color c = pStatus.ToLower() switch
        {
            "running" => Color.blue,
            "initializing" => Color.yellow,
            _ => new Color(50, 50, 50)
        };

        serverStatusLabel.color = c;
    }

    private void UpdateButtonEvent(string pDns, int pPort, string pStatus, int pMaxPlayers, int pPlayersCount)
    {
        connectButton.interactable = pStatus.ToLower() == "running" && pPlayersCount < pMaxPlayers;
        
        connectButton.onClick.AddListener(delegate
        {
            sceneVarSetter.SetCrossSceneVars(pDns, pPort);
            sceneLoader.LoadSceneFromSceneName();
        });
    }
}