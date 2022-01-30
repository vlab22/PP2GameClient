using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameServerDataFacade
{
    public abstract class GameServerDataBaseFacade : MonoBehaviour
    {
        public List<ServerDetailsData> serversDetails;
        protected Dictionary<string,string> _errorsMap;

        public abstract IEnumerator GetGameServersListRoutine();

        public bool HasErrors()
        {
            return _errorsMap.Count > 0;
        }

        public string GetErrorsString()
        {
            return String.Join((string)" | ", (IEnumerable<string>)GetErrorsList());
        }

        public List<string> GetErrorsList()
        {
            return _errorsMap.Values.ToList();
        }
    }
}