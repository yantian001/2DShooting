using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

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

    public enum GameDifficulty
    {
        Normal =1,
        Hard,
        Infinity
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
            if(!_instance)
            {
                _instance = FindObjectOfType<GameManager>();
                if(!_instance)
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

    //游戏记录
    public GameRecords records = null;

    void Init()
    {
        //初始化游戏数据
        InitGameData();
        //初始 emenyController
        InitEmenyController();
        //初始化UI
        InitUI();
        if(records == null)
        {
            records = new GameRecords(level,(int)gameDifficulty);
        }
        Statu = GameStatu.InGame;
    }

    void InitUI()
    {
        //更新任务
        UIManager.Instance.GameType = (int)gameData.gameType;
        UIManager.Instance.UpdateMissionRemain((int)curMissionCount);

        //更新血量显示
        UIManager.Instance.UpdatePlayerHUD(playerCurrentHP, gameData.playerHealth);
    }

    /// <summary>
    /// 初始化 敌人控制器
    /// </summary>
    void InitEmenyController()
    {
        emenyController = FindObjectOfType<EmenyController>();
        if(emenyController == null)
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
        if(gameData == null)
        {
            Debug.LogError("Init level gamedata error");
        }
        if ((gameData.gameType == GameData.GameType.Count) || (gameData.gameType == GameData.GameType.Time))
        {
            curMissionCount = gameData.missionCount;
        }
    }
	void Start () {
       // Debug.Log("Start");
        Init();
	}

    
    public void SpawnedEnemy(int count = 1)
    {
        if(gameData.gameType == GameData.GameType.Count)
        {
            curMissionCount -= count;
            UIManager.Instance.UpdateMissionRemain((int)curMissionCount);
        }
    }

    /// <summary>
    /// 敌人死亡
    /// </summary>
    /// <param name="headshoot">是否爆头死亡</param>
    public void EmenyDead(bool headshoot = false)
    {
        records.EnemyKills += 1;
        if(headshoot)
        {
            records.HeadShotCount += 1;
        }
        isCombo = true;
        curRemainComboInterval = comboInterval;
        currentCombo += 1;
        SoundManager.Instance.PlayComboSound(currentCombo, headshoot);
        if (currentCombo > 0)
        {
            UIManager.Instance.ShowCombo(currentCombo,headshoot);
        }
        //更新最大连击数
        records.MaxCombos = currentCombo;
        //显示分数
       // UIManager.Instance.ShowPoint(3000, headshoot);
        //SoundManager.Instance.PlaySound(SoundManager.SoundType.OneKill);
    }
	
    public void PlayerInjured(float demage)
    {
        playerCurrentHP -= demage;
        UIManager.Instance.UpdatePlayerHUD(playerCurrentHP);
        UIManager.Instance.ShowPlayDamageEffect();
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    void PlayerDead()
    {
        isPlayerDie = true;
    }
	
	void Update () {
        if(Statu == GameStatu.InGame)
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
    /// <summary>
    /// 检查游戏状态
    /// </summary>
    void CheckGameStatu()
    {
        if(isPlayerDie)
        {
            Statu = GameStatu.GameFailed;
            LeanTween.dispatchEvent((int)Events.GAMEFAILED,records);
        }

        if(gameData.gameType == GameData.GameType.Count)
        {
            if(curMissionCount == 0 && records.EnemyKills == gameData.missionCount)
            {
                Statu = GameStatu.GameSuccessed;
                LeanTween.dispatchEvent((int)Events.GAMESUCCESS, records);
            }
        }
        else if(gameData.gameType == GameData.GameType.Time)
        {
            if(curMissionCount <= 0)
            {
                Statu = GameStatu.GameSuccessed;
                LeanTween.dispatchEvent((int)Events.GAMESUCCESS, records);
            }
        }
    }

    /// <summary>
    /// 更新连击
    /// </summary>
    void UpdateCombo()
    {
        if(isCombo)
        {
            curRemainComboInterval -= Time.deltaTime;
            if(curRemainComboInterval <= 0.0f)
            {
                isCombo = false;
                currentCombo = 0;
                UIManager.Instance.HideCombo();
            }
        }
    }

    public bool IsInGame()
    {
        return Statu == GameStatu.InGame;
    }
}
