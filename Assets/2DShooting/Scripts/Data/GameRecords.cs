﻿using UnityEngine;
using System.Collections;

/// <summary>
/// 数据记录类
/// </summary>
public class GameRecords
{

    public int Level { get; private set; }
    public int LevelDiffcutly { get; private set; }

    //记录游戏数据
    private int _maxCombos = 0;
    /// <summary>
    /// 最大连击数
    /// </summary>
    public int MaxCombos
    {
        get { return _maxCombos; }
        set { if (value > _maxCombos) _maxCombos = value; }
    }
 

    private int _enemyKills = 0;
    /// <summary>
    /// 杀敌数
    /// </summary>
    public int EnemyKills
    {
        get { return _enemyKills; }
        set { _enemyKills = value; }
    }

    /// <summary>
    /// 游戏得分
    /// </summary>
    private int _scores = 0;
    public int Scores
    {
        get { return _scores; }
        set { _scores = value; }
    }
    /// <summary>
    /// 爆头数
    /// </summary>
    private int _headShotCount = 0;
    public int HeadShotCount
    {
        get
        {
            return _headShotCount;
        }
        set
        {
            _headShotCount = value;
        }
    }

    public GameRecords(int level ,int diffcutly)
    {
        Level = level;
        LevelDiffcutly = diffcutly;
    }
}

