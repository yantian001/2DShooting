using UnityEngine;
using System.Collections;

public class Shootable : MonoBehaviour {

    /// <summary>
    /// 血量
    /// </summary>
    public float _HP = 10f;

    /// <summary>
    /// 是否死亡
    /// </summary>
    [HideInInspector]
    public bool isDie = false;

    public void TakeDemage(float attack,bool keyHurt = false)
    {
        if(isDie)
        { return; }

        if(keyHurt)
        {
            _HP -= attack * 2;
        }
        else
        {
            _HP -= attack;
        }
        if(_HP <= 0)
        {
            isDie = true;
        }
        
    }
    
}
