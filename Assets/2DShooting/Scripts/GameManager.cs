using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

   
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

  
    void Init()
    {
        Statu = GameStatu.InGame;
    }

    void Awake()
    {
        Debug.Log("Awake");
    }
	void Start () {
        Debug.Log("Start");
        Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
