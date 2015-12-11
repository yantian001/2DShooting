using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SocialPlatforms;

public class Player {
    
    
    #region 属性

    /// <summary>
    /// 数据库ID
    /// </summary>
    public string ObjectID { get; set; }
    /// <summary>
    /// 用户的ID
    /// </summary>
    public string UserID { get; set; }
    /// <summary>
    /// 用户姓名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 各场景间成绩纪录
    /// </summary>
    public List<LevelScore> LevelScores { get; set; }

    #endregion
    

    #region 单例模式
    private static Player _crurrntPlayer = null;
    public static Player CurrentPlayer
    {
        get
        {
            if(_crurrntPlayer == null)
            {
                _crurrntPlayer = CreatePlayer();

                
            }

            return _crurrntPlayer;
        }
        private set
        {
            _crurrntPlayer = value;
        }
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <returns></returns>
   static Player CreatePlayer()
    {
        Player player = null;
        if(PlayerPrefs.HasKey("GameData"))
        {
            string jsonstr = PlayerPrefs.GetString("GameData");
            Debug.Log(jsonstr);
            player = JsonConvert.DeserializeObject<Player>(jsonstr);
        }
        if(player == null)
        {
            player = new Player();
            player.UserID = SystemInfo.deviceUniqueIdentifier;
            player.UserName = "Player" + player.UserID.Substring(0, 4);
            
            player.LevelScores.Add(new LevelScore(1) { LeardBoardID = "CgkImsCF9cIaEAIQAA" });
            player.Save2File();
        }


        return player;
    }

  public void Init()
    {
        SocialManager.Instance.Authenticate(ok=> {
            if(ok)
            {
                UserName = Social.localUser.userName;
                UserID = Social.localUser.id;
            }
           
        });
    }
    #endregion

    #region 构造函数
    public Player()
    {
        LevelScores = new List<LevelScore>();
    }
    #endregion

    #region 方法

    /// <summary>
    /// 增加游戏纪录
    /// </summary>
    /// <param name="level">游戏场景</param>
    /// <param name="score">分数</param>
    /// <param name="weight">权重</param>
    public void AddPlayRecord(GameRecords record)
    {
        if (record == null)
            return;
        LevelScore score = GetScoreByLevel(record.Level);
        if(score == null)
        { return; }

        score.PlayCount += 1;
        //score.Weight = record.Weight;
        score.MaxHits = record.MaxCombos;
        if(score.SetScore(record.Scores))
        {
            //重新计算总分
            // SaveAsync();
            SocialManager.Instance.ReportScore(score.BestScore, score.LeardBoardID);
        }

        Save2File();
    }

    void Sync2Server(Player player)
    {
        //this.SaveAsync();
    }

    /// <summary>
    /// 保存到文件，持久化
    /// </summary>
    void Save2File()
    {
        string jsonstr = JsonConvert.SerializeObject(this);
        PlayerPrefs.SetString("GameData", jsonstr);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 获得游戏场景的纪录
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    LevelScore GetScoreByLevel(int level)
    {
        LevelScore score =LevelScores.Find(p => { return p.LevelID == level; });
        if(score == null)
        {
            score = new LevelScore(level);
            score.LeardBoardID = GameGlobalValue.GetBoardIdByLevel(level);
            LevelScores.Add(score);
        }
        return score;
    }

    LevelScore GetScoreByBoardId(string boardId)
    {
        LevelScore score = LevelScores.Find(p => { return p.LeardBoardID == boardId; });
        if(score == null)
        {
            score = new LevelScore(GameGlobalValue.GetLevelIdByBoardId(boardId));
            score.LeardBoardID = boardId;
            LevelScores.Add(score);
        }
        return score;
    }

    public void UpdateRank(string boardid , IScore score)
    {
        LevelScore levelScore = GetScoreByBoardId(boardid);
        if(levelScore.BestScore <= score.value)
        {
            levelScore.BestScore = (int)score.value;
            levelScore.Rank = score.rank;
            Save2File();
        }
        else 
        {
            SocialManager.Instance.ReportScore(levelScore.BestScore, boardid);
        }

    }

    #endregion
}
