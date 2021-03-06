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
    /// <summary>
    /// 更新剩余连击时间显示
    /// </summary>
    /// <param name="value"></param>
    public void UpdateRemainComboTime(float value)
    {
        if (Combo)
        {
            Slider remainTime = Combo.GetComponentInChildren<Slider>();
            if (remainTime)
            {
                remainTime.value = value;
            }
        }
    }

    public bool isShowComboStar = true;

    public void HideCombo()
    {
        if (isComboShow)
        {
            LeanTween.move(Combo.GetComponent<RectTransform>(), new Vector3(-320f, 25f, 0), 0.1f);
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
    /// <summary>
    /// 是否显示分数
    /// </summary>
    [Tooltip("是否显示分数")]
    public bool isShowPoint = true;

    [Tooltip("连击分数预制")]
    public GameObject comboPoint;
    [Tooltip("爆头分数预制")]
    public GameObject headShotPoint;
    [Tooltip("显示分数的位置")]
    public RectTransform pointTransform;
    [Tooltip("分数显示")]
    public Text scoreText;

    public void ShowPoint(int score, bool isHeadShot)
    {
        if (!isShowPoint)
            return;
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

    /// <summary>
    /// 更新分数显示
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScoreText(int score)
    {
        if (scoreText)
        {
            scoreText.text = score.ToString();
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
    //public GameObject[] MissionUIs;
    /// <summary>
    /// 任务剩余数量显示文本
    /// </summary>
    public Text missionRemainText;
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
        //if (MissionUIs != null && gType >= MissionUIs.Length)
        //    return;
        ////显示当前任务类型的ui
        //for (int i = 0; i < MissionUIs.Length; i++)
        //{
        //    if (gType == i)
        //    {
        //        MissionUIs[i].SetActive(true);
        //        missionRemainText = MissionUIs[i].GetComponentInChildren<Text>();
        //    }
        //    else
        //        MissionUIs[i].SetActive(false);
        //}
    }

    /// <summary>
    /// 更新任务数
    /// </summary>
    /// <param name="remain"></param>
    public void UpdateMissionRemain(int remain)
    {
        if (missionRemainText != null)
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
    public void UpdatePlayerHUD(float current, float max = 0)
    {
        if (playerHUD == null)
        {
            return;
        }
        if (max != 0)
        {
            playerHUD.maxValue = max;
        }
        if (current >= 0)
        {
            playerHUD.value = current;
        }

        playerHUDText = playerHUD.GetComponentInChildren<Text>();
        if (playerHUDText != null)
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
        if (Random.Range(0, 10) == 0)
            LeanTween.dispatchEvent((int)Events.CREATEBLOOD);
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

    /// <summary>
    /// 显示结束UI
    /// </summary>
    /// <param name="success"></param>
    /// <param name="record"></param>
    void ShowFinishUI(bool success, GameRecords record)
    {
        if (uiFinish)
        {
            uiFinish.SetActive(true);
            RectTransform bgRect = uiFinish.GetComponent<RectTransform>().FindChild("Background").GetComponent<RectTransform>();
            if (bgRect)
            {
                Vector3 bgScale = bgRect.localScale;
                bgRect.localScale = Vector3.zero;
                LeanTween.scale(bgRect, bgScale, 0.2f);

                if (GameType != 2)
                {
                    //显示title
                    GameObject tltSucc = bgRect.FindChild("TitleSuccess").gameObject;
                    GameObject tltFail = bgRect.FindChild("TitleFailed").gameObject;
                    if (tltSucc)
                    {
                        tltSucc.SetActive(success);
                    }
                    if (tltFail)
                    {
                        tltFail.SetActive(!success);
                    }
                }
                //更新数据显示

                //杀敌数
                CommonUtils.SetChildText(bgRect, "Infos/Kills/TextCount", record.EnemyKills.ToString());
                //最大连击数
                CommonUtils.SetChildText(bgRect, "Infos/MaxHits/TextCount", record.MaxCombos.ToString());


                //爆头数
                CommonUtils.SetChildText(bgRect, "Infos/HeadShot/TextCount", record.HeadShotCount.ToString());

                //分数
                //CommonUtils.SetChildText(bgRect, "Infos/ScoreText", record.Scores.ToString());

                Text txtScore = bgRect.FindChild("Infos/ScoreText").GetComponent<Text>();
                if (txtScore)
                {
                    //txtScore.text = record.Scores.ToString();
                    StartCoroutine(DigitalDisplay(txtScore, record.Scores, 0, 1000));
                }

                Text txtMoneyEarn = bgRect.FindChild("Infos/MoenyEarn/TextCount").GetComponent<Text>();
                if (txtMoneyEarn)
                {
                    int moneyEarned = GameGlobalValue.GetMoneyFromRecord(record,success);
                    Player.CurrentPlayer.EarnMoney(moneyEarned);
                    StartCoroutine(DigitalDisplay(txtMoneyEarn, moneyEarned));
                }

                //重新开始按钮
                Button btnRestart = bgRect.FindChild("BtnRestart").GetComponent<Button>();
                if (btnRestart)
                {
                    btnRestart.onClick.AddListener(OnRestartClicked);
                }



                //回主页按钮
                Button btnMainMenu = bgRect.FindChild("BtnMainMenu").GetComponent<Button>();
                if (btnMainMenu)
                {
                    btnMainMenu.onClick.AddListener(OnMenuClicked);
                }

                Button btnNext = bgRect.FindChild("BtnNext").GetComponent<Button>();
                if(btnNext)
                {
                    btnNext.onClick.AddListener(OnBtnNextClicked);
                }

                if(record.gameType == global::GameType.Story)
                {
                    if(success)
                    {
                        CommonUtils.SetChildText(bgRect, "Title", "Level Success");
                        btnRestart.gameObject.SetActive(false);
                        btnNext.gameObject.SetActive(true);
                    }
                    else
                    {
                        CommonUtils.SetChildText(bgRect, "Title", "Level Failed");
                        btnRestart.gameObject.SetActive(true);
                        btnNext.gameObject.SetActive(false);
                    }
                }
                else
                {
                    btnRestart.gameObject.SetActive(true);
                    btnNext.gameObject.SetActive(false);
                }
            }

        }
    }

    void OnBtnNextClicked()
    {
        LeanTween.dispatchEvent((int)Events.GAMENEXT);
    }


    IEnumerator DigitalDisplay(Text txt, int to, int from = 0, int per = 100)
    {
        if (txt != null)
        {
            while (from < to)
            {
                if (to - from >= per)
                {
                    from += per;
                }
                else
                    from = to;
                txt.text = from.ToString();
                yield return null;
            }
        }
        // yield return null;
    }

    void OnRestartClicked()
    {
        LeanTween.dispatchEvent((int)Events.GAMERESTART);
    }

    void OnMenuClicked()
    {
        LeanTween.dispatchEvent((int)Events.MAINMENU);
    }
    //RectTransform FindChildByName()

    #endregion

    #region 盾牌

    public RectTransform itemShield;

    private bool isShieldShow = false;
    /// <summary>
    /// 显示盾牌
    /// </summary>
    /// 
    public void ShowShield()
    {
        if (isShieldShow)
            return;
        if (itemShield)
        {
            Vector3 to = itemShield.anchoredPosition3D + new Vector3(0, 450, 0);
            LeanTween.move(itemShield, to, .2f);
        }
        isShieldShow = true;
    }

    /// <summary>
    /// 添加弹孔
    /// </summary>
    public void UpdateShieldStatu()
    {
        if (itemShield)
        {
            int childCount = itemShield.childCount;
            int randomIndex = Random.Range(0, childCount - 1);
            int i = 0;
            while (i < childCount)
            {
                if (randomIndex >= childCount)
                {
                    randomIndex -= childCount;
                }
                var t = itemShield.GetChild(randomIndex);
                if (!t.gameObject.activeSelf)
                {
                    t.gameObject.SetActive(true);
                    break;
                }
                randomIndex++;
                i++;
            }
        }
    }

    public void ShakeShiled()
    {
        iTween.ShakePosition(itemShield.gameObject, new Vector3(0.2f, 0.2f, 0.2f),0.5f);
    }

    /// <summary>
    /// 隐藏盾牌
    /// </summary>
    public void HideShield()
    {
        if (!isShieldShow)
            return;
        if (itemShield)
        {
            Vector3 to = itemShield.anchoredPosition3D + new Vector3(0, -450, 0);
            LeanTween.move(itemShield, to, .2f).setOnComplete(() =>
            {
                for (int i = 0; i < itemShield.childCount; i++)
                {
                    itemShield.GetChild(i).gameObject.SetActive(false);
                }
            });
        }

        isShieldShow = false;
    }
    #endregion

    #region 武器Icon

    public RawImage weaponIcon;


    public void ChangeWeaponIcon(Texture2D texture)
    {
        if (weaponIcon != null && texture != null)
        {
            weaponIcon.texture = texture;
            // weaponIcon = Sprite.Create(texture, weaponIcon.textureRect, weaponIcon.pivot);
        }
    }

    #endregion

    #region 视频广告

    public GameObject _UIVedio;
    /// <summary>
    /// 是否有vedio的UI
    /// </summary>
    /// <returns></returns>
    public bool HasVedioUI()
    {
        if (_UIVedio != null)
            return true;
        return false;
    }

    /// <summary>
    /// 显示Vedio界面
    /// </summary>
    public void ShowVedioUI()
    {
        _UIVedio.SetActive(true);

        var watchVedioButton = _UIVedio.transform.FindChild("BtnWatchVideo").GetComponent<Button>();
        if (watchVedioButton)
        {
            watchVedioButton.onClick.AddListener(() =>
            {
                HideVedioUI();
                LeanTween.dispatchEvent((int)Events.WATCHVIDEOCLICKED);
            });
        }

    }

    /// <summary>
    /// 更新倒计时
    /// </summary>
    /// <param name="countdown"></param>
    public void UpdateVideoCountDownText(int countdown)
    {
        var countDownText = _UIVedio.transform.FindChild("TextCountdown").GetComponent<Text>();
        if (countDownText)
            countDownText.text = countdown.ToString();
    }

    /// <summary>
    /// 隐藏Video界面
    /// </summary>
    public void HideVedioUI()
    {
        _UIVedio.SetActive(false);
    }


    #endregion

    #region 倒计时

    public GameObject UICountDown;

    /// <summary>
    /// 显示倒计时界面
    /// </summary>
    public void ShowCountDown()
    {
        if (UICountDown != null)
        {
            UICountDown.SetActive(true);
        }
    }
    /// <summary>
    /// 更新倒计时显示
    /// </summary>
    /// <param name="value"></param>
    public void UpdateCountDownText(int value)
    {
        if (UICountDown && UICountDown.activeInHierarchy)
        {
            var txt = UICountDown.GetComponentInChildren<Text>();
            if (txt)
            {
                txt.text = value.ToString();
            }
        }
    }
    /// <summary>
    /// 隐藏倒计时
    /// </summary>
    public void HideCountDown()
    {
        if (UICountDown)
        {
            UICountDown.SetActive(false);
        }
    }

    #endregion

    #region Wave Count Down

    public Text waveCountDownText;

    public Text waveStartText;

    public void ShowWaveCountDown()
    {
        if (waveCountDownText != null)
        {
            waveCountDownText.gameObject.SetActive(true);
        }
    }

    public void UpdateWaveCountDownText(int wave, int time)
    {
        if (waveCountDownText != null)
        {
            waveCountDownText.text = string.Format("Wave {0} will start after {1} seconds", wave, time);
        }
    }

    public void HideWaveCountDown()
    {
        if (waveCountDownText != null)
        {
            waveCountDownText.gameObject.SetActive(false);
        }
    }

    public void ShowWaveStart(int wave)
    {
        if(waveStartText)
        {
            waveStartText.text = string.Format("Wave {0} Start !", wave);
            waveStartText.gameObject.SetActive(true);
        }
    }

    #endregion



    #region 暂停

    public GameObject uiPause;
    /// <summary>
    /// 显示暂停
    /// </summary>
    public void ShowPauseUI()
    {
        if (uiPause != null)
        {
            uiPause.SetActive(true);
        }

    }
    /// <summary>
    /// 隐藏暂停
    /// </summary>
    public void HidePauseUI()
    {
        if (uiPause != null)
        {
            uiPause.SetActive(false);
        }
    }

    #endregion

    #region Monobehavior

    public void OnEnable()
    {
        //LTBezierPath
    }

    public void Awake()
    {
        //添加子弹数量变化事件
        LeanTween.addListener(gameObject, (int)Events.BULLETCHANGED, UpdateBulletDisplay);

        //监听游戏完成
        LeanTween.addListener(gameObject, (int)Events.GAMESUCCESS, OnGameSuccess);
        LeanTween.addListener(gameObject, (int)Events.GAMEFAILED, OnGameFailed);

    }

    public void OnDisable()
    {
        //移除事件
        LeanTween.removeListener((int)Events.BULLETCHANGED, UpdateBulletDisplay);
        LeanTween.removeListener(gameObject, (int)Events.GAMESUCCESS, OnGameSuccess);
        LeanTween.removeListener(gameObject, (int)Events.GAMEFAILED, OnGameFailed);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

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
