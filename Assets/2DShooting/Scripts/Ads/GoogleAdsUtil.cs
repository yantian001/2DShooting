using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class GoogleAdsUtil : MonoBehaviour
{

    public string androidUnitId = "ca-app-pub-8295605020027148/3214523116";

    public string iOSUnitId = "ca-app-pub-8295605020027148/7644722719";

    public string androidBannerUnitId = "ca-app-pub-8295605020027148/5886957911";

    public string iOSBannerUnitId = "ca-app-pub-8295605020027148/7363691119";

    public static GoogleAdsUtil Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GoogleAdsUtil>();
                if (_instance == null)
                {
                    GameObject container = new GameObject();
                    container.name = "Google Ads Container";
                    _instance = container.AddComponent<GoogleAdsUtil>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    private static GoogleAdsUtil _instance;

    InterstitialAd intersititial = null;
    BannerView banner = null;
    BannerView bannerPause = null;
    BannerView bannerHome = null;
    public void Awake()
    {
        if (_instance == null)
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
    void Start()
    {
        RequestInterstitial();
        //RequestTopBannerView();
        RequestPauseBannerView();
        //RequestHomeBannerView();
    }

    void RequestHomeBannerView()
    {
#if UNITY_ANDROID
        string adUnitID = "ca-app-pub-8295605020027148/4775531117";
#elif UNITY_IPHONE
        string adUnitID = "ca-app-pub-8295605020027148/4855719914";
#endif
        //AdSize size = new AdSize(900, 300);
        bannerHome = new BannerView(adUnitID, AdSize.Banner, AdPosition.BottomLeft);
        AdRequest request = new AdRequest.Builder().Build();
        bannerHome.LoadAd(request);
        bannerHome.AdFailedToLoad += Banner_AdFailedToLoad;
        bannerHome.Hide();

    }

    public void ShowHomeBannerView()
    {
        if (bannerHome != null)
            bannerHome.Show();
    }

    public void HideHomeBannerView()
    {
        if (bannerHome != null)
            bannerHome.Hide();
    }


    void RequestTopBannerView()
    {
#if UNITY_ANDROID
        string adUnitID = androidBannerUnitId;
#elif UNITY_IPHONE
        string adUnitID = iOSBannerUnitId;
#endif
        //AdSize size = new AdSize(900, 300);
        banner = new BannerView(adUnitID, AdSize.Banner, AdPosition.Top);
        AdRequest request = new AdRequest.Builder().Build();
        banner.LoadAd(request);
        banner.AdFailedToLoad += Banner_AdFailedToLoad;
        banner.Hide();

    }

    /// <summary>
    /// 显示暂停banner
    /// </summary>
    public void ShowPauseBanner()
    {
        if(bannerPause != null)
        {
            bannerPause.Show();
        }
    }

    /// <summary>
    /// 隐藏banner
    /// </summary>
    public void HidePauseBanner()
    {
        if (bannerPause != null)
        {
            bannerPause.Hide();
        }
    }


    void RequestPauseBannerView()
    {
#if UNITY_ANDROID
        string adUnitID = "ca-app-pub-8295605020027148/3378986710";
#elif UNITY_IPHONE
        string adUnitID = ""ca-app-pub-8295605020027148/6332453116;
#endif
        //AdSize size = new AdSize(900, 300);
        bannerPause = new BannerView(adUnitID, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerPause.LoadAd(request);
        bannerPause.AdFailedToLoad += Banner_AdFailedToLoad;
        bannerPause.Hide();
    }

    private void Banner_AdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        // throw new System.NotImplementedException();
        Debug.Log("Fail to load banner :" + e.Message);
    }

    public void ShowTopBannerView()
    {
        if(banner != null)
        {
            banner.Show();
        }
    }


    public void HideTopBannerView()
    {
        if (banner != null)
        {
            banner.Hide();
        }
    }


    void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitID = androidUnitId;
#elif UNITY_IPHONE
        string adUnitID = iOSUnitId;
#endif
       // Debug.Log("Start Interstitial");
        intersititial = new InterstitialAd(adUnitID);
        AdRequest request = new AdRequest.Builder().Build();
        intersititial.AdClosed += Intersititial_AdClosed;
        intersititial.AdLoaded += Intersititial_AdLoaded;
        intersititial.AdFailedToLoad += Intersititial_AdFailedToLoad;
        intersititial.LoadAd(request);
    }

    private void Intersititial_AdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Load google ads failed :"+ e.Message);
    }

    private void Intersititial_AdLoaded(object sender, System.EventArgs e)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Google Ads Loaded!" + e.ToString());
    }

    private void Intersititial_AdClosed(object sender, System.EventArgs e)
    {
        //throw new System.NotImplementedException();
        if (intersititial != null)
        {
            intersititial.Destroy();
            LeanTween.dispatchEvent((int)Events.INTERSTITIALCLOSED);
        }

        RequestInterstitial();
    }

    public bool HasInterstital()
    {
        if (intersititial != null && intersititial.IsLoaded())
            return true;
        return false;
    }

    public void ShowInterstital()
    {
        if (HasInterstital())
            intersititial.Show();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
