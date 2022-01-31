using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameServerDataFacade;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class ServerListUiController : MonoBehaviour
{
    public ServerPanelController serverPanelPrefab;

    public RectTransform contentPanel;

    public UnityEvent serverUpdatingEvent;
    public StringUnityEvent serverGetListErrorEvent;
    public UnityEvent updatedSuccessfulEvent;

    public GameServerDataBaseFacade gameServerDataFacade;

    public float delayBeforeInitialize = 5;

    public float tryAgainAfterDelay = 5;

    private Dictionary<int, ServerDetailsData> _serversMap;
    private Dictionary<int, ServerPanelController> _serversPanelsMap;
    [SerializeField] private bool _serverListUpdating = false;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        _serverListUpdating = false;
        _serversMap = new Dictionary<int, ServerDetailsData>();
        _serversPanelsMap = new Dictionary<int, ServerPanelController>();

        if (delayBeforeInitialize > 0)
            yield return new WaitForSeconds(delayBeforeInitialize);

        InitializePanel();
    }

    private void InitializePanel()
    {
        StartCoroutine(UpdateServersListRoutine());
    }

    private IEnumerator UpdateServersListRoutine()
    {
        if (_serverListUpdating)
            yield break;
        
        _serverListUpdating = true;

        serverUpdatingEvent?.Invoke();

        yield return new WaitForSeconds(0.3f);

        yield return gameServerDataFacade.GetGameServersListRoutine();

        if (gameServerDataFacade.serversDetails.Count > 0)
        {
            _serversMap = gameServerDataFacade.serversDetails.ToDictionary(k => k.id, v => v);
            UpdateServerListUi(gameServerDataFacade.serversDetails);
            updatedSuccessfulEvent?.Invoke();
        }
        else if (gameServerDataFacade.HasErrors())
        {
            Clear();

            serverGetListErrorEvent?.Invoke(gameServerDataFacade.GetErrorsString() +
                                            $", Trying again after {tryAgainAfterDelay} secs");
            StartCoroutine(InitializeAgainAfterDelay());
        }
        else
        {
            StartCoroutine(InitializeAgainAfterDelay());
        }

        _serverListUpdating = false;
    }

    private void UpdateServerListUi(List<ServerDetailsData> servers)
    {
        RemoveOrphansPanels();

        foreach (var srv in servers)
        {
            if (!_serversPanelsMap.TryGetValue(srv.id, out var serverPanel))
            {
                serverPanel = Instantiate<ServerPanelController>(serverPanelPrefab, Vector3.zero, quaternion.identity,
                    contentPanel);
                _serversPanelsMap.Add(srv.id, serverPanel);
            }

            serverPanel.UpdateUiControls(srv);
        }
    }

    private IEnumerator InitializeAgainAfterDelay()
    {
        yield return new WaitForSeconds(tryAgainAfterDelay);

        InitializePanel();
    }

    private void RemoveOrphansPanels()
    {
        var keysToRemove = new List<int>();
        foreach (var srvKv in _serversPanelsMap)
        {
            if (!_serversMap.ContainsKey(srvKv.Key))
            {
                Destroy(srvKv.Value);
                keysToRemove.Add(srvKv.Key);
            }
        }

        foreach (var k in keysToRemove)
        {
            _serversPanelsMap.Remove(k);
        }
    }

    private void Clear()
    {
        _serversMap.Clear();

        foreach (var kv in _serversPanelsMap)
        {
            Destroy(kv.Value);
        }

        _serversPanelsMap.Clear();
    }

    public void TryRefresh()
    {
        StartCoroutine(UpdateServersListRoutine());
    }
}