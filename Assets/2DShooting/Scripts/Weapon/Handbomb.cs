using UnityEngine;
using System.Collections;

public class Handbomb : MonoBehaviour
{

    public float rotateTime = 10f;

    public float scaleTime = 10f;

    private Shootable shootable;
    //public float moveToTime = 5f;

    public float attack;
    public Transform bombTarget;

    public GameObject explosionEffect;

    /// <summary>
    /// 爆炸音效
    /// </summary>
    public AudioClip explosionAudio;
    // Use this for initialization
    void Start()
    {
        if (bombTarget == null)
        {
            bombTarget = GameObject.Find("HandbombTarget").transform;
        }
        if (shootable == null)
        {
            shootable = GetComponent<Shootable>();
        }
        //LeanTween.rot(gameObject, new Vector3(0, 0, 180), rotateTime).setRepeat(10); ;
        iTween.RotateBy(gameObject, iTween.Hash("amount", new Vector3(0, 0, 30), "time", rotateTime, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));

        LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1.5f), scaleTime);
        //Vector3[] pts = new Vector3[3];
        //pts[2] = transform.position + new Vector3(0, 1, 0);
        //pts[0] = bombTarget.position;
        //pts[1] = (pts[0] + pts[2]) / 2;
        
        LeanTween.move(gameObject, bombTarget.position, scaleTime).setEase(LeanTweenType.easeInExpo).setOnComplete(OnMoveComplete);
    }

    void OnMoveComplete()
    {
        //  LeanTween.move(gameObject, bombTarget.position, scaleTime);
        Explosion(false);
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
        if(explosionAudio != null)
        {
            SoundManager.PlayAduio(SoundManager.Instance.gameObject, explosionAudio);
        }
        if (!broken)
        {
            GameManager.Instance.PlayerInjured(attack);
            LeanTween.dispatchEvent((int)Events.SHAKECAMERA);
        }
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (shootable != null)
        {
            if (shootable.isDie)
                Explosion(true);
        }
    }
}
