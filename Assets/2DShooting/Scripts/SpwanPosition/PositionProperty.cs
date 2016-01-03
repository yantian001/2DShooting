using UnityEngine;
using System.Collections;

public class PositionProperty : MonoBehaviour {

    /// <summary>
    /// 位置的类型
    /// </summary>
    public EnemySpwanPosition positionType = EnemySpwanPosition.FixedPosition;
    /// <summary>
    /// 最大的可移动x
    /// </summary>
    public float maxMovementX = 5.3f;
    /// <summary>
    /// 最小的可移动x
    /// </summary>
    public float minMovementX = -5.3f;
    /// <summary>
    /// 允许横向漫游
    /// </summary>
    public bool allowWanderX = true;

    /// <summary>
    /// 获取随机位置
    /// </summary>
    /// <returns></returns>
    public float GetRandomX()
    {
        return Random.Range(minMovementX, maxMovementX);
    }

}
