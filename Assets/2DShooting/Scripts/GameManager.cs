using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{

    //游戏状态
    public enum GameStatu
    {
        OnTutorial,
        InGame,
        GamePaused,
        GameSuccessed,
        GameFailed
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

    void Init()
    {
        level = GameLogic.s_CurrentScene;
        gameDifficulty = GameLogic.s_CurrentDifficulty;
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
       
        Statu = GameStatu.InGame;
        //播放开始音效
        SoundManager.Instance.PlaySound(SoundManager.SoundType.GameStart);
        PlayerPrefs.DeleteAll();
        Player player = Player.CurrentPlayer;
       
    }

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
    }

    void InitGameData()
    {
        //if(gameData == null)
        string levelPath = string.Format("GameData/Level{0}-{1}", level, (int)gameDifficulty);
        gameData = Resources.Load<GameData>(levelPath);
        playerCurrentHP = gameData.playerHealth;
        if (gameData == null)
        {
            Debug.LogError("Init level gamedata error");
        }
        if ((gameData.gameType == GameData.GameType.Count) || (gameData.gameType == GameData.GameType.Time))
        {
            curMissionCount = gameData.missionCount;
        }
    }

    /// <summary>
    /// 产生怪物
    /// </summary>
    /// <param name="count">产生怪物的数量</param>
    public void SpawnedEnemy(int count = 1)
    {
        //if (gameData.gameType == GameData.GameType.Count)
        //{
        //    curMissionCount -= count;
        //    UIManager.Instance.UpdateMissionRemain((int)curMissionCount);
        //}
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
                if(curQurShildValue <= 0)
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
    }

    /// <summary>
    /// 检查游戏状态
    /// </summary>
    void CheckGameStatu()
    {
        if (isPlayerDie)
        {
            Statu = GameStatu.GameFailed;
            SoundManager.Instance.PlaySound(SoundManager.SoundType.GameSuccess);
            LeanTween.dispatchEvent((int)Events.GAMEFAILED, records);
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

        if(Statu == GameStatu.GameFailed || Statu == GameStatu.GameSuccessed)
        {
            Player.CurrentPlayer.AddPlayRecord(records);
        }
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

    public bool IsInGame()
    {
        return Statu == GameStatu.InGame;
    }



    /// <summary>
    /// 获得医疗包
    /// </summary>
    /// <param name="evt"></param>
    void OnGetMedKit(LTEvent evt)
    {
        if(evt.data != null)
        {
            float addedValue = 0.0f;
            if(float.TryParse(evt.data.ToString(),out addedValue))
            {
                playerCurrentHP += addedValue;
                UIManager.Instance.UpdatePlayerHUD(playerCurrentHP);
            }
            SoundManager.Instance.PlaySound(SoundManager.SoundType.GetLife);
        }
    }



    /// <summary>
    /// 获得盾牌道具
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetShield(LTEvent evt)
    {
       // throw new NotImplementedException();
       if(evt.data != null)
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
    }


    void Update()
    {
        if (Statu == GameStatu.InGame)
        {
            if (playerCurrentHP <= 0)
            {
                PlayerDead();
            }

            CheckGameStatu();
        }
        //更新连击
        UpdateCombo();
        //InvokeRepeating()
    }
    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.ITEMMEDKITHIT, OnGetMedKit);
        LeanTween.removeListener((int)Events.ITEMSHIELDHIT, OnGetShield);
    }

    #endregion


}
