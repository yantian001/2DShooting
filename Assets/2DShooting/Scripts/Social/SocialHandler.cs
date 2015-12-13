using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SocialHandler : MonoBehaviour {

    static SocialHandler _instance = null;

    /// <summary>
    /// 启动时 自动刷新 
    /// </summary>
    public bool refreshOnStart = false;
    /// <summary>
    /// 自动登录
    /// </summary>
    public bool loginOnStart = false;

    [Tooltip("是否自动刷新")]
    public bool AutoRefresh = false;
    /// <summary>
    /// 刷新间隔
    /// </summary>
    [Tooltip("刷新间隔")]
    public float AutoRefreshRate = 300f;
    /// <summary>
    /// 是否加载完成
    /// </summary>
    [HideInInspector]
    public bool IsLoadFinish = false;

    private bool isLoading = false;

    public static SocialHandler Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SocialHandler>();
                if(_instance == null)
                {
                    GameObject socialHandler = new GameObject();
                    socialHandler.name = "SocialHandlerContainer";
                    _instance = socialHandler.AddComponent<SocialHandler>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }

    }

	// Use this for initialization
	void Start () {
        
        if(loginOnStart)
        {
            Player.CurrentPlayer.Login();
        }
	    if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            if(refreshOnStart)
            {
                Sync();
            }

            if(AutoRefresh)
            {
                StartCoroutine(AutoSync());
            }
        }
        else
        {
            IsLoadFinish = true;
        }
        
	}

    IEnumerator AutoSync()
    {
        while(true)
        {
            yield return new WaitForSeconds(AutoRefreshRate);
            Sync();
        }
    }

    void Sync()
    {
        SocialManager.Instance.SyncRank();
        IsLoadFinish = false;
        isLoading = true;
    }
	
	// Update is called once per frame
	void Update () {
	    if(isLoading)
        {
            if(SocialManager.Instance.CheckUpdated())
            {
                isLoading = false;
                IsLoadFinish = true;
            }

        }
	}
}
