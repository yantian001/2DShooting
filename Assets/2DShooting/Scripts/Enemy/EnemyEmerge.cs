using UnityEngine;
using System.Collections;

public class EnemyEmerge : MonoBehaviour
{

    //出现的方式
    public enum EmergeType
    {
        Runin,
        Dropdown,
        Roll,
        ParaChute
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

    GAFEnemy enemy;
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

    public void OnEnable()
    {
        //if (anim == null)
        //{
        //    anim = GetComponent<Animator>();
        //}

        if (enemy == null)
        {
            enemy = GetComponent<GAFEnemy>();
        }
        if (enemy != null)
        {
            enemy.ReadyFroShoot = false;
            anim = enemy.anim;
          //  RunEmerge();
        }
      
    }

    //执行出现
    public void RunEmerge()
    {
        switch (emergeType)
        {
            case EmergeType.Runin:
                RunAction("isWalking");
                break;
            case EmergeType.Dropdown:
                RunDrop();
                break;
            case EmergeType.Roll:
                RunAction("isRolling");
                break;
            case EmergeType.ParaChute:
                RunParachute();
                break;
        }
    }

    void RunParachute()
    {
         var parachute = transform.FindChild("Parachute");
        if(parachute)
        {
            parachute.gameObject.SetActive(true);
        }

        var mask = transform.FindChild("mask");
        if (mask)
            mask.gameObject.SetActive(false);

        Vector3 pos = gameObject.transform.localPosition + emergeValue;
        pos = gameObject.transform.parent.InverseTransformVector(pos);
        iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", emergeTime, "easetype", "linear", "oncomplete", "OnRunParachuteComplete", "islocal", true, "oncompletetarget", gameObject));
        if (anim != null)
        {
            anim.SetBool("parachute", true);
        }
    }


    void OnRunParachuteComplete()
    {
        var parachute = transform.FindChild("Parachute");
        if (parachute)
        {
            parachute.gameObject.SetActive(false);
        }
        var mask = transform.FindChild("mask");
        if (mask)
            mask.gameObject.SetActive(true);
        if (enemy)
        {
            enemy.ReadyFroShoot = true;
        }
        if (anim != null)
        {
            anim.SetBool("parachute", false);
        }
        isFinish = true;
    }

    void RunAction(string paramName)
    {
        Vector3 pos = gameObject.transform.localPosition + emergeValue;
        pos = gameObject.transform.parent.InverseTransformVector(pos);
        iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", emergeTime, "easetype", "linear", "oncomplete", "OnEmergeComplete", "islocal", true, "oncompletetarget", gameObject, "oncompleteparams", paramName));
        if (emergeValue.z != 0)
        {
            Vector3 scale = new Vector3(emergeValue.z, emergeValue.z, emergeValue.z);
            iTween.ScaleTo(gameObject, scale, emergeTime);
        }
        if (anim != null)
        {
            anim.SetBool(paramName, true);
        }
    }
    /// <summary>
    /// 跳入
    /// </summary>
    void RunDrop()
    {
        Vector3 pos = gameObject.transform.localPosition + emergeValue;
        pos = gameObject.transform.parent.InverseTransformVector(pos);
        iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", emergeTime, "easetype", "linear", "oncomplete", "OnRunDropComplete", "islocal", true, "oncompletetarget", gameObject));
        if (emergeValue.z != 0)
        {
            Vector3 scale = new Vector3(emergeValue.z, emergeValue.z, emergeValue.z);
            iTween.ScaleTo(gameObject, scale, emergeTime);
        }
    }
    /// <summary>
    /// 跳入完成
    /// </summary>
    void OnRunDropComplete()
    {
        if (enemy)
        {
            enemy.ReadyFroShoot = true;
        }
        if (anim != null)
        {
            anim.SetTrigger("isDroping");
        }
        isFinish = true;
    }

    void OnEmergeComplete(System.Object param)
    {
        if (enemy)
        {
            enemy.ReadyFroShoot = true;
        }
        if (anim != null)
        {
            anim.SetBool(param.ToString(), false);
        }
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
