﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{



    #region 连杀UI
    public RectTransform Combo;
    public Text comboText;
    bool isComboShow = false;
    public void ShowCombo(int icombo, bool isHeadShot = false)
    {
        if (!isComboShow && icombo > 1)
        {
            LeanTween.move(Combo, new Vector3(30f, 25f, 0), 0.1f);
            isComboShow = true;
        }
        UpdateComboText(icombo);
        if (isShowComboStar)
        {
            ShowComboStar(icombo, isHeadShot);
        }
    }
    void UpdateComboText(int icombo)
    {
        RectTransform rect = comboText.GetComponent<RectTransform>();
        Vector3 loclScale = rect.localScale;
        LeanTween.scale(rect, loclScale * 1.5f, 0.05f).setOnComplete(() =>
        {
            comboText.text = icombo.ToString();
            LeanTween.scale(rect, loclScale, 0.05f);
        });
        comboText.text = icombo.ToString();
    }

    public bool isShowComboStar = true;

    public void HideCombo()
    {
        if (isComboShow)
        {
            LeanTween.move(Combo.GetComponent<RectTransform>(), new Vector3(-259f, 25f, 0), 0.1f);
            isComboShow = false;
        }
    }

    [Tooltip("连杀五角星")]
    public GameObject[] comboStars;
    [Tooltip("爆头五角星")]
    public GameObject headShotStar;
    [Tooltip("连杀五角星显示位置")]
    public RectTransform comboStarTransform;

    public void ShowComboStar(int combo, bool isHeadShot)
    {
        if ((!isHeadShot && combo <= 0) || comboStarTransform == null)
        {
            return;
        }
        combo = combo > 6 ? 6 : combo;
        GameObject objToCreate = null;
        if (isHeadShot)
        {
            objToCreate = headShotStar;
        }
        else
        {
            objToCreate = comboStars[combo - 1];
        }

        GameObject created = Instantiate(objToCreate);
        RectTransform createdRect = created.GetComponent<RectTransform>();
        if (createdRect != null)
        {
            //createdRect.anchoredPosition = comboStarTransform.anchoredPosition;
            //createdRect.SetParent(comboStarTransform.parent);
            CopyRectTransfrom(comboStarTransform, createdRect);
        }

    }

    #endregion

    #region 弹夹
    public Text bulletClipText;

    public void UpdateBulletDisplay(LTEvent evt)
    {
        if (bulletClipText != null && evt.data != null)
        {
            bulletClipText.text = evt.data.ToString();
        }
    }

    public void OnClipButtonClick()
    {
        LeanTween.dispatchEvent((int)Events.RELOAD);
    }
    #endregion

    #region 分数显示
    [Tooltip("连击分数预制")]
    public GameObject comboPoint;
    [Tooltip("爆头分数预制")]
    public GameObject headShotPoint;
    [Tooltip("显示分数的位置")]
    public RectTransform pointTransform;

    public void ShowPoint(int score, bool isHeadShot)
    {
        if (pointTransform == null)
        {
            return;
        }
        GameObject createPrefab = comboPoint;
        if (isHeadShot && headShotPoint)
        {
            createPrefab = headShotPoint;
        }
        if (!createPrefab)
            return;
        GameObject created = Instantiate(createPrefab);
        RectTransform createdRectTransform = created.GetComponent<RectTransform>();
        //createdRectTransform. = pointTransform;
        createdRectTransform.SetParent(pointTransform.parent);
        createdRectTransform.anchoredPosition = pointTransform.anchoredPosition;
        createdRectTransform.anchorMax = pointTransform.anchorMax;
        createdRectTransform.anchorMin = pointTransform.anchorMin;
        createdRectTransform.pivot = pointTransform.pivot;
        //createdRectTransform.
        //更改分数显示
        Text pointTxt = created.GetComponent<Text>();
        if (pointTxt)
        {
            pointTxt.text = string.Format(pointTxt.text, score);
        }

    }

    #endregion

    #region 单例模式
    public static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "UIManagerContainer";
                    _instance = obj.AddComponent<UIManager>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }

    }
    #endregion

    #region 任务显示
    /// <summary>
    /// 任务类型显示UI
    /// </summary>
    public GameObject[] MissionUIs;
    /// <summary>
    /// 任务剩余数量显示文本
    /// </summary>
    private Text missionRemainText;
    /// <summary>
    /// 任务类型
    /// </summary>
    private int gameType;
    /// <summary>
    /// 任务类型
    /// </summary>
    public int GameType
    {
        get
        {
            return gameType;
        }
        set
        {
            gameType = value;
            ChangeGameType(gameType);
        }
    }
    /// <summary>
    /// 更改任务类型
    /// </summary>
    /// <param name="gType">当前的任务类型</param>
    public void ChangeGameType(int gType)
    {
        if (MissionUIs != null && gType >= MissionUIs.Length)
            return;
        //显示当前任务类型的ui
        for(int i= 0; i<MissionUIs.Length;i++)
        {
            if (gType == i)
            {
                MissionUIs[i].SetActive(true);
                missionRemainText = MissionUIs[i].GetComponentInChildren<Text>();
            }
            else
                MissionUIs[i].SetActive(false);
        }
    }

    /// <summary>
    /// 更新任务数
    /// </summary>
    /// <param name="remain"></param>
    public void UpdateMissionRemain(int remain)
    {
        if(missionRemainText != null)
        {
            missionRemainText.text = remain.ToString();
        }
    }
    #endregion

    #region 玩家血量
    /// <summary>
    /// 玩家血条
    /// </summary>
    public Slider playerHUD;
    Text playerHUDText;
    /// <summary>
    /// 更新玩家血条显示
    /// </summary>
    /// <param name="current">当前血量</param>
    /// <param name="max">最大血量,最大血量 = 0时 ,不更新</param>
    public void UpdatePlayerHUD(float current,float max = 0)
    {
        if(playerHUD == null)
        {
            return;
        }
        if(max != 0)
        {
            playerHUD.maxValue = max;
        }
        if(current >= 0)
        {
            playerHUD.value = current;
        }

        playerHUDText = playerHUD.GetComponentInChildren<Text>();
        if(playerHUDText != null)
        {
            playerHUDText.text = current.ToString();
        }
    }

    /// <summary>
    /// 角色受伤效果
    /// </summary>
    [Tooltip("角色受伤效果")]
    public PlayerDamageAnim playerDamage;

    /// <summary>
    /// 显示角色受伤效果
    /// </summary>
    public void ShowPlayDamageEffect()
    {
        if (playerDamage)
        {
            playerDamage.damaged = true;
        }
    }
    #endregion

    #region 游戏结束
    public GameObject uiFinish;

    /// <summary>
    /// 游戏成功
    /// </summary>
    /// <param name="evt"></param>
    public void OnGameSuccess(LTEvent evt)
    {
        ShowFinishUI(true, evt.data as GameRecords);
    }

    public void OnGameFailed(LTEvent evt)
    {
        ShowFinishUI(false, evt.data as GameRecords);
    }

    void ShowFinishUI(bool success , GameRecords record)
    {
        if(uiFinish)
        {
            uiFinish.SetActive(true);
            RectTransform bgRect = uiFinish.GetComponent<RectTransform>().FindChild("Background").GetComponent<RectTransform>();
            if(bgRect)
            {
                Vector3 bgScale = bgRect.localScale;
                bgRect.localScale = Vector3.zero;
                LeanTween.scale(bgRect, bgScale, 0.2f);
            }
        }
    }

    #endregion

    public void OnEnable()
    {

    }

    public void Awake()
    {
        //添加子弹数量变化事件
        LeanTween.addListener(gameObject, (int)Events.BULLETCHANGED, UpdateBulletDisplay);
        Debug.Log("UIManager Inited");

        //监听游戏完成
        LeanTween.addListener(gameObject, (int)Events.GAMESUCCESS, OnGameSuccess);
        LeanTween.addListener(gameObject, (int)Events.GAMEFAILED, OnGameFailed);
    }

    public void OnDisable()
    {
        //移除事件
        LeanTween.removeListener((int)Events.BULLETCHANGED, UpdateBulletDisplay);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 拷贝UI对象的位置，以及父对象
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    void CopyRectTransfrom(RectTransform from, RectTransform to)
    {
        to.SetParent(from.parent);
        to.anchoredPosition = from.anchoredPosition;
        to.anchorMax = from.anchorMax;
        to.anchorMin = from.anchorMin;
        to.pivot = from.pivot;
    }
}