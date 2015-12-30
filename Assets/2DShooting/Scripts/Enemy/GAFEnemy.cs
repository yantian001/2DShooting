using UnityEngine;
using System.Collections;


public class GAFEnemy : MonoBehaviour {

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

    /// <summary>
    /// 攻击值
    /// </summary>
    public float attack = 1.0f;
    //枪口位置
    public Transform firePlace;
    //子弹
    public GameObject bullet;
    //射击目标（主角）
    public GameObject target;
    //枪口效果
    public GameObject muzzleEffect;
    //死亡音效
    public AudioClip deathAduio;

    //开枪音效
    public AudioClip fireAudio;
    /// <summary>
    /// 死亡得分
    /// </summary>
    [Tooltip("死亡得分")]
    public int score = 0;
    float timeFromShoot = 0f;
    bool _readyForShoot = true;

    bool firstShoot = true;

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

    void Start() {
        
        timeFromShoot = 0.0f;
        if(anim == null)
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
        if (target == null) {
            target = GameObject.FindWithTag("Player");
            if (target == null) {
                target = GameObject.FindWithTag("Player");
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (ReadyFroShoot)
        {
            timeFromShoot += Time.deltaTime;
        }
        if (CanShoot()) {

            int attackCount = shootCountPerTime;
            if(randomShootCount)
            {
                attackCount += Random.Range(-randomShootCountValue, randomShootCountValue);
            }
            //StartCoroutine(CoroutineShoot(attackCount));
            DoAction();
            firstShoot = false;
            timeFromShoot = 0f;
        }

    }

    public void DoAction()
    {
        EnemyAction[] actions = GetComponents<EnemyAction>();
        if(actions != null && actions.Length > 0)
        {
            //根据权重
            int totalWight = 0;
            int i = 0;
            for(;i<actions.Length;i++)
            {
                totalWight += actions[i].weight;
            }
            int actionIndex = 0;
            int randomWight = Random.Range(0, totalWight + 1);
            for(i=0;i<actions.Length;i++)
            {
                randomWight -= actions[i].weight;
                if(randomWight <= 0)
                {
                    actionIndex = i;
                    break;
                }
            }
            actions[actionIndex].Run();
        }
    }

    IEnumerator CoroutineShoot(int t)
    {
        // int count = int.Parse(t.ToString());
        ReadyFroShoot = false;
        for(int i=0;i<t;i++)
        {
            if(CanBrokenShoot())
            {
                break;
            }
            Shoot();
            yield return new WaitForSeconds(0.1f);

        }
        ReadyFroShoot = true;
    }

   

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
    bool CanShoot() {
        bool canShoot = false;
        if(CanBrokenShoot())
        {
            return false;
        }
        //if(ReadyFroShoot &&)
        if (((timeFromShoot >= shootInterval) || firstShoot) && !(target == null || isDead || !ReadyFroShoot || !GameManager.Instance.IsInGame())) {
            canShoot = true;
        }
        
        return canShoot;
    }
    /// <summary>
    /// 射击
    /// </summary>
    void Shoot() {
        //Debug.Log ("shooting:");
        PlayFireAnimation();
        PlayMuzzleEffect(firePlace);
        ShowBullet();
        PlayFireAudio();
        GameManager.Instance.PlayerInjured(attack);
    }

    //播放开枪音效
    void PlayFireAudio()
    {
        if(fireAudio != null)
        {
            iTween.Stab(gameObject, fireAudio, 0f);
        }
    }

    //显示射击子弹
    void ShowBullet()
    {
        Vector3 to = (firePlace.transform.position - target.transform.position).normalized;
        GameObject blt = (GameObject)Instantiate(bullet, firePlace.transform.position, Quaternion.FromToRotation(Vector3.right, to));
        blt.transform.parent = firePlace;
        Vector3 scale = blt.transform.localScale;
        blt.transform.localScale = Vector3.zero;
        iTween.MoveTo(blt, iTween.Hash("position", target.transform.position, "time",1, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", blt));
        iTween.ScaleTo(blt, scale, 0.2f);
    }
    // 子弹自动销毁
    void OnBulletMoveComplete(System.Object obj) {
        Destroy((GameObject)obj);
    }

    void PlayMuzzleEffect(Transform tran)
    {
        if (muzzleEffect != null)
        {
            GameObject muzzle = Instantiate(muzzleEffect);
            muzzle.transform.position = tran.position;
            muzzle.transform.localScale = transform.lossyScale;
            muzzle.transform.parent = tran;
        }
    }

    //播放人物射击动画
    void PlayFireAnimation()
    {
        //if (anim != null)
        //{
        //    anim.SetTrigger("shoot");
        //}
    }

    public void TakeDamage(float damageVal,bool isHeadShot = false) {
        //Debug.Log("Take damage :" + damageVal);
        if (isDead) {
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
        if (_HP <= 0.0f) {
            Die(isHeadShot);
        }
        else
        {
            if(anim != null)
            {
                anim.SetTrigger("injured");
            }
        }
    }

    void Die(bool isHeadShot = false) {
        //play die animation
        isDead = true;

        if (coliders != null)
        {
            for (int i = 0; i < coliders.Length; i++)
            {
                coliders[i].enabled = false;
            }
        }

        if (anim != null) {
            anim.SetTrigger("dead");
            //float length = anim.GetCurrentAnimatorClipInfo(0).Length;
        }
        Destroy(gameObject, 1);
        PlayDeathAudio();

        //通知GameManager死亡
        GameManager.Instance.EmenyDead(score,isHeadShot);
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
