using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScene : MonoBehaviour {

    public int waitSconds = 3;

    /// <summary>
    /// 进度条
    /// </summary>
    [Tooltip("加载进度条")]
    public Slider progress;

    AsyncOperation async = null;
    int nowProcess;
    int totalProcess;
    float totalTime;
	// Use this for initialization
	void Start () {
        nowProcess = 0;
        totalTime = 0;
        StartCoroutine(LoadScene());
	}
	
    IEnumerator LoadScene()
    {
        async = Application.LoadLevelAsync(GameGlobalValue.s_CurrentScene);
        async.allowSceneActivation = false;
        yield return async;
    }

	// Update is called once per frame
	void Update () {
        totalTime += Time.deltaTime;
        if (async == null)
            return;
        if(async.progress < 0.9f)
        {
            totalProcess = (int)(async.progress * 100);
        }
        else
        {
            totalProcess = 100;
        }
        if(nowProcess <totalProcess)
        {
            nowProcess++;
        }
        if(progress )
        {
            progress.value = (float)nowProcess / 100;
        }
        if(totalTime>=waitSconds && totalProcess == 100)
        {
            async.allowSceneActivation = true;
        }
	}
}
