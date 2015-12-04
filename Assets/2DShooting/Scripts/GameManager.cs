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

    //当前任务书
    public float curMissionCount = 0f;
    //连击有效时间
    public float comboInterval = 5.0f;

    private float curRemainComboInterval = 0.0f;
    //当前连击数
    private int currentCombo = 0;
    //最大连击数
    private int maxCombo = 0;

    private bool isCombo = false;

    void Init()
    {
        //初始化游戏数据
        InitGameData();
        //初始 emenyController
        InitEmenyController();
        Statu = GameStatu.InGame;
    }

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
        if(gameData == null)
        {
            Debug.LogError("Init level gamedata error");
        }
        if ((gameData.gameType == GameData.GameType.Count) || (gameData.gameType == GameData.GameType.Count))
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
        }
    }

    public void EmenyDead(bool headshoot = false)
    {
        isCombo = true;
        curRemainComboInterval = comboInterval;
        currentCombo += 1;
        if(headshoot)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.HeadShot);
        }
        else
        {
            SoundManager.Instance.PlayComboSound(currentCombo);
        }

        if(currentCombo > 1)
        {
            UIManager.Instance.ShowCombo(currentCombo);
        }
        //更新最大连击数
        maxCombo = maxCombo > currentCombo ? maxCombo : currentCombo;
        //SoundManager.Instance.PlaySound(SoundManager.SoundType.OneKill);
    }
	
	// Update is called once per frame
	void Update () {

        //更新连击
        UpdateCombo();
	}

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
}
