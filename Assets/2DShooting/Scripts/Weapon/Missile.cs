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
	// Use this for initialization
	void Start () {
        shootable = GetComponent<Shootable>();
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), explosionTime).setOnComplete(()=>
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
        if(!broken)
        {
            GameManager.Instance.PlayerInjured(attack);
        }
        Destroy(gameObject);
    }
}
