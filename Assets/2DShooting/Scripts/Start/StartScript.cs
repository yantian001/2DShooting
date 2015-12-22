using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    /// <summary>
    /// 开始按钮事件
    /// </summary>
   public void OnStartButtonClicked()
    {
        LeanTween.dispatchEvent((int)Events.MAINMENU);
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
