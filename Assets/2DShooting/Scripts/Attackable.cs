using UnityEngine;
using System.Collections;

public class Attackable : MonoBehaviour {
    [Tooltip("攻击值")]
    public float attack;
    /// <summary>
    /// 获取攻击值
    /// </summary>
    /// <returns></returns>
    public float GetAttack()
    {
        return attack;
    }
}
