using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

    /// <summary>
    /// 伤害值
    /// </summary>
    public float attack = 10f;

    /// <summary>
    /// 爆炸时间
    /// </summary>
    public float explosionTime = 8f;

    private Shootable shootable;

    public GameObject explosionEffect;
    /// <summary>
    /// 爆炸音效
    /// </summary>
    public AudioClip explosionAduio;
    // Use this for initialization
    void Start () {
        shootable = GetComponent<Shootable>();
        LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1f), explosionTime).setOnComplete(()=>
        {
            Explosion(false);
        });
        //iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(1f, 1f, 1f), "time", explosionTime, "easetype", iTween.EaseType.linear,));
    }
	
	// Update is called once per frame
	void Update () {
	    if(shootable != null )
        {
            if (shootable.isDie)
                Explosion(true);
        }
	}

    void Explosion(bool broken)
    {
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect);
            effect.transform.position = transform.position;
            effect.transform.SetParent(transform.parent);
            effect.transform.localScale *= transform.lossyScale.x;
        }

        if(explosionAduio != null)
        {
            SoundManager.PlayAduio(SoundManager.Instance.gameObject,explosionAduio, 1f);
        }

        if (!broken)
        {
            GameManager.Instance.PlayerInjured(attack);
            LeanTween.dispatchEvent((int)Events.SHAKECAMERA);
        }
        Destroy(gameObject);
    }
}
