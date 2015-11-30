using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour {

	//敌人的生命值
	public float _HP = 1.0f;
	//敌人的射击间隔
	public float shootInterval = 1.0f;
	//枪口位置
	public Transform firePlace;
	//子弹
	public GameObject bullet;
	//射击目标（主角）
	public GameObject target ;

	float timeFromShoot = 0f;
	bool isDead = false;
	Animator anim;
	BoxCollider2D collider;
	void Start () {
		timeFromShoot = 0.0f;
		anim = this.GetComponent<Animator> ();
		if (collider == null) {
			collider = GetComponent<BoxCollider2D>();
			if(collider == null){
				collider = gameObject.AddComponent<BoxCollider2D>();
			}
		}

		//获得射击对象 
		if (target == null) {
			target = GameObject.FindWithTag("Player");
			if(target == null){
				target = GameObject.FindWithTag("Player");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		timeFromShoot += Time.deltaTime;
		if (CanShoot ()) {
			Shoot();
			timeFromShoot = 0;
		}

	}

	bool CanShoot(){
		bool canShoot = true;
		if (timeFromShoot < shootInterval || target == null || isDead) {
			canShoot = false;
		}
		return canShoot;

	}

	void Shoot(){
		//Debug.Log ("shooting:");
		ShowBullet ();

	}

	void ShowBullet()
	{
		Vector3 to = (firePlace.transform.position - target.transform.position).normalized;
		GameObject blt = (GameObject)Instantiate(bullet,firePlace.transform.position,Quaternion.FromToRotation(Vector3.right,to));
		Vector3 scale = blt.transform.localScale;
		blt.transform.localScale = Vector3.zero;
		iTween.MoveTo (blt, iTween.Hash ("position", target.transform.position, "time", 0.2, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", blt));
		iTween.ScaleTo (blt, scale,.2f);
	}

	void OnBulletMoveComplete(System.Object obj){
		Destroy ((GameObject)obj);
	}

	public void TakeDamage(float damageVal){
		Debug.Log ("Take damage :" + damageVal);
		if (isDead) {
			return;
		}
		_HP -= damageVal;
		if (_HP <= 0.0f) {
			Die();
		}
	}

	void Die(){
		//play die animation
		isDead = true;
		collider.enabled = false;
		if (anim != null) {
			anim.SetTrigger("dead");
			//float length = anim.GetCurrentAnimatorClipInfo(0).Length;

			Destroy(gameObject,2);
		}
	}
}
