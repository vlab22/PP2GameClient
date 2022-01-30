public class PostData
{
    public string user_auth;
    public string user_pass_auth;
}

public class PlayerCounterPostData : PostData
{
    public int server_id;
    public int player_count;
}