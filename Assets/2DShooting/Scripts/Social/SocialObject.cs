using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class SocialObject  {

	public string UserID { get; set; }
    public string UserName { get; set; }
    public string Score { get; set; }
    public int Rank { get; set; }

    public SocialObject()
    {

    }

    public SocialObject(IScore s)
    {
        UserID = s.userID;
        Rank = s.rank;
        Score = s.formattedValue;
    }
}
