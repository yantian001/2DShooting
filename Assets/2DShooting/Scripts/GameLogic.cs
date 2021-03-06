﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class GameLogic : MonoBehaviour
{


    /// <summary>
    /// Loading界面
    /// </summary>
    public int s_LoadingSceneId = 2;
    public int s_MainMenuSceneId = 0;


    private static GameLogic _logic = null;

    public static GameLogic Instance
    {
        get
        {
            if (_logic == null)
            {
                _logic = FindObjectOfType<GameLogic>();
                if (_logic == null)
                {
                    GameObject logicContainer = new GameObject();
                    logicContainer.name = "GameLogicContainer";
                    _logic = logicContainer.AddComponent<GameLogic>();
                }
            }
            return _logic;
        }
    }

    void Awake()
    {
        if (_logic == null)
        {
            _logic = this;
            DontDestroyOnLoad(gameObject);

            LeanTween.addListener((int)Events.GAMERESTART, OnGameRestart);
            LeanTween.addListener((int)Events.MAINMENU, OnGameMainMenu);
            LeanTween.addListener((int)Events.BACKTOSTART, BackToStart);
            LeanTween.addListener((int)Events.GAMENEXT, OnGameNext);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnGameRestart(LTEvent evt)
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnGameNext(LTEvent evt)
    {
        if (GameGlobalValue.IsLastLevel(GameGlobalValue.s_CurrentLevel))
            OnGameMainMenu(evt);
        else
        {
            GameGlobalValue.s_CurrentLevel += 1;
            Application.LoadLevel(Application.loadedLevel);
        }
    }
    

    void OnGameMainMenu(LTEvent evt)
    {

        GameGlobalValue.s_CurrentScene = s_MainMenuSceneId;
        //bool isShowLoading = true;
        //if(evt.data != null)
        //{
        //    bool.TryParse(evt.data.ToString(), out isShowLoading);
        //}
        Loading(false);
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
        //LeanTween.removeListener((int)Events.MAINMENU, OnGameMainMenu);
        //LeanTween.removeListener((int)Events.BACKTOSTART, BackToStart);
        //LeanTween.removeListener((int)Events.GAMENEXT, OnGameNext);
    }

    public void Loading(bool showLoading = true)
    {
        if (showLoading)
            Application.LoadLevel(s_LoadingSceneId);
        else
            Application.LoadLevel(GameGlobalValue.s_CurrentScene);
    }


    public void BackToStart(LTEvent evt)
    {
        GameGlobalValue.s_CurrentScene = 0;
        Loading(false);
    }

    void Update()
    {
        int levelIndex = Application.loadedLevel;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (levelIndex == 0)
            {
                if (ChartboostUtil.Instance.HasQuitInterstitial())
                {
                    ChartboostUtil.Instance.ShowQuitInterstitial();
                    LeanTween.addListener((int)Events.INTERSTITIALCLOSED, OnInterstitialClosed);
                }
                else if (GoogleAdsUtil.Instance.HasInterstital())
                {
                    GoogleAdsUtil.Instance.ShowInterstital();
                    LeanTween.addListener((int)Events.INTERSTITIALCLOSED, OnInterstitialClosed);
                }
                else
                {
                    Application.Quit();
                }

            }
            else if (levelIndex == s_MainMenuSceneId)
            {
                BackToStart(null);
            }
            else if (levelIndex == s_LoadingSceneId)
            {

            }
            else
            {
                if (GameManager.Instance.IsInGame())
                    GameManager.Instance.OnPauseClicked();
                else if (GameManager.Instance.IsGamePaused())
                {
                    GameManager.Instance.OnContinueClicked();
                }
                else if (GameManager.Instance.IsGameFinished())
                {
                    OnGameMainMenu(null);
                }
            }
        }
    }

    public void OnInterstitialClosed(LTEvent evt)
    {
        Application.Quit();
    }

}
