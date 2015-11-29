using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	//是否可以连续开枪
	public bool canComboFire = true;

	public float attack = 1.0f;
	//射击间隔
	public float shootInterval = 0.2f;
	//瞄准器的位置
	public Transform signTransform;

	public Animator anim;

	public AudioSource fireAudio;
	//枪口效果位置
	public GameObject muzzleEffectPlace;

	public GameObject muzzleEffect;
	// Use this for initialization

	public GameObject impactEffect;

	bool canShoot = false;
	float deltaTime =0.0f;
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
//		if (fireAudio != null && !fireAudio.isPlaying) {
//			fireAudio.Play();
//		}
		PlayShootAudio ();
		//Debug.DrawRay (postion, Vector3.zero,Color.blue);
		//LayerMask mask = ~(1 << 0);
		RaycastHit2D rayhit = Physics2D.Raycast (postion, Vector2.zero);
		RaycastHit2D[] rayhitall = Physics2D.RaycastAll (postion, Vector2.zero);
		if (rayhitall.Length > 0) {
			for(int i = 0 ;i < rayhitall.Length;i++)
			{
				if(rayhitall[i].collider != null){
					Debug.Log(rayhitall[i].collider.name);
				}
			}
		}
		if (rayhit != null && rayhit.collider != null) {
			//Debug.Log(rayhit.collider.name);
			Enemy enemy = rayhit.collider.gameObject.GetComponent<Enemy>();
			if(enemy != null){
				enemy.TakeDamage(attack);
			}
		}

	}

	void PlaySignImpactEffect(){
		
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
