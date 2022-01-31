using System;

[Serializable]
public class ServerDetailsData
{
    public int id;
    public string name;
    public string dns;
    public int port;
    public string region;
    public int maxPlayers;
    public int playersCount;
    public string status;
}