using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {

   // public GoogleAnalyticsV3 googleAnalyticsV3;

	// Use this for initialization
	//void Start () {
 //       Application.targetFrameRate = 60;
	//}

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// 开始按钮事件
    /// </summary>
   public void OnStartButtonClicked()
    {
        LeanTween.dispatchEvent((int)Events.MAINMENU,false);
    }

    public void OnHighScoreClicked()
    {
        SocialManager.Instance.ShowLeardBoardUI();
    }


    public void OnMoreAppClicked()
    {
        if(ChartboostUtil.Instance.HasMoreAppOnDefault())
        {
            ChartboostUtil.Instance.ShowMoreAppOnDefault();
        }
    }


	// Update is called once per frame
	void Update () {
	
	}
}
