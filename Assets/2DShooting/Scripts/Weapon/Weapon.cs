using UnityEngine;
using System.Collections;
using System;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 武器的ID
    /// </summary>
    public int ID = 0;

    private WeaponItem weaponItem;
    /// <summary>
    /// 武器名字
    /// </summary>
    public string Name = "";

    private float _attack;
    //枪支的攻击力
    public float attack
    {
        get
        {

            return Mathf.CeilToInt(_attack * (1 - UnityEngine.Random.Range(-10f, 10f) / 100));
        }
        set
        {
            _attack = value;
        }
    }

    //射击间隔
    [HideInInspector]
    public float shootInterval = 0.2f;
    //子弹数量
    [HideInInspector]
    public int BulletCount;
    /// <summary>
    /// 弹夹容量
    /// </summary>
    [HideInInspector]
    private int magSize;
    /// <summary>
    /// 得分加成
    /// </summary>
    [HideInInspector]
    public float scoreBonus = 0.0f;

    //是否可以连续开枪
    public bool canComboFire = true;

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

    public Transform shellPlace;

    public GameObject shellEffect;


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
    /// 武器的icon
    /// </summary>
    public Texture2D WeaponIcon;

    bool canShoot = false;
    float deltaTime = 0.0f;
    public Animator anim;

    int curBulltCount, curMagSize;
    bool isBulltOk = false;
    bool isReloading = false;
    bool isWeaponOuting = false;

    void Start()
    {

        UpdateBulletDisplay();
    }

    public void Awake()
    {

        //Debug.Log("wepon init");
        weaponItem = WeaponManager.Instance.GetWeaponItemById(ID);
        if (weaponItem == null)
        {
            Debug.LogError("Miss weapon info.");
            return;
        }

        WeaponProperty wp = weaponItem.GetCurrentProperty();
        attack = wp.Power;
        shootInterval = 1f / wp.FireRate;
        this.scoreBonus = wp.ScoreBonus;
        curBulltCount = BulletCount = wp.BulletCount;

        magSize = wp.ClipSize;

    }

    public void OnEnable()
    {
        if (background == null)
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
                    joystickMovement.smoothRatio = moveSpeed;
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
        curMagSize = magSize;
        isBulltOk = true;
        //监听换弹夹事件
        LeanTween.addListener(gameObject, (int)Events.RELOAD, Reload);
        LeanTween.addListener(gameObject, (int)Events.CLIPREFILL, Refill);
        UpdateBulletDisplay();
        //Debug.Log("Weapon enabled!");
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.RELOAD, Reload);
        LeanTween.removeListener(gameObject, (int)Events.CLIPREFILL, Refill);
    }


    //更新子弹数量显示
    void UpdateBulletDisplay()
    {
        LeanTween.dispatchEvent((int)Events.BULLETCHANGED, string.Format("{0}/{1}/{2}", curMagSize, magSize, curBulltCount));
    }

    //换弹夹
    void Reload(LTEvent ent = null)
    {
        if (isReloading)
            return;
        int refillCount = magSize - curMagSize;
        if (curBulltCount < refillCount)
        {
            refillCount = curBulltCount;
        }
        curBulltCount -= refillCount;
        curMagSize += refillCount;
        if (anim)
        {
            isReloading = true;
            anim.SetTrigger("reload");

        }
        else
        {
            UpdateBulletDisplay();
            isReloading = false;
        }
        //isBulltOk = false;
    }

    /// <summary>
    /// 重新填充
    /// </summary>
    public void Refill(LTEvent evt)
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.WeaponEqiuped);
        //curMagSize = magSize;
        curBulltCount += BulletCount;
        UpdateBulletDisplay();
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
        if (curMagSize <= 0 && curBulltCount > 0)
        {
            Reload();
        }
        else
        {
            if (curMagSize <= 0 && curBulltCount <= 0)
            {
                if(isBulltOk == true)
                {
                    LeanTween.dispatchEvent((int)Events.NEEDBULLET);
                }
                isBulltOk = false;
            }
            else
            {
                isBulltOk = true;
            }
        }
    }

    void LoadFinish()
    {
        isReloading = false;
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
        if (canFire && canShoot && isBulltOk && !isReloading && !isWeaponOuting && (!GameManager.Instance.IsGamePauseOrOver()))
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
        ShowShell();
        //更新子弹数量显示
        curMagSize -= 1;
        UpdateBulletDisplay();
        if (shakeWhenShoot)
        {
            ShakeBackground();
        }
        RaycastHit2D rayhit = Physics2D.Raycast(postion, Vector2.zero);
        bool isHitEnemy = false;
        if (rayhit.collider != null)
        {
            GAFEnemy gafEnemy = rayhit.collider.gameObject.GetComponentInParent<GAFEnemy>();
            if (gafEnemy == null)
            {
                gafEnemy = rayhit.collider.gameObject.GetComponent<GAFEnemy>();
            }
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
                isHitEnemy = true;
            }

            Shootable shootable = rayhit.collider.GetComponent<Shootable>();
            if (shootable != null)
            {
                shootable.TakeDemage(attack);
                isHitEnemy = true;
            }

            //判断是否击中了箱子
            GameItem item = rayhit.collider.GetComponent<GameItem>();
            if (item)
            {
                item.TakeDamage(attack);
                isHitEnemy = true;
            }
        }
        if (!isHitEnemy)
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
        if (background != null)
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
        Vector3 to = target - muzzleEffectPlace.transform.position;
        GameObject bult = Instantiate(bullet, muzzleEffectPlace.transform.position, Quaternion.FromToRotation(Vector3.up, to)) as GameObject;
       // bult.transform.position = muzzleEffectPlace.transform.position;

        iTween.MoveTo(bult, iTween.Hash("position", target, "time", 0.2, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", bult));

    }

    void ShowShell()
    {
        if (shellPlace && shellEffect)
        {
            var shell = (GameObject)Instantiate(shellEffect, shellPlace.position, shellPlace.rotation);
            shell.transform.SetParent(shellPlace);
        }
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
    private System.Action weaponOutCallback;
    public void WeaponOut(System.Action callback)
    {
        isWeaponOuting = true;
        weaponOutCallback = callback;
        anim.SetTrigger("out");
    }


    public void OnWeaponOutComplete()
    {
        transform.parent.gameObject.SetActive(false);
        isWeaponOuting = false;
        if (weaponOutCallback != null)
            weaponOutCallback();
    }

    public void WeaponIn()
    {
        //throw new NotImplementedException();
        transform.parent.gameObject.SetActive(true);
        anim.SetTrigger("in");
    }

    public void OnWeaponInComplete()
    {
        transform.localPosition = transform.localPosition;
    }
}
