using UnityEngine;
using System.Collections;
using System;


public class GameManager : MonoBehaviour
{

    //游戏状态
    public enum GameStatu
    {
        OnTutorial,
        Init,
        WaitWaveStart,
        InGame,
        GamePaused,
        GameSuccessed,
        GameFailed,
        ShowContinuVedio
    }

    private GameStatu _statu;
    //游戏状态
    public GameStatu Statu
    {
        get { return _statu; }
        private set { _statu = value; }
    }

    //当前关卡
    public int level = 1;


    //游戏难度
    public GameDifficulty gameDifficulty = GameDifficulty.Normal;
    //游戏数据
    private GameData gameData;

    //单列模式
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<GameManager>();
                if (!_instance)
                {
                    GameObject gameManagerContainer = new GameObject();
                    gameManagerContainer.name = "GameManagerContainer";
                    _instance = gameManagerContainer.AddComponent<GameManager>();
                }

            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public JoystickControl controlModule = JoystickControl.Background;

    //EnemyController
    private EmenyController emenyController;

    //当前剩余任务数
    public float curMissionCount = 0f;


    //连击有效时间
    public float comboInterval = 5.0f;

    private float curRemainComboInterval = 0.0f;
    //当前连击数
    private int currentCombo = 0;
    //最大连击数
    //private int maxCombo = 0;

    private bool isCombo = false;

    /// <summary>
    /// 玩家当前血量
    /// </summary>
    float playerCurrentHP;
    bool isPlayerDie = false;
    /// <summary>
    /// 是否有护盾
    /// </summary>
    bool haveShield = false;

    /// <summary>
    /// 护盾值
    /// </summary>
    float shieldValue = 0.0f;

    /// <summary>
    /// 盾值的4分之一
    /// </summary>
    float qurShieldValue = 0.0f;
    
    float curQurShildValue = 0.0f;
    //游戏记录
    public GameRecords records = null;
    /// <summary>
    /// 武器集合
    /// </summary>
    public Weapon[] weaponList;

    private Weapon currentWeapon = null;

    private int currentWeaponId = 1;
    /// <summary>
    /// 是否显示过广告
    /// </summary>
    bool alreadyShowVedio = false;
    /// <summary>
    /// 视频广告等待时间
    /// </summary>
    [Range(1,10)]
    public int timeWaitVideo = 5;
    /// <summary>
    /// 视频广告奖励
    /// </summary>
    bool videoRewarded = false;

    Coroutine vedioCountDownCorotuine = null;

    int currentWave = 1;

    int currentTurns = 1;

    void Init()
    {
        Statu = GameStatu.Init;
        level = GameGlobalValue.s_CurrentScene;
        gameDifficulty = GameGlobalValue.s_CurrentDifficulty;
        currentWeaponId = GameGlobalValue.s_currentWeaponId;

        currentWave = 1;
        currentTurns = 1;
        if (records == null)
        {
            records = new GameRecords(level, (int)gameDifficulty);
        }
        //初始化游戏数据
        InitGameData();
        //初始 emenyController
        InitEmenyController();
        //初始化UI
        InitUI();
        //初始化武器
        InitWeapon();
        //播放开始音效
        SoundManager.Instance.PlaySound(SoundManager.SoundType.GameStart);
        //PlayerPrefs.DeleteAll();
        Player player = Player.CurrentPlayer;

    }

    /// <summary>
    /// 初始化武器
    /// </summary>
    void InitWeapon()
    {
        if (weaponList == null || weaponList.Length <= 0)
        {
            Debug.LogError("cant find weapon !");
            return;
        }
        currentWeapon = GetWeapon(currentWeaponId);

        if(currentWeapon == null)
        {
            Debug.LogError("Current weapon error!");
        }
        
        currentWeapon.gameObject.transform.parent.gameObject.SetActive( true);

        //设置相关属性
        if(controlModule == JoystickControl.Background)
        {
            var background = GameObject.FindGameObjectWithTag("Background");
            //设置移动速度
            if (background != null)
            {
                var joystickMovement = background.GetComponent<JoystickBackgroundMovment>();
                if (joystickMovement != null)
                {
                    joystickMovement.signTran = currentWeapon.signTransform;
                    if (currentWeapon.overrideMovement)
                    {
                        joystickMovement.smoothRatio = currentWeapon.moveSpeed;
                        joystickMovement.checkNearTarget = currentWeapon.checkNearTarget;
                        joystickMovement.nearSmoothRatio = currentWeapon.nearTargetMoveSpeed;
                    }
                }
            }
        }
        else if(controlModule == JoystickControl.Camera)
        {
            var joystickCamera = FindObjectOfType<JoystickCameraMovment>();
            if(joystickCamera != null )
            {
                joystickCamera.signTran = currentWeapon.signTransform;
                if (currentWeapon.overrideMovement)
                {
                    joystickCamera.smoothRatio = currentWeapon.moveSpeed;
                    joystickCamera.checkNearTarget = currentWeapon.checkNearTarget;
                    joystickCamera.nearSmoothRatio = currentWeapon.nearTargetMoveSpeed;
                }
            }
        }
        UIManager.Instance.ChangeWeaponIcon(currentWeapon.WeaponIcon);
    }

    Weapon GetWeapon(int weaponId)
    {
        Weapon result = null;
        for (int i = 0; i < weaponList.Length; i++)
        {
            if (weaponList[i].ID == weaponId)
            {
                result = weaponList[i];
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// 初始化UI
    /// </summary>
    void InitUI()
    {
        //更新任务
        UIManager.Instance.GameType = (int)gameData.gameType;
        UIManager.Instance.UpdateMissionRemain(records.EnemyKills);

        //更新血量显示
        UIManager.Instance.UpdatePlayerHUD(playerCurrentHP, gameData.playerHealth);
    }

    /// <summary>
    /// 初始化 敌人控制器
    /// </summary>
    void InitEmenyController()
    {
        emenyController = FindObjectOfType<EmenyController>();
        if (emenyController == null)
        {
            Debug.Log("Init EmenyController error!!");
        }
        emenyController.gameData = gameData;

        //emenyController.SetWave(gameData.waves[0]);
        SetWave();
    }

    void SetWave()
    {
        int waves = gameData.waves.Length;

        int waveIndex = (currentWave -1) % waves;

        currentTurns = currentWave / waves;

        emenyController.SetWave(gameData.waves[waveIndex]);
    }

    void InitGameData()
    {
        //if(gameData == null)
        string levelPath = string.Format("GameData/Level{0}-{1}", level, (int)gameDifficulty);
        gameData = Instantiate( Resources.Load<GameData>(levelPath));
        
        if (gameData == null)
        {
            Debug.LogError("Init level gamedata error");
        }
        playerCurrentHP = gameData.playerHealth;
        if ((gameData.gameType == GameData.GameType.Count) || (gameData.gameType == GameData.GameType.Time))
        {
            curMissionCount = gameData.missionCount;
        }

        if(gameData.autoEnhance)
        {
            StartCoroutine(GameEnhance());
        }
    }

    IEnumerator GameEnhance()
    {
        while(true)
        {
            yield return new WaitForSeconds(gameData.enhanceAfterSeconds);
            if(IsInGame())
                gameData.AutoEnhanment();
        }
       
    }
    
    /// <summary>
    /// 敌人死亡
    /// </summary>
    /// <param name="headshoot">是否爆头死亡</param>
    public void EmenyDead(int score, bool headshoot = false)
    {

        records.EnemyKills += 1;
        UIManager.Instance.UpdateMissionRemain(records.EnemyKills);

        if (headshoot)
        {
            records.HeadShotCount += 1;
        }
        isCombo = true;
        curRemainComboInterval = comboInterval;
        currentCombo += 1;
        SoundManager.Instance.PlayComboSound(currentCombo, headshoot);
        if (currentCombo > 0)
        {
            UIManager.Instance.ShowCombo(currentCombo, headshoot);
        }
        //更新最大连击数
        records.MaxCombos = currentCombo;
        int addScore = GameGlobalValue.GetHitScore(currentCombo, ref score, headshoot);
        if (headshoot)
            records.HeadshotAddScore += addScore;
        else
            records.HitAddAcores += addScore;

        //计算武器加成
        if(currentWeapon != null)
        {
            records.WeaponScoreBonus += (int)(currentWeapon.scoreBonus * score);
            score += (int)(currentWeapon.scoreBonus * score);
        }
        records.Scores += score;
        UIManager.Instance.UpdateScoreText(records.Scores);
        //显示分数
        UIManager.Instance.ShowPoint(score, headshoot);
        //SoundManager.Instance.PlaySound(SoundManager.SoundType.OneKill);
    }

    /// <summary>
    /// 玩家受到伤害
    /// </summary>
    /// <param name="demage">伤害值</param>
    public void PlayerInjured(float demage)
    {
        if (haveShield && shieldValue > 0)
        {
            if (shieldValue >= demage)
            {
                shieldValue -= demage;
                curQurShildValue -= demage;
                if (curQurShildValue <= 0)
                {
                    UIManager.Instance.UpdateShieldStatu();
                    curQurShildValue = qurShieldValue;
                }
            }
            else
            {
                shieldValue = 0;
                playerCurrentHP -= (demage - shieldValue);
                UIManager.Instance.UpdatePlayerHUD(playerCurrentHP);
                UIManager.Instance.ShowPlayDamageEffect();
            }
            if (shieldValue <= 0)
            {
                haveShield = false;
                UIManager.Instance.HideShield();
            }
        }
        else
        {
            playerCurrentHP -= demage;
            UIManager.Instance.UpdatePlayerHUD(playerCurrentHP);
            UIManager.Instance.ShowPlayDamageEffect();
        }
    }


    /// <summary>
    /// 玩家死亡
    /// </summary>
    void PlayerDead()
    {
        isPlayerDie = true;
        SoundManager.Instance.PlaySound(SoundManager.SoundType.PlayerDie);
    }

    /// <summary>
    /// 检查游戏状态
    /// </summary>
    void CheckGameStatu()
    {
        if (isPlayerDie)
        {
            if((!alreadyShowVedio)&& (ChartboostUtil.Instance.HasGameOverVideo()) && UIManager.Instance.HasVedioUI())
            {
                Statu = GameStatu.ShowContinuVedio;
                UIManager.Instance.ShowVedioUI();
                LeanTween.addListener((int)Events.WATCHVIDEOCLICKED, OnWatchVideoClicked);
               vedioCountDownCorotuine = StartCoroutine(VideoCountDown(timeWaitVideo));
            }
            else
            {
                GameFinish();
            }
           
        }

        if (gameData.gameType == GameData.GameType.Count)
        {
            if (curMissionCount <= 0 && records.EnemyKills >= gameData.missionCount)
            {
                Statu = GameStatu.GameSuccessed;
                SoundManager.Instance.PlaySound(SoundManager.SoundType.GameFailed);
                LeanTween.dispatchEvent((int)Events.GAMESUCCESS, records);
            }
        }
        else if (gameData.gameType == GameData.GameType.Time)
        {
            if (curMissionCount <= 0)
            {
                Statu = GameStatu.GameSuccessed;
                SoundManager.Instance.PlaySound(SoundManager.SoundType.GameFailed);
                LeanTween.dispatchEvent((int)Events.GAMESUCCESS, records);
            }
        }

        
    }

    /// <summary>
    /// 点击了观看按钮
    /// </summary>
    /// <param name="evt"></param>
    void OnWatchVideoClicked(LTEvent evt)
    {
        alreadyShowVedio = true;
        //停止倒计时
        //StopCoroutine(VideoCountDown(timeWaitVideo));
        //StopCoroutine();
        if(vedioCountDownCorotuine != null )
        {
            StopCoroutine(vedioCountDownCorotuine);
        }
        ChartboostUtil.Instance.ShowGameOverVideo();
        //移除监听
        LeanTween.removeListener((int)Events.WATCHVIDEOCLICKED, OnWatchVideoClicked);

        LeanTween.addListener((int)Events.VIDEOREWARD, OnVideoRewarded);
        LeanTween.addListener((int)Events.VIDEOCLOSED, OnVideoClosed);
    }

    /// <summary>
    /// 获得了奖励
    /// </summary>
    /// <param name="evt"></param>
    void OnVideoRewarded(LTEvent evt)
    {
        Debug.Log("Get Rewards!");
        videoRewarded = true;
    }

    void OnVideoClosed(LTEvent evt)
    {
        LeanTween.removeListener((int)Events.VIDEOREWARD, OnVideoRewarded);
        LeanTween.removeListener((int)Events.VIDEOCLOSED, OnVideoClosed);
        if(videoRewarded)
        {
            // GameContinue();
            GameReward();
        }
        else
        {
            GameFinish();
        }
    }


    void GameReward()
    {
        AddLife(50);
        videoRewarded = false;
        GameContinue();
    }

    void GameContinue()
    {
        StartCountDown();
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    void GameFinish()
    {
        
        Statu = GameStatu.GameFailed;
        SoundManager.Instance.PlaySound(SoundManager.SoundType.GameSuccess);
        LeanTween.dispatchEvent((int)Events.GAMEFAILED, records);
        if (Statu == GameStatu.GameFailed || Statu == GameStatu.GameSuccessed)
        {
            Player.CurrentPlayer.AddPlayRecord(records);
        }
        //显示广告
        if(GoogleAdsUtil.Instance.HasInterstital())
        {
            GoogleAdsUtil.Instance.ShowInterstital();
        }
       else if(ChartboostUtil.Instance.HasInterstitialOnDefault())
        {
            ChartboostUtil.Instance.ShowInterstitialOnDefault();
        }
    }

    /// <summary>
    /// 倒计时结束
    /// </summary>
    void VideoCountDownFinish()
    {
        UIManager.Instance.HideVedioUI();
        //移除监听
        LeanTween.removeListener((int)Events.WATCHVIDEOCLICKED, OnWatchVideoClicked);
        GameFinish();
    }
    /// <summary>
    /// 倒计时
    /// </summary>
    /// <param name="total"></param>
    /// <returns></returns>
    IEnumerator VideoCountDown(int total)
    {
        while(total >=0)
        {
            UIManager.Instance.UpdateVideoCountDownText(total);
            yield return new WaitForSeconds(1.0f);
            total--;
        }
        VideoCountDownFinish();
        vedioCountDownCorotuine = null;
    }

    /// <summary>
    /// 更新连击
    /// </summary>
    void UpdateCombo()
    {
        if (isCombo)
        {
            curRemainComboInterval -= Time.deltaTime;
            if (curRemainComboInterval <= 0.0f)
            {
                isCombo = false;
                currentCombo = 0;
                UIManager.Instance.HideCombo();
            }
            UIManager.Instance.UpdateRemainComboTime(curRemainComboInterval / comboInterval);
        }
    }

    /// <summary>
    /// 是否在游戏中
    /// </summary>
    /// <returns></returns>
    public bool IsInGame()
    {
        return Statu == GameStatu.InGame;
    }
    /// <summary>
    /// 游戏是否暂停
    /// </summary>
    /// <returns></returns>
    public bool IsGamePaused()
    {
        return Statu == GameStatu.GamePaused;
    }
    /// <summary>
    /// 是否游戏结束
    /// </summary>
    /// <returns></returns>
    public bool IsGameFinished()
    {
        return Statu == GameStatu.GameFailed || Statu == GameStatu.GameSuccessed;
    }


    /// <summary>
    /// 获得医疗包
    /// </summary>
    /// <param name="evt"></param>
    void OnGetMedKit(LTEvent evt)
    {
        if (evt.data != null)
        {
            float addedValue = 0.0f;
            if (float.TryParse(evt.data.ToString(), out addedValue))
            {
                AddLife(addedValue);
            }
            //SoundManager.Instance.PlaySound(SoundManager.SoundType.GetLife);
        }
    }
    /// <summary>
    /// 增加生命
    /// </summary>
    /// <param name="value"></param>
    void AddLife(float value)
    { 
        playerCurrentHP += value;
        UIManager.Instance.UpdatePlayerHUD(playerCurrentHP);
        SoundManager.Instance.PlaySound(SoundManager.SoundType.GetLife);
    }

    /// <summary>
    /// 获得盾牌道具
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetShield(LTEvent evt)
    {
        // throw new NotImplementedException();
        if (evt.data != null)
        {
            float addedVal = 0.0f;
            if (float.TryParse(evt.data.ToString(), out addedVal))
            {
                this.haveShield = true;
                shieldValue += addedVal;
                qurShieldValue = shieldValue / 4;
                curQurShildValue = qurShieldValue;
                UIManager.Instance.ShowShield();
            }
        }
    }


    void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        UIManager.Instance.ShowCountDown();
        int total = 3;
        while (total > 0)
        {
            UIManager.Instance.UpdateCountDownText(total);
            yield return new WaitForSeconds(1f);
            total--;
        }
        UIManager.Instance.HideCountDown();
        Statu = GameStatu.InGame;
        //Debug.Log(Statu);
    }

    #region Click Event
    /// <summary>
    /// 暂停按钮点击事件
    /// </summary>
    public void OnPauseClicked()
    {
        Statu = GameStatu.GamePaused;
        UIManager.Instance.ShowPauseUI();
        GoogleAdsUtil.Instance.ShowPauseBanner();
    }
    /// <summary>
    /// 继续按钮点击事件
    /// </summary>
    public void OnContinueClicked()
    {
        
        GoogleAdsUtil.Instance.HidePauseBanner();
        UIManager.Instance.HidePauseUI();
        GameContinue();
        
    }
    /// <summary>
    /// 重新开始点击事件
    /// </summary>
    public void OnRestartClicked()
    {

        GoogleAdsUtil.Instance.HidePauseBanner();
        UIManager.Instance.HidePauseUI();
        LeanTween.dispatchEvent((int)Events.GAMERESTART);
    }
    /// <summary>
    /// 退出点击事件
    /// </summary>
    public void OnBackClicked()
    {
        GoogleAdsUtil.Instance.HidePauseBanner();
        UIManager.Instance.HidePauseUI();
        LeanTween.dispatchEvent((int)Events.MAINMENU);
    }

    #endregion

    #region MonoBehaviour Method

    public void OnEnable()
    {
        //监听打破医疗包
        LeanTween.addListener((int)Events.ITEMMEDKITHIT, OnGetMedKit);
        //监听获得盾牌
        LeanTween.addListener((int)Events.ITEMSHIELDHIT, OnGetShield);
    }



    void Start()
    {
        // Debug.Log("Start");
        Init();
        GameContinue();
    }


    void Update()
    {
        if (Statu == GameStatu.InGame)
        {
            if (playerCurrentHP <= 0)
            {
                PlayerDead();
            }
            else
            {
                isPlayerDie = false;
            }

            CheckGameStatu();

            CheckWave();

        }
        //更新连击
        UpdateCombo();
        //InvokeRepeating()
    }

    /// <summary>
    /// 检查波数
    /// </summary>
    void CheckWave()
    {
        if (emenyController == null)
            return;
        if(emenyController.IsWaveCompleted())
        {
            OnWaveCompleted();
        }
    }

    void OnWaveCompleted()
    {
        Statu = GameStatu.WaitWaveStart;

        currentWave += 1;
        SetWave();

        GameContinue();
    }
    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.ITEMMEDKITHIT, OnGetMedKit);
        LeanTween.removeListener((int)Events.ITEMSHIELDHIT, OnGetShield);
    }

    #endregion


}
