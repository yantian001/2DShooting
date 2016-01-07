using UnityEngine;
using System.Collections;

public class EnemyShootHandbomb : EnemyShoot {

    public override void Start()
    {
        base.Start();
        this.animParamName = "handbomb";
        this.animParamType = AnimatorParameterType.Trigger;
    }

    public override GameObject GetTarget()
    {
        return null;
    }

    protected override void PlayFireAnimation()
    {
        SetParamValue();
    }

    protected override void PlayerInjure()
    {
        
    }

    protected override void PlayMuzzleEffect()
    {
        
    }

    protected override void ShowBullet()
    {
        if(bullet && firePlace)
        {
            GameObject createbullet = (GameObject)Instantiate(bullet, firePlace.position, Quaternion.identity);
            Handbomb bomb = createbullet.GetComponent<Handbomb>();
            if(bomb != null)
            {
                bomb.attack = attack; 
            }
            createbullet.transform.localScale = firePlace.lossyScale;
            //createbullet.transform.SetParent(firePlace.parent);
        }
    }
}
