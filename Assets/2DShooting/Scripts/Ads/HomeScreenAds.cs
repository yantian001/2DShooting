using UnityEngine;
using System.Collections;

public class HomeScreenAds : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GoogleAdsUtil.Instance.ShowHomeBannerView();
    }

    void OnDisable()
    {
        GoogleAdsUtil.Instance.HideHomeBannerView();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
