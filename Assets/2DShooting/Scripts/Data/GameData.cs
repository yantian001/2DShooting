using UnityEngine;
using System.Collections;

[SerializeField]
public class GameData : ScriptableObject
{

    //游戏类型
    public enum GameType
    {
        Count,
        Time,
        Infinity
    }
    //游戏类型
    public GameType gameType;
    /// <summary>
    /// 玩家血量
    /// </summary>
    [Tooltip("玩家血量")]
    public float playerHealth = 100f;
    /// <summary>
    /// 任务总数
    /// </summary>
    [Tooltip("任务总数")]
    public float missionCount;
    //敌人产生的间隔
    public float emenySpwanInterval = 1.0f;
    //敌人攻击间隔
    public float emenyShootInterval = 1.0f;
    //敌人伤害值
    public float emenyAttack = 2.0f;
    //是否随机参数伤害
    public bool useRondomAttack = false;
    //伤害随机值
    public float emenyAttackRandomVal = 1.0f;
    //敌人命中率
    [Range(1, 100)]
    public float emenyHitRatio = 60f;
    //敌人血量
    public float emenyHP = 1;
    //是否随机血量
    public bool useRandomHP = false;
    //血量随机值
    public float emenyHPRandomVal = 1;
    //同时存在的敌人数
    public int maxEnemyCount = 1;
    //同一地点最多的人数
    public int maxEnemyPerPosition = 1;
    /// <summary>
    /// 是否随时间增强
    /// </summary>
    public bool autoEnhance = true;
    /// <summary>
    /// 每次增强间隔时间
    /// </summary>
    public float enhanceAfterSeconds = 60f;
    /// <summary>
    /// 每次增强的数值
    /// </summary>
    [Range(0, 1)]
    public float valuePerEnhance = 0.05f;
    /// <summary>
    /// 已增强的次数
    /// </summary>
  public  int enhanceTime = 0;

    public void AutoEnhanment()
    {
        enhanceTime++;
        emenySpwanInterval *= (1 - valuePerEnhance);
        emenyShootInterval *= (1 - valuePerEnhance);
        emenyAttack *= (1 + valuePerEnhance);
        emenyAttackRandomVal *= (1 + valuePerEnhance);
        emenyHitRatio *= (1 + valuePerEnhance);
        emenyHP *= (1 + valuePerEnhance);
        emenyHPRandomVal *= (1 + valuePerEnhance);
        maxEnemyCount = maxEnemyCount  + enhanceTime / 2;
       
    }
}
