using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGlobalValue
{

    //Score
    public static float hit2Score = .2f;
    public static float hit3Score = .3f;
    public static float hit4Score = .4f;
    public static float hit5Score = .5f;
    public static float hit6Score = .6f;
    public static float headShotScore = 1f;

    public static IDictionary<int, string> LevelBoardMap = new Dictionary<int, string>() { { 1, "CgkIkfn43vwGEAIQAQ" }, { 2, "CgkIkfn43vwGEAIQAg" }, { 3, "CgkIkfn43vwGEAIQAw" } };

    public static int GetLevelIdByBoardId(string id)
    {
        int levelid = 1;
        foreach (KeyValuePair<int, string> keyVal in LevelBoardMap)
        {
            if (keyVal.Value == id)
            {
                levelid = keyVal.Key;
                break;
            }

        }
        return levelid;
    }

    public static string GetBoardIdByLevel(int level)
    {
        string ret = "";
        if (LevelBoardMap.ContainsKey(level))
        {
            ret = LevelBoardMap[level];
        }
        return ret;
    }

    public static int GetHitScore(int combo, ref int score, bool headShot = false)
    {
        //爆头
        int addScore = 0;
        if (headShot)
        {
            //addScore = (int)(score * headShotScore) - score;
            addScore = (int)(score * headShotScore);

        }


        //两连杀
        if (combo == 2)
        {
            addScore += (int)(score * hit2Score);
        }
        else if (combo == 3)
        {
            addScore += (int)(score * hit3Score);
        }
        else if (combo == 4)
        {
            addScore += (int)(score * hit4Score);
        }
        else if (combo == 5)
        {
            addScore += (int)(score * hit5Score);
        }
        else if (combo >= 6)
        {
            addScore += (int)(score * hit6Score);
        }
        score += addScore;
        return addScore;
    }

    /// <summary>
    /// 当前场景
    /// </summary>
    public static int s_CurrentScene = 1;
    /// <summary>
    /// 当前关卡
    /// </summary>
    public static int s_CurrentLevel = 1;
    /// <summary>
    /// 没个场景的最大关数
    /// </summary>
    public static int maxLevelsPerScene = 20;

    public static bool IsLastLevel(int level)
    {
        if (level + 1 > maxLevelsPerScene)
            return true;
        return false;
    }
    /// <summary>
    /// 当前游戏难度
    /// </summary>
    public static GameDifficulty s_CurrentDifficulty = GameDifficulty.Normal;

    public static GameType s_CurrentGameType = GameType.Story;
    /// <summary>
    /// 当前武器ID
    /// </summary>
    public static int s_currentWeaponId = 0;


    #region 武器相关
    /// <summary>
    /// 最大攻击值
    /// </summary>
    public static float s_MaxWeaponAttack = 100;
    /// <summary>
    /// 每秒最大攻击次数
    /// </summary>
    public static float s_MaxFireRatePerSeconds = 20;
    /// <summary>
    /// 最大晃动距离
    /// </summary>
    public static float s_MaxShakeDistance = 1.0f;
    /// <summary>
    /// 最大的弹夹数量
    /// </summary>
    public static int s_MaxMagazineSize = 100;
    /// <summary>
    /// 最大引动速度
    /// </summary>
    public static int s_MaxMobility = 20;
    /// <summary>
    /// 最大得分奖励
    /// </summary>
    public static int s_MaxSocreBonus = 2;
    #endregion

    #region 钱
    /// <summary>
    /// 分数转换率
    /// </summary>
    public static float s_moneyRate = 0.01f;

    public static int s_MoneyPerLevel = 500;
    public static int GetMoneyFromRecord(GameRecords record, bool success = false)
    {
        int money = Mathf.CeilToInt(s_moneyRate * record.Scores);
        if (record.gameType == GameType.Story)
        {
            if (success)
            {
                if (Player.CurrentPlayer.GetSceneCurrentLevel(record.Level) > record.SubLevel)
                {
                    money += s_MoneyPerLevel / 10;
                }
                else
                {
                    money += s_MoneyPerLevel;
                }
            }
        }
        return money;
    }

    #endregion
}
