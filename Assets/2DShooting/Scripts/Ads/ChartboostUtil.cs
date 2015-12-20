using UnityEngine;
using System.Collections;
using ChartboostSDK;
public class ChartboostUtil : MonoBehaviour {

    static ChartboostUtil _instance = null;

    public static ChartboostUtil Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ChartboostUtil>();
                if(_instance == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "ChartboostUtilContainer";
                    _instance = gameObject.AddComponent<ChartboostUtil>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        
        Chartboost.cacheInterstitial(CBLocation.HomeScreen);
        Chartboost.cacheInterstitial(CBLocation.GameOver);
        Chartboost.cacheRewardedVideo(CBLocation.GameOver);

        Chartboost.didDismissRewardedVideo += Chartboost_didDismissRewardedVideo;
        Chartboost.didCompleteRewardedVideo += Chartboost_didCompleteRewardedVideo;
        Chartboost.didFailToLoadRewardedVideo += Chartboost_didFailToLoadRewardedVideo;
        Chartboost.shouldDisplayRewardedVideo += Chartboost_shouldDisplayRewardedVideo;
        Chartboost.shouldDisplayInterstitial += Chartboost_shouldDisplayInterstitial;
	}

    private bool Chartboost_shouldDisplayInterstitial(CBLocation arg1)
    {
        //throw new System.NotImplementedException();
        Chartboost.showInterstitial(arg1);
        return true;
    }

    private bool Chartboost_shouldDisplayRewardedVideo(CBLocation arg1)
    {
        // throw new System.NotImplementedException();
         Chartboost.showRewardedVideo(arg1);
        return true;
    }

    private void Chartboost_didFailToLoadRewardedVideo(CBLocation arg1, CBImpressionError arg2)
    {
        // throw new System.NotImplementedException();

        Debug.Log("Load rewarded video failed");
    }

    private void Chartboost_didCompleteRewardedVideo(CBLocation arg1, int arg2)
    {
        Debug.Log("Chartboost_didCompleteRewardedVideo");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    private void Chartboost_didDismissRewardedVideo(CBLocation obj)
    {
        Debug.Log("Chartboost_didDismissRewardedVideo");
    }

    /// <summary>
    /// 是否有游戏结束时的视频广告
    /// </summary>
    /// <returns></returns>
    public bool HasGameOverVideo()
    {
        return Chartboost.hasRewardedVideo(CBLocation.GameOver);
    }
    /// <summary>
    /// 显示游戏结束时的奖励视频广告
    /// </summary>
    public void ShowGameOverVideo()
    {
        if(Chartboost.hasRewardedVideo(CBLocation.GameOver))
        {
            Chartboost.showRewardedVideo(CBLocation.GameOver);
        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
