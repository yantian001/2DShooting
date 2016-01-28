using UnityEngine;
using System.Collections;

public class EenmyWanderMoveShoot : EnemyWanderMoveX {
    public float attactPerTime = 0.1f;

    public float shootRate = 0.1f;

    /// <summary>
    /// 可攻击的目标
    /// </summary>
    public GameObject[] targets;
    /// <summary>
    /// 运行时获取可攻击对象
    /// </summary>
    public bool getAllTargets = true;
    /// <summary>
    /// 攻击对象的标签
    /// </summary>
    public string targetTag = "Player";
    /// <summary>
    /// 攻击目标
    /// </summary>
    public GameObject target;
    /// <summary>
    /// 随机目标
    /// </summary>
    public bool randomTarget = true;

    /// <summary>
    /// 枪口位置
    /// </summary>
    public Transform firePlace;

    /// <summary>
    /// 枪口特效
    /// </summary>
    public GameObject muzzleEffect;
    /// <summary>
    /// 子弹
    /// </summary>
    public GameObject bullet;
    /// <summary>
    /// 开枪音效
    /// </summary>
    public AudioClip fireAudio;
    /// <summary>
    /// 需要目标
    /// </summary>
    public bool needTarget = true;

    public override void OnMoveStart()
    {
        base.OnMoveStart();
        InvokeRepeating("shoot", 0, shootRate);
    }

    public override void OnMoveComplete()
    {
        base.OnMoveComplete();
        CancelInvoke("shoot");
    }

   public void shoot()
    {
        if (needTarget)
        {
            if (target == null || randomTarget)
            {
                target = GetTarget();
            }

            if (target == null)
                return;
        }

        //播放动画
        PlayFireAnimation();
        PlayMuzzleEffect();
        ShowBullet();
        PlayFireAudio();
        PlayerInjure();

    }

    /// <summary>
    /// 播放开火动画
    /// </summary>
    protected virtual void PlayFireAnimation()
    {

    }

    /// <summary>
    /// 播放枪口动画
    /// </summary>
    protected virtual void PlayMuzzleEffect()
    {
        if (muzzleEffect && firePlace)
        {
            GameObject muzzle = Instantiate(muzzleEffect);
            muzzle.transform.position = firePlace.position;
            muzzle.transform.localScale = transform.lossyScale;
            muzzle.transform.parent = firePlace;
        }
    }

    /// <summary>
    /// 显示子弹
    /// </summary>
    protected virtual void ShowBullet()
    {
        Vector3 to = (firePlace.transform.position - target.transform.position).normalized;
        GameObject blt = (GameObject)Instantiate(bullet, firePlace.transform.position, Quaternion.FromToRotation(Vector3.right, to));
        blt.transform.parent = firePlace;
        Vector3 scale = blt.transform.localScale;
        blt.transform.localScale = Vector3.zero;
        iTween.MoveTo(blt, iTween.Hash("position", target.transform.position, "time", 1, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", blt));
        iTween.ScaleTo(blt, scale, 0.2f);
    }

    // 子弹自动销毁
    void OnBulletMoveComplete(System.Object obj)
    {
        Destroy((GameObject)obj);
    }

    /// <summary>
    /// 播放开枪音效
    /// </summary>
    protected virtual void PlayFireAudio()
    {
        if (fireAudio != null)
        {
            //iTween.Stab(gameObject, fireAudio, 0f);
            SoundManager.PlayAduioITween(gameObject, fireAudio,0.2f);
        }
    }
    /// <summary>
    /// 玩家伤害
    /// </summary>
    protected virtual void PlayerInjure()
    {
        GameManager.Instance.PlayerInjured(attactPerTime);
    }

    public virtual GameObject GetTarget()
    {
        GameObject rst = null;

        if(targets == null || targets.Length <= 0)
        {
            targets = GameObject.FindGameObjectsWithTag(targetTag);
        }

        if (targets != null && targets.Length > 0)
        {
            int totalWeight = 0;
            int i = 0;
            while (i < targets.Length)
            {
                Weight w = targets[i].GetComponent<Weight>();
                if (w != null)
                {
                    totalWeight += w._Weight;
                }
                i++;
            }

            int randomWeight = Random.Range(0, totalWeight + 1);
            i = 0;
            while (randomWeight > 0)
            {
                Weight w = targets[i].GetComponent<Weight>();
                if (w != null)
                {
                    randomWeight -= w._Weight;
                }
                if (randomWeight <= 0)
                {
                    break;
                }
                i++;
                if (i == targets.Length)
                    i = 0;
            }
            rst = targets[i];
        }

        return rst;
    }
    public override void EnemyDie()
    {
        base.EnemyDie();
        LeanTween.cancel(gameObject);
        OnMoveComplete();
    }
}
