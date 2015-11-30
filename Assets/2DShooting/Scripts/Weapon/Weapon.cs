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
	
    //枪击的音效
	public AudioSource fireAudio;
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
			fireAudio = GetComponent<AudioSource>();
		}
		canShoot = false;
		deltaTime = 0.0f;
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
		Vector3 pos = signTransform.position;
		bool canFire = true;
		if (isCombo && !canComboFire) {
			canFire = false;
		} 

		if(canFire && canShoot)
			Shoot (pos);
	}

	public void Shoot(Vector3 postion){
		//Debug.Log (postion);
		deltaTime = 0;
		canShoot = false;
		anim.SetTrigger ("isShooting");
		PlayMuzzleEffect ();
		PlayShootAudio ();
        ShowBullet();
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

    //显示子弹，从枪口到准星
    void ShowBullet()
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
        iTween.MoveTo(bult, iTween.Hash("position", signTransform.position, "time", 0.1, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject,"oncompleteparams",(System.Object)bult));
        
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
			if(!fireAudio.isPlaying)
				fireAudio.Play();
//			AudioSource auio = (AudioSource)Instantiate(fireAudio);
//			auio.Play();
		}
	}
}
