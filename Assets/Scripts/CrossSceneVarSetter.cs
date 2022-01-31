using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneVarSetter : MonoBehaviour
{
   public void SetCrossSceneVars(string serverAddress, int serverPort)
   {
      CrossScenesVars.Instance.serverAddress = serverAddress;
      CrossScenesVars.Instance.serverPort = serverPort;
   }
}
