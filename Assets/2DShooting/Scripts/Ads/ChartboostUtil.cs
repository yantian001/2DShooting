using UnityEngine;
using System.Collections;
using ChartboostSDK;
public class ChartboostUtil  {

    static ChartboostUtil _instance = null;

    public static ChartboostUtil Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = CreateInstance();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    
    //void Awake()
    //{
    //    if(_instance == null)
    //    {
    //        _instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

	// Use this for initialization

    static ChartboostUtil CreateInstance()
    {
        ChartboostUtil rest = new ChartboostUtil();
        rest.Start();
        return rest;
    }
	void Start () {
        
        Chartboost.cacheInterstitial(CBLocation.Default);
        Chartboost.cacheRewardedVideo(CBLocation.Default);

        Chartboost.didDismissRewardedVideo += Chartboost_didDismissRewardedVideo;
        Chartboost.didCompleteRewardedVideo += Chartboost_didCompleteRewardedVideo;
        Chartboost.didFailToLoadRewardedVideo += Chartboost_didFailToLoadRewardedVideo;
        Chartboost.didDisplayRewardedVideo += Chartboost_didDisplayRewardedVideo;
        // Chartboost.showInterstitial(CBLocation.HomeScreen);
        //Chartboost.showRewardedVideo(CBLocation.Default);
	}

    private void Chartboost_didDisplayRewardedVideo(CBLocation obj)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Show video ads at :" + obj);
    }

    private void Chartboost_didFailToLoadRewardedVideo(CBLocation arg1, CBImpressionError arg2)
    {
        // throw new System.NotImplementedException();

        Debug.Log("Load rewarded video failed");
    }

    private void Chartboost_didCompleteRewardedVideo(CBLocation arg1, int arg2)
    {

        Debug.Log("Chartboost_didCompleteRewardedVideo");
        LeanTween.dispatchEvent((int)Events.VIDEOREWARD);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    private void Chartboost_didDismissRewardedVideo(CBLocation obj)
    {
        Debug.Log("Chartboost_didDismissRewardedVideo");
        LeanTween.dispatchEvent((int)Events.VIDEOCLOSED);
    }

    /// <summary>
    /// 是否有游戏结束时的视频广告
    /// </summary>
    /// <returns></returns>
    public bool HasGameOverVideo()
    {
        return Chartboost.hasRewardedVideo(CBLocation.Default);
    }
    /// <summary>
    /// 显示游戏结束时的奖励视频广告
    /// </summary>
    public void ShowGameOverVideo()
    {
        if(Chartboost.hasRewardedVideo(CBLocation.Default))
        {
            Chartboost.showRewardedVideo(CBLocation.Default);
        }

    }

    public bool HasInterstitialOnDefault()
    {
        return Chartboost.hasInterstitial(CBLocation.Default);
    }

    public void ShowInterstitialOnDefault()
    {
        Chartboost.showInterstitial(CBLocation.Default);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
