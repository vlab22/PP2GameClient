using UnityEngine;

namespace GameServerDataFacade
{
    public class InitializeAzFcDataFacade : MonoBehaviour
    {
        public AzureFcGameServerDataFacade facade;

        public bool isLocalhost;
    
        public string userAuth = "pp2game";
        public string userAuthPass = "KDcx6kB2v77B5bm5AwY7XakYSsSB7Q4R";
        public string authCode = "s1GIzLcjWf3uSHN7WS8HIbhh4LFWdVIsi25scAASvpBuy9u7JkRcWw==";
    
        public string url = "https://pp2gameazurefcs.azurewebsites.net";

        public string getServerListUri = "/api/ProcessNewConnectionFcAz";
    
        public string localHostUrl = "http://localhost:7071";
    
        private void Awake()
        {
            var urlParam = GetArg("url");
            facade.url = !string.IsNullOrWhiteSpace(urlParam) ? urlParam : isLocalhost ? localHostUrl : url;
            facade.getServerListUri = getServerListUri;

            facade.userAuth = userAuth;
            facade.userPassAuth = userAuthPass;
            facade.authCode = authCode;
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
}
