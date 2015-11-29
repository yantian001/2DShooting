using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour {

	// Use this for initialization
	public float _HP = 1.0f;
	bool isDead = false;
	Animator anim;
	BoxCollider2D collider;
	void Start () {
		anim = this.GetComponent<Animator> ();
		if (collider == null) {
			collider = GetComponent<BoxCollider2D>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
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
