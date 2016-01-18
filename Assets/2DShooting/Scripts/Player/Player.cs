using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SocialPlatforms;

public class Player
{


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

    /// <summary>
    /// 角色武器
    /// </summary>
    public List< PlayerWeaponInfo> Weapons;
    /// <summary>
    /// 角色金钱数
    /// </summary>
    public int Money;
    /// <summary>
    /// 已装备的武器ID
    /// </summary>
    public int EquipedWeaponId = 0;

    #endregion


    #region 单例模式
    private static Player _crurrntPlayer = null;
    public static Player CurrentPlayer
    {
        get
        {
            if (_crurrntPlayer == null)
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
        if (PlayerPrefs.HasKey("GameData"))
        {
            string jsonstr = PlayerPrefs.GetString("GameData");
            Debug.Log(jsonstr);
            player = JsonConvert.DeserializeObject<Player>(jsonstr);

        }
        if (player == null)
        {
            player = new Player();
            player.UserID = SystemInfo.deviceUniqueIdentifier;
            player.UserName = "Player" + player.UserID.Substring(0, 4);
            player.LevelScores.Add(new LevelScore(1) { LeardBoardID = "CgkImsCF9cIaEAIQAA" });
            player.Save2File();
        }

        return player;
    }
    /// <summary>
    /// 登录
    /// </summary>
    public void Login()
    {
        Login(null);
    }
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="onComplete">登录完成后回调</param>
    public void Login(System.Action<bool> onComplete)
    {
        SocialManager.Instance.Authenticate(ok =>
        {
            if (ok)
            {
                UserName = Social.localUser.userName;
                UserID = Social.localUser.id;

                List<LevelScore> needReportScores = GetNeedReportScores();
                if (needReportScores != null && needReportScores.Count > 0)
                {
                    for (int i = 0; i < needReportScores.Count; i++)
                    {
                        LevelScore score = needReportScores[i];
                        this.ReportScore(score.LeardBoardID, score.BestScore, (b) =>
                        {
                            score.NeedReported = !b;
                        });
                    }
                }

                Save2File();
            }
            if (onComplete != null)
            {
                onComplete(ok);
            }
        });
    }
    #endregion

    #region 构造函数
    public Player()
    {
        LevelScores = new List<LevelScore>();
        Weapons = new List<PlayerWeaponInfo>();
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
        if (score == null)
        { return; }

        score.PlayCount += 1;
        score.MaxHits = record.MaxCombos;
        if (score.SetScore(record.Scores))
        {
            //重新计算总分
            this.ReportScore(score.LeardBoardID, score.BestScore, (ok) =>
            {
                score.NeedReported = !ok;
            });
        }
        Save2File();
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
    public LevelScore GetScoreByLevel(int level)
    {
        LevelScore score = LevelScores.Find(p => { return p.LevelID == level; });
        if (score == null)
        {
            score = new LevelScore(level);
            score.LeardBoardID = GameGlobalValue.GetBoardIdByLevel(level);
            LevelScores.Add(score);
        }
        return score;
    }

    public LevelScore GetScoreByBoardId(string boardId)
    {
        LevelScore score = LevelScores.Find(p => { return p.LeardBoardID == boardId; });
        if (score == null)
        {
            score = new LevelScore(GameGlobalValue.GetLevelIdByBoardId(boardId));
            score.LeardBoardID = boardId;
            LevelScores.Add(score);
        }
        return score;
    }

    /// <summary>
    /// 更新榜单排名
    /// </summary>
    /// <param name="boardid"></param>
    /// <param name="score"></param>
    public void UpdateRank(string boardid, IScore score)
    {
        LevelScore levelScore = GetScoreByBoardId(boardid);
        if (levelScore.BestScore <= score.value)
        {
            levelScore.BestScore = (int)score.value;
            levelScore.Rank = score.rank;
            Save2File();
        }
        else
        {
            this.ReportScore(boardid, levelScore.BestScore, (ok) =>
            {
                levelScore.NeedReported = !ok;
            });
        }

    }
    /// <summary>
    /// 获取所有需要上传的成绩
    /// 
    /// </summary>
    /// <returns></returns>
    public List<LevelScore> GetNeedReportScores()
    {
        List<LevelScore> result = null;
        if (this.LevelScores != null)
        {
            result = LevelScores.FindAll(p => { return p.NeedReported; });
        }
        return result;
    }

    /// <summary>
    /// 上传成绩到指定排行榜
    /// </summary>
    /// <param name="boardId">排行版ID</param>
    /// <param name="score">分数</param>
    /// <param name="onComplete">回调函数</param>
    public void ReportScore(string boardId, int score, System.Action<bool> onComplete = null)
    {
        SocialManager.Instance.ReportScore(score, boardId, onComplete);
    }



    #endregion

    #region Money

    public void UseMoney(int money)
    {
        Money -= money;
        LeanTween.dispatchEvent((int)Events.MONEYCHANGED, Money);
        Save2File();
    }

    public void EarnMoney(int money)
    {
        Money += money;
        LeanTween.dispatchEvent((int)Events.MONEYCHANGED, Money);
        Save2File();
    }

    #endregion

    #region Weapon
    /// <summary>
    /// 根据武器ID获取当前武器信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PlayerWeaponInfo GetWeaponInfoById(int id)
    {
        PlayerWeaponInfo rst = null;
        if (Weapons != null && Weapons.Count > 0)
        {
            for (int i = 0; i < Weapons.Count; i++)
            {
                if (Weapons[i].Id == id)
                {
                    rst = Weapons[i];
                    break;
                }
            }
        }
        return rst;
    }

    /// <summary>
    /// 解锁武器
    /// </summary>
    /// <param name="id"></param>
    public void UnlockWeapon(int id)
    {
        PlayerWeaponInfo pwi = GetWeaponInfoById(id);
        if (pwi == null)
        {
            pwi = new PlayerWeaponInfo() { IsUnlocked = true, Id = id, Level = 0 };
        }
        Weapons.Add(pwi);
        Save2File();
    }

    /// <summary>
    /// 武器购买
    /// </summary>
    /// <param name="id"></param>
    /// <param name="money"></param>
    /// <returns></returns>
    public bool BuyWeapon(int id ,int money)
    {
        if(Money >= money)
        {
            UseMoney(money);
            UnlockWeapon(id);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EquipWeapon(int id)
    {
        if(EquipedWeaponId != id)
        {
            EquipedWeaponId = id;
            Save2File();
        }

    }

    public void UpgradeWeapon(int id,int money)
    {
        //UseMoney(money);
        PlayerWeaponInfo pwi = GetWeaponInfoById(id);
        if(pwi != null && pwi.IsUnlocked)
        {
            pwi.Level += 1;
            UseMoney(money);
            Save2File();
        }
    }
    #endregion
}
