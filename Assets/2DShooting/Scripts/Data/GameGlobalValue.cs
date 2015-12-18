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

    public static IDictionary<int, string> LevelBoardMap = new Dictionary<int, string>() { { 1, "CgkImsCF9cIaEAIQAA" } };

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
        if(LevelBoardMap.ContainsKey(level))
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
    /// 当前游戏难度
    /// </summary>
    public static GameDifficulty s_CurrentDifficulty = GameDifficulty.Normal;
    /// <summary>
    /// 当前武器ID
    /// </summary>
    public static int s_currentWeaponId = 0;
}
