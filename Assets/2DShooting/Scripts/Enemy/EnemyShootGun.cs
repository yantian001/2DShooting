using UnityEngine;
using System.Collections;

public class EnemyShootGun : EnemyShoot {

    public override bool Run()
    {
        UpdateActionStatus(true);
        InvokeRepeating("shoot",0f,0.1f);
        Invoke("broken", Random.Range(0.3f,0.6f));
        return true;
    }

    void shoot()
    {
        if (needTarget)
        {
            if (target == null || randomTarget)
            {
                target = GetTarget();
            }

            if (target == null)
                return ;
        }

        //播放动画
        PlayFireAnimation();
        PlayMuzzleEffect();
        ShowBullet();
        PlayFireAudio();
        PlayerInjure();

    }

    void broken()
    {
        CancelInvoke("shoot");
        UpdateActionStatus(false);
    }

    public override void EnemyDie()
    {
        base.EnemyDie();
        if(IsInvoking("broken"))
        {
            CancelInvoke("broken");
        }
        broken();
    }
}
