using System;
using UnityEngine;

public class CrossScenesVars : MonoSingleton<CrossScenesVars>
{
    public string serverAddress = "";
    public int serverPort = -1;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}