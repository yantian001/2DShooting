using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

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

    //场景对象
    public GameObject background;

    public bool shakeWhenShoot = false;

	bool canShoot = false;
	float deltaTime =0.0f;
    Animator anim;
    void Start () {
		if (signTransform == null) {
			signTransform = GameObject.Find("Sign").transform;
		}
		if (anim == null) {
			anim = this.GetComponent<Animator>();
		}
		if (fireAudio == null) {
            fireAudio = GetComponent<AudioSource>().clip ;
		}
		canShoot = false;
		deltaTime = 0.0f;

        //获取随机射击的范围
        if(randomShooting && randomShootingSize == Vector3.zero)
        {
            if (signTransform != null && signTransform.GetComponent<SpriteRenderer>() != null )
            {
                randomShootingSize = signTransform.GetComponent<SpriteRenderer>().bounds.size;
            }
            else
            {
                randomShootingSize = new Vector3(.25f, .25f, 0f);
            }
        }
       
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime += Time.deltaTime;
		if (deltaTime >= shootInterval) {
			canShoot = true;
		} else {
			canShoot = false;
		}
	}


	public void Fire(bool isCombo){
		
		bool canFire = true;
		if (isCombo && !canComboFire) {
			canFire = false;
		} 
		if(canFire && canShoot)
        {
            Vector3 pos = GetShootPosition(isCombo);
            Shoot(pos);
        }
			
	}

    Vector3 GetShootPosition(bool comboFire)
    {
        Vector3 shootPos = signTransform.position;
        if(randomShooting && comboFire)
        {

            shootPos = shootPos + new Vector3(Random.Range(-randomShootingSize.x / 2, randomShootingSize.x / 2), Random.Range(-randomShootingSize.y / 2, randomShootingSize.y / 2), 0);
        }
        return shootPos;
    }

	public void Shoot(Vector3 postion){
		//Debug.Log (postion);
		deltaTime = 0;
		canShoot = false;
		anim.SetTrigger ("isShooting");
		PlayMuzzleEffect ();
		PlayShootAudio ();
        ShowBullet(postion);
        if(shakeWhenShoot)
        {
            ShakeBackground();
        }
        RaycastHit2D rayhit = Physics2D.Raycast (postion, Vector2.zero);
		if ((rayhit != null) && rayhit.collider != null) {
			//Debug.Log(rayhit.collider.name);
			Enemy enemy = rayhit.collider.gameObject.GetComponent<Enemy>();
           
			if(enemy != null){
				enemy.TakeDamage(attack);
			}
		}
        else
        {
            PlaySignImpactEffect(postion);
        }

	}

    //晃动
    void ShakeBackground()
    {
        //iTween.ShakePosition(background, new Vector3(.05f, .05f, 0), 0.05f);
        iTween.ShakePosition(Camera.main.gameObject, new Vector3(.05f, .05f, 0), 0.05f);

    }

    //显示子弹，从枪口到准星
    void ShowBullet(Vector3 target)
    {
        if(bullet == null)
        {
            return;
        }
        GameObject bult = Instantiate(bullet) as GameObject;
        bult.transform.position = muzzleEffectPlace.transform.position;
        bult.transform.parent = muzzleEffectPlace.transform;
        
        //Transform bulletTransform =
        //bulletTransform.position = signTransform.position;
        //bulletTransform.localScale = bult.transform.localScale * 0.5f;
        //子弹运行到准星的位置
        iTween.MoveTo(bult, iTween.Hash("position", target, "time", 0.1, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject,"oncompleteparams",(System.Object)bult));
        
    }

   public void OnBulletMoveComplete(System.Object target)
    {
       // Debug.Log("move complete");
       Destroy((Object)target);
    }

    //添加子弹击中地面的标记
	void PlaySignImpactEffect(Vector3 position){
        GameObject impact = Instantiate(impactEffect) as GameObject;
        impact.transform.localPosition = background.transform.InverseTransformVector(position);
        impact.transform.parent = background.transform;
    }

	//播放射击时枪口特效
	void PlayMuzzleEffect(){
		if (muzzleEffect != null && muzzleEffectPlace != null) {
			GameObject effect = (GameObject)Instantiate(muzzleEffect,muzzleEffectPlace.transform.position,muzzleEffectPlace.transform.rotation);
			effect.transform.parent = muzzleEffectPlace.transform;
		}
	}
	//播放开枪音效
	void PlayShootAudio()
	{
		if (fireAudio != null) {
			//if(!fireAudio.isPlaying)
			//	fireAudio.Play();

            iTween.Stab(gameObject, fireAudio, 0f);
//			AudioSource auio = (AudioSource)Instantiate(fireAudio);
//			auio.Play();
		}
	}
}
