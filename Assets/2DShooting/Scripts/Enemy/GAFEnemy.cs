using UnityEngine;
using System.Collections;


public class GAFEnemy : MonoBehaviour {

    //敌人的生命值
    public float _HP = 1.0f;
    //敌人的射击间隔
    public float shootInterval = 1.0f;

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
        timeFromShoot += Time.deltaTime;
        if (CanShoot()) {
            Shoot();
            timeFromShoot = 0;
        }

    }

    bool CanShoot() {
        bool canShoot = true;
        if (timeFromShoot < shootInterval || target == null || isDead || !ReadyFroShoot || !GameManager.Instance.IsInGame()) {
            canShoot = false;
        }
        return canShoot;
    }

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
        Vector3 pso = (target.transform.position);
        iTween.MoveTo(blt, iTween.Hash("position", pso, "time", 0.1, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", blt));

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
        Destroy(gameObject, 2);
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
