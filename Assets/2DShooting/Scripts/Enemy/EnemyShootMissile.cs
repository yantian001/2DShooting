using UnityEngine;
using System.Collections;

public class EnemyShootMissile : EnemyShoot {

    public override void Start()
    {
        base.Start();
    }

    public override GameObject GetTarget()
    {
        return null;
    }

    protected override void PlayerInjure()
    {
        
    }

    protected override void PlayFireAnimation()
    {
        SetParamValue();
    }

    protected override void PlayMuzzleEffect()
    {
        
    }

    protected override void PlayFireAudio()
    {
        base.PlayFireAudio();
    }

    protected override void ShowBullet()
    {
        if (bullet && firePlace)
        {
            GameObject createbullet = (GameObject)Instantiate(bullet, firePlace.position, Quaternion.identity);
            Handbomb bomb = createbullet.GetComponent<Handbomb>();
            if (bomb != null)
            {
                bomb.attack = attack;
            }
            createbullet.transform.localScale = firePlace.lossyScale;
            //createbullet.transform.SetParent(firePlace.parent);
        }
    }
}
