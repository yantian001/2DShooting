using UnityEngine;
using System.Collections;
using Parse;

public class NetworkHandler : MonoBehaviour
{

    #region 单例模式

    private static NetworkHandler _instance = null;
    public static NetworkHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NetworkHandler>();
                if (_instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "NetworkHandlerContainer";
                    _instance = container.AddComponent<NetworkHandler>();
                }

            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }


    public void SavePlayer2Server(Player play)
    {

        if (string.IsNullOrEmpty(play.ObjectID))
        {
            ParseObject obj = new ParseObject("TestGameScore");
            ConvertPlayer2Object(play, ref obj);
            obj.SaveAsync().ContinueWith(t =>
            {
                
                Debug.Log(obj.Get<string>("objectId"));
            });
        }
        //ParseObject obj = new ParseObject("TestGameScore");
        //ConvertPlayer2Object(play, ref obj);
        //obj.SaveAsync().ContinueWith(t =>
        //{
        //    Debug.Log(t);
        //    //Debug.Log(obj.Get<string>("objectId"));
        //});
    }

    void ConvertPlayer2Object(Player player, ref ParseObject obj)
    {
        if (player != null && obj != null)
        {
            obj["userName"] = player.UserName;
            obj["userID"] = player.UserID;
            obj["levelScores"] = player.LevelScores;
        }
    }
    #endregion
}
