using UnityEngine;
using System.Collections;

public class EnemyEmerge : MonoBehaviour
{

    //出现的方式
    public enum EmergeType
    {
        Runin,
        Dropdown
    }
    //进入的方式
    public EmergeType emergeType;
    //进入方式 影响的数值 ,Z 表示缩放大小
    public Vector3 emergeValue = Vector3.zero;
    //进入方式持续时间
    public float emergeTime = 0.1f;
    //是否在start中执行
    public bool runOnStart = false;

    bool isFinish = false;
    //是否执行完成
    public bool IsFinish
    {
        get
        {
            return isFinish;
        }
        private set
        {
            isFinish = value;
        }
    }
    Animator anim;
    // Use this for initialization
    void Start()
    {

        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        isFinish = false;
        if (runOnStart)
        {

        }
    }

    //执行出现
    public void RunEmerge()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();

        }
        switch (emergeType)
        {
            case EmergeType.Runin:
                Vector3 pos = gameObject.transform.localPosition + emergeValue;
                pos = gameObject.transform.parent.InverseTransformVector(pos);
                iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", emergeTime, "easetype","linear" , "oncomplete", "OnEmergeComplete", "islocal",true, "oncompletetarget", gameObject));
                if (emergeValue.z != 0)
                {
                    Vector3 scale = new Vector3(emergeValue.z, emergeValue.z, emergeValue.z);
                    iTween.ScaleTo(gameObject, scale, emergeTime);
                }
                if (anim != null)
                {
                    anim.SetBool("isWalking", true);
                }
                break;
            case EmergeType.Dropdown:
                break;
        }
    }

    void OnEmergeComplete()
    {
        anim.SetBool("isWalking", false);
        isFinish = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CopyFrom(EnemyEmerge e)
    {
        emergeType = e.emergeType;
        emergeValue = e.emergeValue;
        emergeTime = e.emergeTime;
    }
}
