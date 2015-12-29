using UnityEngine;
using System.Collections;
using System;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 武器的ID
    /// </summary>
    public int ID = 0;

    public string Name = "";
    //是否可以连续开枪
    public bool canComboFire = true;
    //枪支的攻击力
    public float attack = 1.0f;
    //射击间隔
    public float shootInterval = 0.2f;
    //瞄准器的位置
    public Transform signTransform;
    //是否随机射击，否则一直射击准星
    public bool randomShooting = false;
    //随机射击的大小
    public Vector3 randomShootingSize = Vector3.zero;

    //枪击的音效
    public AudioClip fireAudio;
    //枪口效果位置
    public GameObject muzzleEffectPlace;
    //枪口的效果
    public GameObject muzzleEffect;
    //子弹打中物体的效果
    public GameObject impactEffect;
    //子弹
    public GameObject bullet;
    //子弹数量
    public int BulletCount;
    
    //场景对象
    public GameObject background;

    public bool shakeWhenShoot = false;
    /// <summary>
    /// 是否覆盖移动速度
    /// </summary>
    public bool overrideMovement = false;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed = 30f;
    /// <summary>
    /// 是否检查敌人
    /// </summary>
    public bool checkNearTarget = false;
    /// <summary>
    /// 近敌人移动速度
    /// </summary>
    public float nearTargetMoveSpeed = 20f;
    /// <summary>
    /// 得分加成
    /// </summary>
    public float scoreBonus = 0.0f;

    /// <summary>
    /// 武器的icon
    /// </summary>
    public Texture2D WeaponIcon;

    bool canShoot = false;
    float deltaTime = 0.0f;
    Animator anim;

    int curBulltCount;
    bool isBulltOk = false;

    void Start()
    {
        UpdateBulletDisplay();
    }

    public void OnEnable()
    {
        if(background == null)
        {
            background = GameObject.FindGameObjectWithTag("Background");
        }
        if (signTransform == null)
        {
            signTransform = GameObject.Find("Sign").transform;
        }
        if (anim == null)
        {
            anim = this.GetComponent<Animator>();
        }
        if (fireAudio == null)
        {
            fireAudio = GetComponent<AudioSource>().clip;
        }
        canShoot = false;
        deltaTime = 0.0f;

        //设置移动速度
        if (background != null)
        {
            var joystickMovement = background.GetComponent<JoystickBackgroundMovment>();
            if (joystickMovement != null)
            {
                joystickMovement.signTran = signTransform;
                if (overrideMovement)
                {
                    joystickMovement.smoothRatio = moveSpeed ;
                    joystickMovement.checkNearTarget = checkNearTarget;
                    joystickMovement.nearSmoothRatio = nearTargetMoveSpeed;
                }
            }
        }
        
        //获取随机射击的范围
        if (randomShooting && randomShootingSize == Vector3.zero)
        {
            if (signTransform != null && signTransform.GetComponent<SpriteRenderer>() != null)
            {
                randomShootingSize = signTransform.GetComponent<SpriteRenderer>().bounds.size;
            }
            else
            {
                randomShootingSize = new Vector3(.25f, .25f, 0f);
            }
        }

        //现有子弹数量
        curBulltCount = BulletCount;
        isBulltOk = true;
        //监听换弹夹事件
        LeanTween.addListener(gameObject, (int)Events.RELOAD, Reload);
        UpdateBulletDisplay();
        //Debug.Log("Weapon enabled!");
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.RELOAD, Reload);
    }


    //更新子弹数量显示
    void UpdateBulletDisplay()
    {
        LeanTween.dispatchEvent((int)Events.BULLETCHANGED, string.Format("{0}/{1}", curBulltCount, BulletCount));
    }

    //换弹夹
    void Reload(LTEvent ent = null)
    {
        curBulltCount = BulletCount;
        if (anim)
        {
            isBulltOk = false;
            anim.SetTrigger("reload");

        }
        else
        {
            UpdateBulletDisplay();
            isBulltOk = true;
        }
        //isBulltOk = false;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime >= shootInterval)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }
        CheckBulletStatu();
    }

    void CheckBulletStatu()
    {
        if (curBulltCount <= 0)
        {
            Reload();
        }
    }

    void LoadFinish()
    {
        isBulltOk = true;
        UpdateBulletDisplay();
    }

    public void Fire(bool isCombo)
    {

        bool canFire = true;
        CheckBulletStatu();
        if (isCombo && !canComboFire)
        {
            canFire = false;
        }
        if (canFire && canShoot && isBulltOk && GameManager.Instance.Statu == GameManager.GameStatu.InGame)
        {
            Vector3 pos = GetShootPosition(isCombo);
            Shoot(pos);
        }

    }

    Vector3 GetShootPosition(bool comboFire)
    {
        Vector3 shootPos = signTransform.position;
        if (randomShooting && comboFire)
        {

            shootPos = shootPos + new Vector3(UnityEngine.Random.Range(-randomShootingSize.x / 2, randomShootingSize.x / 2), UnityEngine.Random.Range(-randomShootingSize.y / 2, randomShootingSize.y / 2), 0);
        }
        return shootPos;
    }

    public void Shoot(Vector3 postion)
    {
        //Debug.Log (postion);
        deltaTime = 0;
        canShoot = false;
        anim.SetTrigger("isShooting");
        PlayMuzzleEffect();
        PlaySignEffect();
        PlayShootAudio();
        ShowBullet(postion);
        //更新子弹数量显示
        curBulltCount -= 1;
        UpdateBulletDisplay();
        if (shakeWhenShoot)
        {
            ShakeBackground();
        }
        RaycastHit2D rayhit = Physics2D.Raycast(postion, Vector2.zero);
        if (rayhit.collider != null)
        {
           // Debug.Log(rayhit.collider.name);

            //是否击中了敌人
            Enemy enemy = rayhit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {

                Debug.Log(rayhit.collider.GetType());
                if (rayhit.collider.GetType() == typeof(CircleCollider2D))
                {
                    enemy.TakeDamage(attack, true);
                }
                else
                {
                    enemy.TakeDamage(attack);
                }

            }

            GAFEnemy gafEnemy = rayhit.collider.gameObject.GetComponentInParent<GAFEnemy>();
            if (gafEnemy != null)
            {

               // Debug.Log(gafEnemy.name);
                //Debug.Log(rayhit.collider.GetType());
                if (rayhit.collider.GetType() == typeof(CircleCollider2D))
                {
                    gafEnemy.TakeDamage(attack, true);
                }
                else
                {
                    gafEnemy.TakeDamage(attack);
                }

            }


            //判断是否击中了箱子
            GameItem item = rayhit.collider.GetComponent<GameItem>();
            if (item)
            {
                item.TakeDamage(attack);
            }
        }
        else
        {
            PlaySignImpactEffect(postion);
        }

    }

    /// <summary>
    /// 播放准星动画
    /// </summary>
    private void PlaySignEffect()
    {
        //throw new NotImplementedException();
        if (signTransform != null)
        {
            var animator = signTransform.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("shot");
            }
        }
    }

    //晃动
    void ShakeBackground()
    {
        //iTween.ShakePosition(background, new Vector3(.05f, .05f, 0), 0.05f);
        if(background != null)
        {
            iTween.ShakePosition(background, new Vector3(0.025f, 0.025f, 0), 0.01f);
        }
        //iTween.ShakePosition(Camera.main.gameObject, new Vector3(.015f, .015f, 0), 0.01f);

    }

    //显示子弹，从枪口到准星
    void ShowBullet(Vector3 target)
    {
        if (bullet == null)
        {
            return;
        }
        GameObject bult = Instantiate(bullet) as GameObject;
        bult.transform.position = muzzleEffectPlace.transform.position;

        iTween.MoveTo(bult, iTween.Hash("position", target, "time", 0.2, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", bult));

    }

    public void OnBulletMoveComplete(System.Object target)
    {
        // Debug.Log("move complete");
        Destroy((target as UnityEngine.Object));
    }

    //添加子弹击中地面的标记
    void PlaySignImpactEffect(Vector3 position)
    {
        GameObject impact = Instantiate(impactEffect) as GameObject;
        impact.transform.localPosition = background.transform.InverseTransformVector(position);
        impact.transform.parent = background.transform;
    }

    //播放射击时枪口特效
    void PlayMuzzleEffect()
    {
        if (muzzleEffect != null && muzzleEffectPlace != null)
        {
            GameObject effect = (GameObject)Instantiate(muzzleEffect, muzzleEffectPlace.transform.position, muzzleEffectPlace.transform.rotation);
            effect.transform.localScale = muzzleEffectPlace.transform.localScale;
            effect.transform.parent = muzzleEffectPlace.transform;
        }
    }
    //播放开枪音效
    void PlayShootAudio()
    {
        if (fireAudio != null)
        {
            //if(!fireAudio.isPlaying)
            //	fireAudio.Play();

            iTween.Stab(gameObject, fireAudio, 0f);
            //			AudioSource auio = (AudioSource)Instantiate(fireAudio);
            //			auio.Play();
        }
    }
}
