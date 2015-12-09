using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{

    /// <summary>
    /// 当前场景
    /// </summary>
    public static int s_CurrentScene = 1;
    /// <summary>
    /// 当前游戏难度
    /// </summary>
    public static GameDifficulty s_CurrentDifficulty = GameDifficulty.Normal;
    /// <summary>
    /// Loading界面
    /// </summary>
    public static int s_LoadingSceneId = 2;

    public static int s_MainMenuSceneId = 0;
    void Awake()
    {
        LeanTween.addListener((int)Events.GAMERESTART, OnGameRestart);
        LeanTween.addListener((int)Events.MAINMENU, OnGameMainMenu);
    }

    void OnGameRestart(LTEvent evt)
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnGameMainMenu(LTEvent evt)
    {
        s_CurrentScene = s_MainMenuSceneId;
        Loading();
    }

    // Update is called once per frame
    public void OnDestroy()
    {
       // Debug.Log("OnDestroy");
    }

    public void OnDisable()
    {
        // Debug.Log("OnDisable");
        LeanTween.removeListener((int)Events.GAMERESTART, OnGameRestart);
    }

    public static void Loading()
    {
        Application.LoadLevel(s_LoadingSceneId);
    }
}
