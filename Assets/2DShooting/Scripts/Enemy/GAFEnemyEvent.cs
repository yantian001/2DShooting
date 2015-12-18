using UnityEngine;
using System.Collections;

public class GAFEnemyEvent : MonoBehaviour {

    // Use this for initialization
  public  GAFEnemy enemy = null;

    void Start()
    {
        if(enemy == null)
        {
            enemy = GetComponentInParent<GAFEnemy>();
        }
    }
    /// <summary>
    /// 开始受伤动画
    /// </summary>
    void BeginInjuredAnimation()
    {
       if(enemy != null)
        {
            enemy.Injuring = true;
        }
    }
    /// <summary>
    /// 结束受伤动画
    /// </summary>
    void EndInjureAdnimation()
    {
        enemy.Injuring = false;
    }
}
