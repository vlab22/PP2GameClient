using System.Collections;
using System.Collections.Generic;
using GameServerDataFacade;
using UnityEngine;
using UnityEngine.Events;

public class ServerListUiController : MonoBehaviour
{
    public RectTransform serverPanelPrefab;

    public RectTransform contentPanel;
    
    public UnityEvent serverUpdatingEvent;
    public StringUnityEvent serverGetListErrorEvent;

    public GameServerDataBaseFacade gameServerDataFacade;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializePanel();
    }

    private void InitializePanel()
    {
        StartCoroutine(UpdateServersListRoutine());
    }

    private IEnumerator UpdateServersListRoutine()
    {
        serverUpdatingEvent?.Invoke();
        
        yield return gameServerDataFacade.GetGameServersListRoutine();

        if (gameServerDataFacade.serversDetails.Count > 0)
        {
            
        }
        else if (gameServerDataFacade.HasErrors())
        {
            serverGetListErrorEvent?.Invoke(gameServerDataFacade.GetErrorsString());
        }
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
