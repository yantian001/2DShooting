using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {


    public float speed = 1.0f;

    /// <summary>
    /// 爆炸特效
    /// </summary>
    public GameObject explosionEffect;
    /// <summary>
    /// 爆炸音效
    /// </summary>
    public AudioClip explosionAudio;
    /// <summary>
    /// 伤害范围
    /// </summary>
    public float damageRadui = 1f;
    /// <summary>
    /// 攻击
    /// </summary>
    public Attackable attack;

    Vector3 target;

	// Use this for initialization
	void Start () {
	    if(attack == null)
        {
            attack = GetComponent<Attackable>();
        }
       // SetTarget(new Vector3(-1f, -1f, -1f));
	}

    void Run()
    {
        float distance = Vector3.Distance(transform.position, target);
        float time = distance / speed;
        LeanTween.move(gameObject, target, time).setOnComplete(OnArriveTarget);
    }

    void OnArriveTarget()
    {
        if(explosionEffect != null)
        {
            var explosion = (GameObject)Instantiate(explosionEffect, target, Quaternion.identity);
            explosion.transform.SetParent(transform.parent);

            SoundManager.PlayAduio(SoundManager.Instance.gameObject, explosionAudio);
            LeanTween.dispatchEvent((int)Events.SHAKECAMERA);
            TakeEffect();
        }

        Destroy(gameObject, 0.1f);
    }

    void TakeEffect()
    {
        if (attack == null)
            return;
        var hits = Physics2D.CircleCastAll(target, damageRadui,Vector2.zero);

        if(hits!=null && hits.Length >0 )
        {
            for(int i=0;i<hits.Length;i++)
            {
                GAFEnemy gafEnemy = hits[i].collider.gameObject.GetComponentInParent<GAFEnemy>();
                if (gafEnemy == null)
                {
                    gafEnemy = hits[i].collider.gameObject.GetComponent<GAFEnemy>();
                }
                
                if(gafEnemy != null)
                {
                    gafEnemy.TakeDamage(attack.GetAttack());
                }
            }
        }
    }


    public void SetTarget(Vector3 _target)
    {
        target = _target;
        Run();
    }
}
