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
    //枪口效果
    public GameObject muzzleEffect;
	float timeFromShoot = 0f;
	bool _readyForShoot = true;

    //是否可以准备好射击
    public bool ReadyFroShoot
    {
        get { return _readyForShoot; }
        set { _readyForShoot = value; }
    }

	bool isDead = false;
	Animator anim;
	BoxCollider2D collider2d;
    Collider2D[] coliders;

	void Start () {
		timeFromShoot = 0.0f;
		anim = this.GetComponent<Animator> ();

        //物理对象
        coliders = GetComponents<Collider2D>();
        if (coliders == null && coliders.Length <= 0)
        {
            gameObject.AddComponent<BoxCollider2D>();
            coliders = GetComponents<Collider2D>();
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
		if (timeFromShoot < shootInterval || target == null || isDead || !ReadyFroShoot) {
			canShoot = false;
		}
		return canShoot;

	}

	void Shoot(){
        //Debug.Log ("shooting:");
        PlayFireAnimation();
        PlayMuzzleEffect(firePlace);
		ShowBullet ();

	}

    //显示射击子弹
	void ShowBullet()
	{
        Vector3 to = (firePlace.transform.position - target.transform.position).normalized;
       // Vector3 to = (Camera.main.WorldToScreenPoint(firePlace.transform.position) - Camera.main.WorldToScreenPoint(target.transform.position)).normalized;
        Debug.Log( Camera.main.WorldToScreenPoint(target.transform.position));
        Debug.Log(Camera.main.WorldToScreenPoint(firePlace.transform.position));
        GameObject blt = (GameObject)Instantiate(bullet,firePlace.transform.position,Quaternion.FromToRotation(Vector3.right,to));
       

        blt.transform.parent = firePlace.transform;
        Debug.Log("target world pos:" + target.transform.position); 
        Debug.Log("target world pos1:" + target.transform.TransformVector(target.transform.localPosition));
        Vector3 pso = blt.transform.parent.InverseTransformVector(target.transform.position);
        //Camera.main.()
        //Vector3 scale = blt.transform.localScale;
        //blt.transform.localScale = Vector3.zero;
        // X, Y
       // Vector3 movetoXY = new Vector3(target.transform.position.x, target.transform.position.y, blt.transform.position.z);
		iTween.MoveTo (blt, iTween.Hash ("position", pso, "islocal",true, "time", 1, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", blt));
        //Vector3 movetoZ = new Vector3(blt.transform.position.x, blt.transform.position.y, target.transform.position.z);
        //iTween.MoveTo(blt, iTween.Hash("position", movetoZ, "time", 1, "oncomplete", "OnBulletMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", blt));
        
        //iTween.move
        //iTween.ScaleTo (blt, scale,.2f);
	}
    // 子弹自动销毁
	void OnBulletMoveComplete(System.Object obj){
		Destroy ((GameObject)obj);
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
        if (anim != null)
        {
            anim.SetTrigger("shoot");
        }
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
			Destroy(gameObject,2);
		}
	}
}
