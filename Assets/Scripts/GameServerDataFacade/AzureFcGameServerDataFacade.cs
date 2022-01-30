using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace GameServerDataFacade
{
    [Serializable]
    public class AzureFcGameServerDataFacade : GameServerDataBaseFacade
    {
        public string url;
        public string getServerListUri;

        public string userAuth;
        public string userPassAuth;
        public string authCode;

        private void Awake()
        {
            serversDetails = new List<ServerDetailsData>();
            _errorsMap = new Dictionary<string, string>();
        }

        public override IEnumerator GetGameServersListRoutine()
        {
            serversDetails.Clear();
            _errorsMap.Clear();
            
            var httpUrl = url + getServerListUri + $"?code={authCode}";

            var postData = new PostData()
            {
                user_auth = userAuth,
                user_pass_auth = userPassAuth,
            };

            var json = JsonUtility.ToJson(postData);

            var www = new UnityWebRequest(httpUrl, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    $"Fail to request to save game server data: {httpUrl}  | error: {www.error}");
                
                _errorsMap.Add("fail", www.error);
            }
            else
            {
                Debug.Log($"AzureFcGameServerDataFacade upload complete! Data Result: '{www.downloadHandler.text}'");

                var server = JsonUtility.FromJson<ServerListDataFromJson>("{\"servers\":" + www.downloadHandler.text + "}");
                
                serversDetails = server.servers.ToList();
            }
        }
    }

    [Serializable]
    public class ServerListDataFromJson
    {
        public ServerDetailsData[] servers;
    }
}