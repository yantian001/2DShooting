using UnityEngine;
using System.Collections;

public class LevelScore
{
    /// <summary>
    /// 关卡ID
    /// </summary>
    public int LevelID { get; set; }
    /// <summary>
    /// 最佳纪录
    /// </summary>
    public int BestScore { get; set; }
    /// <summary>
    /// 游戏次数
    /// </summary>
    public int PlayCount { get; set; }

    ///// <summary>
    ///// 用户的排名
    ///// </summary>
   public int Rank { get; set; }
    ///// <summary>
    ///// 权重 , 改关卡分数在总分数所在的权重
    ///// </summary>
    //public double Weight { get; set; }

    private int _maxHits;
    /// <summary>
    /// 最大连击数
    /// </summary>
    public int MaxHits
    {
        get
        {
            return _maxHits;
        }
        set
        {
            if (value > MaxHits)
                _maxHits = value;
        }
    }
    /// <summary>
    /// 爆头总数
    /// </summary>
    public int HeadShotCount { get; set; }


    public LevelScore(int level )
    {
        LevelID = level;
        BestScore = 0;
        PlayCount = 0;
    }

    /// <summary>
    /// 更新成绩, 如果创造了新纪录,则返回true
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public bool SetScore(int score)
    {
        bool newScore = score > BestScore;
        if(newScore)
        {
            BestScore = score;
        }
        return newScore;
    }
}
