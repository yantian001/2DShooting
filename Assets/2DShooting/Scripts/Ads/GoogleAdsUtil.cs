using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class GoogleAdsUtil : MonoBehaviour {

    public string androidUnitId = "ca-app-pub-8295605020027148/3214523116";

    public string iOSUnitId = "ca-app-pub-8295605020027148/7644722719";

    public static GoogleAdsUtil Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GoogleAdsUtil>();
                if(_instance == null)
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

    public void Awake()
    {
        if(_instance == null )
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

    }


    void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitID = androidUnitId;
#elif UNITY_IPHONE
        string adUnitID = iOSUnitId;
#endif

        intersititial = new InterstitialAd(adUnitID);
        AdRequest request = new AdRequest.Builder().Build();
        intersititial.AdClosed += Intersititial_AdClosed;
        intersititial.AdLoaded += Intersititial_AdLoaded;
        intersititial.LoadAd(request);
    }

    private void Intersititial_AdLoaded(object sender, System.EventArgs e)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Google Ads Loaded!" + e.ToString());
    }

    private void Intersititial_AdClosed(object sender, System.EventArgs e)
    {
        //throw new System.NotImplementedException();
        if(intersititial != null)
        {
            intersititial.Destroy();
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
    void Update () {
	
	}
}
