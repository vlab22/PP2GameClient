using System;
using UnityEngine.Events;

[Serializable]
public class StringUnityEvent : UnityEvent<string>
{
}

[Serializable]
public class String2XUnityEvent : UnityEvent<string,string>
{
}

[Serializable]
public class StringIntUnityEvent : UnityEvent<string,int>
{
}

[Serializable]
public class ServerDataUnityEvent : UnityEvent<ServerDetailsData>
{
    
}