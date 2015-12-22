using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class SocialManager
{

    public static int rankLimit = 25;

    private string rankFileName = "rank.json";
    #region variables
    public Dictionary<string, List<SocialObject>> SocialObjects;
    /// <summary>
    /// 只更新用户信息
    /// </summary>
    public bool onlyUpdatePlayer = false;

    public bool updated = false;

    private bool updating = false;

    public int updateUserCount = 0;
    public int updateRankCount = 0;

    public int updatedUserCount = 0, updatedRankCount = 0;
    #endregion

    #region 单列
    static SocialManager _instance = null;

    public static SocialManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = CreateInstance();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }


    #endregion

    #region 构造函数

    private SocialManager()
    {
        SocialObjects = new Dictionary<string, List<SocialObject>>();
    }

    public bool CheckUpdated()
    {
        if (updating)
        {
            if (onlyUpdatePlayer)
            {
                return updatedUserCount >= updateUserCount;
            }
            else
                return (updatedUserCount >= updateUserCount && updatedRankCount >= updateRankCount);
        }
        return true;
    }

    void CheckUpdateStatu()
    {
        if (onlyUpdatePlayer)
        {
            if (updatedUserCount >= updateUserCount)
            {
                updated = true;
                //Player.CurrentPlayer.s
            }
        }
        else
        {
            if (updatedUserCount >= updateUserCount && updatedRankCount >= updateRankCount)
            {
                updated = true;
                SaveRank2Local();
                LeanTween.dispatchEvent((int)Events.LEARDBOARDUPDATED);
            }
        }
    }

    /// <summary>
    /// 保存到本地
    /// </summary>
    void SaveRank2Local()
    {
        // Ranks
        string jsonstr = JsonConvert.SerializeObject(SocialObjects);
        Debug.Log("Save rankinfo to disk" + jsonstr);
        FileUtils.Save2File(rankFileName, jsonstr);
    }

    public void SyncRank()
    {
        if (!Social.localUser.authenticated)
            return;
        GetRankFromService();
    }

    /// <summary>
    /// 从service里获取排名信息
    /// </summary>
    void GetRankFromService()
    {
        updating = true;
        foreach (KeyValuePair<int, string> keyVal in GameGlobalValue.LevelBoardMap)
        {
            updateUserCount += 1;
            string boardId = keyVal.Value;
            //更新用户信息
            this.GetMyScore(boardId, (ok, s) =>
            {
                if (ok)
                {
                    Player.CurrentPlayer.UpdateRank(boardId, s);
                    updatedUserCount += 1;
                    CheckUpdateStatu();
                }
            });

            if (!onlyUpdatePlayer)
            {
                updateRankCount += 1;
                //更新排行榜
                this.GetTopByByLeaderboardID(boardId, new Range(1, 25), (ok, scores) =>
                {
                    Debug.Log(ok);
                    if (ok)
                    {
                        List<SocialObject> objs = new List<SocialObject>();
                        List<string> UserIds = new List<string>();
                        foreach (IScore s in scores)
                        {
                            if(s != null)
                                UserIds.Add(s.userID);
                        }
                        Social.LoadUsers(UserIds.ToArray(), users =>
                        {
                            foreach (IScore s in scores)
                            {
                                SocialObject obj = new SocialObject(s);
                                IUserProfile user = FindUser(users, s.userID);
                                if(user != null)
                                {
                                    obj.UserName = FindUser(users, s.userID).userName;
                                }
                                objs.Add(obj);
                            }
                            if (SocialObjects.ContainsKey(boardId))
                            {
                                SocialObjects[boardId] = objs;
                            }
                            else
                            {
                                SocialObjects.Add(boardId, objs);
                            }
                            updatedRankCount += 1;
                            CheckUpdateStatu();
                        });
                    }
                });
            }
        }
    }


    

    /// <summary>
    /// 从本地获取排名信息
    /// </summary>
    void GetRankFromLocal()
    {
        string str = FileUtils.ReadFile(rankFileName);
        if (!string.IsNullOrEmpty(str))
        {
            Dictionary<string, List<SocialObject>> localObjects = JsonConvert.DeserializeObject<Dictionary<string, List<SocialObject>>>(str);
            foreach (KeyValuePair<int, string> keyVal in GameGlobalValue.LevelBoardMap)
            {
                if (localObjects.ContainsKey(keyVal.Value))
                {
                    SocialObjects.Add(keyVal.Value, localObjects[keyVal.Value]);
                }
                else
                {
                    SocialObjects.Add(keyVal.Value, null);
                }
            }
        }

    }
    /// <summary>
    /// 通过用户ID查找用户
    /// </summary>
    /// <param name="users"></param>
    /// <param name="userid"></param>
    /// <returns></returns>
    IUserProfile FindUser(IUserProfile[] users, string userid)
    {
        IUserProfile rst = null;
        foreach (IUserProfile user in users)
        {
            if (user.id == userid)
            {
                rst = user;
                break;
            }
        }
        return rst;
    }

    /// <summary>
    /// 显示排行榜
    /// </summary>
    public void ShowLeardBoardUI()
    {
        Social.ShowLeaderboardUI();
    }

    static SocialManager CreateInstance()
    {
        SocialManager manger = new SocialManager();
        manger.Init();
        return manger;
    }

    void Awake()
    {
       Init();
    }

    void Init()
    {
#if UNITY_ANDROID
        GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = false;
        GooglePlayGames.PlayGamesPlatform.Activate();
#endif
#if UNITY_IPHONE
    // by defaults when player gets achievement nothing happens, so we call this function to show standard iOS popup when achievement is completed by user
    UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
        //从本地获取排名信息
        GetRankFromLocal();
    }
    #endregion

    #region functions
    /// <summary>
    /// 登陆
    /// </summary>
    public void Authenticate(System.Action<bool> onAuthComplete)
    {
        //if(!Application.)
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(onAuthComplete);
        }
        else
        {
            onAuthComplete(true);
        }
    }

    public bool ISAuthenticated()
    {
        return Social.localUser.authenticated;
    }

    /// <summary>
    /// 获得我的排名
    /// </summary>
    /// <param name="id"></param>
    /// <param name="onComplete"></param>
    public void GetMyScore(string id, System.Action<bool, IScore> onComplete)
    {
        if (!Social.localUser.authenticated)
        {
            return;
        }
        ILeaderboard lb = Social.CreateLeaderboard();
        lb.id = id;
        lb.SetUserFilter(new string[] { Social.localUser.id });
        lb.LoadScores(b =>
        {
            onComplete(b, lb.localUserScore);
        });
    }

    /// <summary>
    /// 提交分数
    /// </summary>
    /// <param name="score"></param>
    /// <param name="id"></param>
    /// <param name="onComplete"></param>
    public void ReportScore(long score, string id, System.Action<bool> onComplete = null)
    {
        if(!CommonUtils.IsNetworkOk() || !Social.localUser.authenticated)
        {
            if(onComplete != null)
            {
                onComplete(false);
            }
        }
        else
        {
            Debug.Log("Report to:" + id + " with score :" + score.ToString());
            Social.ReportScore(score, id, onComplete);
        }
        
    }

    /// <summary>
    /// 获取排行榜的排名
    /// </summary>
    /// <param name="id">排行榜ID</param>
    /// <param name="range">区间</param>
    /// <param name="onComplete">完成回调</param>
    /// <param name="scope">时间区间</param>
    /// <param name="userScope">用户区间</param>
    public void GetTopByByLeaderboardID(string id, Range range, System.Action<bool, IScore[]> onComplete, TimeScope scope = TimeScope.AllTime, UserScope userScope = UserScope.Global)
    {
        if (!Social.localUser.authenticated)
            return;
        ILeaderboard lb = Social.CreateLeaderboard();
        Debug.Log(lb);
        if (lb == null)
            return;
        lb.id = id;
        lb.range = range;
        lb.timeScope = scope;
        lb.userScope = userScope;
        lb.LoadScores(ok =>
       {
           onComplete(ok, lb.scores);
       });
    }


    /// <summary>
    /// 获取相应排行版的信息
    /// </summary>
    /// <param name="leardboardId"></param>
    /// <returns></returns>
    public List<SocialObject> GetObjectsById(string leardboardId)
    {
        if (SocialObjects.ContainsKey(leardboardId))
        {
            return SocialObjects[leardboardId];
        }
        return null;
    }

    #endregion




}
