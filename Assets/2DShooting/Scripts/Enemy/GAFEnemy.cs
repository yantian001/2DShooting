using UnityEngine;
using System.Collections;


public class GAFEnemy : MonoBehaviour
{
    public int id = 1;
    //敌人的生命值
    public float _HP = 1.0f;
    //敌人的射击间隔
    public float shootInterval = 1.0f;
    /// <summary>
    /// 每次攻击的射击次数
    /// </summary>
    [Tooltip("每次攻击的射击次数")]
    public int shootCountPerTime = 1;
    /// <summary>
    /// 是否随机射击次数
    /// </summary>
    [Tooltip("是否随机射击次数")]
    public bool randomShootCount = false;
    /// <summary>
    /// 随机射击次数的随机值
    /// </summary>
    [Tooltip("随机射击次数的随机值")]
    public int randomShootCountValue = 1;
    /// <summary>
    /// 连续射击的间隔时间
    /// </summary>
    [Tooltip("连续射击的间隔时间")]
    public float comboShootInterval = 0.15f;

    public AudioClip deathAduio;


    /// <summary>
    /// 死亡得分
    /// </summary>
    [Tooltip("死亡得分")]
    public int score = 0;

    float currentActionInterval;
    float timeFromShoot = 0f;
    bool _readyForShoot = true;

    bool firstShoot = false;
    /// <summary>
    /// 
    /// </summary>
    bool actioning = false;

    string currentActionName = "";

    //是否可以准备好射击
    public bool ReadyFroShoot
    {
        get { return _readyForShoot; }
        set { _readyForShoot = value; }
    }

    bool isDead = false;
    [Tooltip("动画控制器")]
    public Animator anim;

    BoxCollider2D collider2d;
    Collider2D[] coliders;
    public bool Injuring { get; set; }

    #region monobahavior 
    void Start()
    {
        currentActionInterval = GetActionInterval();
        timeFromShoot = 0.0f;
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }


        ////物理对象
        //coliders = GetComponents<Collider2D>();
        //if (coliders == null && coliders.Length <= 0)
        //{
        //    gameObject.AddComponent<BoxCollider2D>();
        //    coliders = GetComponents<Collider2D>();
        //}

        //获得射击对象 
        //if (target == null) {
        //    target = GameObject.FindWithTag("Player");
        //    if (target == null) {
        //        target = GameObject.FindWithTag("Player");
        //    }
        //}
    }

    /// <summary>
    /// 计算随机值
    /// </summary>
    /// <returns></returns>
    float GetActionInterval()
    {
        float randomFloat = Random.Range(-0.1f, 0.1f);
        return shootInterval + shootInterval * randomFloat;
    }


    // Update is called once per frame
    void Update()
    {
        if (ReadyFroShoot)
        {
            timeFromShoot += Time.deltaTime;
        }
        if (CanShoot())
        {

            //int attackCount = shootCountPerTime;
            //if(randomShootCount)
            //{
            //    attackCount += Random.Range(-randomShootCountValue, randomShootCountValue);
            //}
            //StartCoroutine(CoroutineShoot(attackCount));
            DoAction();
            firstShoot = false;
            currentActionInterval = GetActionInterval();
            timeFromShoot = 0f;
        }

    }
    #endregion


    /// <summary>
    /// 更新动画状态
    /// </summary>
    /// <param name="isActioning"></param>
    /// <param name="actionName"></param>
    public void UpdateAction(bool isActioning, string actionName = "")
    {
        actioning = isActioning;
        currentActionName = actionName;
    }

    /// <summary>
    /// 运行动作
    /// </summary>
    public void DoAction()
    {

        if (actioning)
            return;
        EnemyAction[] actions = GetComponents<EnemyAction>();


        if (actions != null && actions.Length > 0)
        {
            //根据权重
            int totalWight = 0;
            int i = 0;
            for (; i < actions.Length; i++)
            {
                totalWight += actions[i].weight;
            }
            int actionIndex = 0;
            int randomWight = Random.Range(0, totalWight + 1);
            for (i = 0; i < actions.Length; i++)
            {
                //if (actions[i].enabled)
                //{
                randomWight -= actions[i].weight;
                if (randomWight <= 0)
                {
                    actionIndex = i;
                    break;
                }
                //}
            }
            actions[actionIndex].Run();
        }
    }



    //IEnumerator CoroutineShoot(int t)
    //{
    //    // int count = int.Parse(t.ToString());
    //    ReadyFroShoot = false;
    //    for(int i=0;i<t;i++)
    //    {
    //        if(CanBrokenShoot())
    //        {
    //            break;
    //        }
    //        Shoot();
    //        yield return new WaitForSeconds(0.1f);

    //    }
    //    ReadyFroShoot = true;
    //}



    /// <summary>
    /// 判断是否能打断射击
    /// </summary>
    /// <returns></returns>
    bool CanBrokenShoot()
    {
        if (isDead || Injuring)
            return true;
        return false;
    }
    /// <summary>
    /// 是否可以射击
    /// </summary>
    /// <returns></returns>
    bool CanShoot()
    {
        bool canShoot = false;
        if (CanBrokenShoot())
        {
            return false;
        }
        //if(ReadyFroShoot &&)
        if (((timeFromShoot >= currentActionInterval) || firstShoot) && !(isDead || !ReadyFroShoot || !GameManager.Instance.IsInGame()))
        {
            canShoot = true;
        }

        return canShoot;
    }


    public void TakeDamage(float damageVal, bool isHeadShot = false)
    {
        //Debug.Log("Take damage :" + damageVal);
        if (isDead)
        {
            return;
        }
        if (isHeadShot)
        {
            _HP -= damageVal * 2;
        }
        else
        {
            _HP -= damageVal;
        }
        if (_HP <= 0.0f)
        {
            Die(isHeadShot);
        }
        else
        {
            if (anim != null)
            {
                if (!actioning)
                    anim.SetTrigger("injured");
            }
        }
    }

    void Die(bool isHeadShot = false)
    {
        //play die animation
        isDead = true;

        if (coliders != null)
        {
            for (int i = 0; i < coliders.Length; i++)
            {
                coliders[i].enabled = false;
            }
        }

        if (anim != null)
        {
            anim.SetTrigger("dead");
            //float length = anim.GetCurrentAnimatorClipInfo(0).Length;
        }

        //取消所有动作
        LeanTween.cancel(gameObject);
        Destroy(gameObject, 1);
        PlayDeathAudio();

        //通知GameManager死亡
        GameManager.Instance.EmenyDead(score, isHeadShot);

        LeanTween.dispatchEvent((int)Events.EMENYDIE);
    }

    /// <summary>
    /// 播放死亡音效
    /// </summary>
    void PlayDeathAudio()
    {
        if (deathAduio != null)
        {
            iTween.Stab(gameObject, deathAduio, 0f);
        }
    }
}
